using System;
using System.Collections.Generic;
using System.Linq;
using EndlessEscapade.Utilities.Sets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Items;

public class ItemFishingLength : GlobalItem
{
    public static Dictionary<int, ItemFishingLengthData> FishingDataByType { get; private set; } = new();

    public override void SaveData(Item item, TagCompound tag) {
        tag["fishingDataKeys"] = FishingDataByType.Keys.ToList();
        tag["fishingDataValues"] = FishingDataByType.Values.ToList();
    }

    public override void LoadData(Item item, TagCompound tag) {
        var fishingDataKeys = tag.Get<List<int>>("fishingDataKeys");
        var fishingDataValues = tag.Get<List<ItemFishingLengthData>>("fishingDataValues");

        var zipped = fishingDataKeys.Zip(
            fishingDataValues,
            (x, y) => new {
                Key = x,
                Value = y
            }
        );

        FishingDataByType = zipped.ToDictionary(x => x.Key, x => x.Value);
    }

    public override bool OnPickup(Item item, Player player) {
        bool isFish = TryGetFishLength(item, player, out int length);

        if (isFish) {
            FishingDataByType[item.type].LengthsCatched.Add(length);
            FishingDataByType[item.type].DatesCatched.Add(DateTime.Now);
        }

        return true;
    }

    private static bool TryGetFishLength(Item item, Player player, out int length) {
        bool isFish = ItemSets.IsSmallFish[item.type] || ItemSets.IsMediumFish[item.type] || ItemSets.IsBigFish[item.type];

        float minCmLength = 0f;
        float maxCmLength = 0f;

        if (ItemSets.IsSmallFish[item.type]) {
            minCmLength = 8f;
            maxCmLength = 17f;
        }

        if (ItemSets.IsMediumFish[item.type]) {
            minCmLength = 12f;
            maxCmLength = 33f;
        }

        if (ItemSets.IsBigFish[item.type]) {
            minCmLength = 18f;
            maxCmLength = 45f;
        }

        float skillModifier = 1f + player.fishingSkill / 100f;
        float preciseLength = Main.rand.NextFloat(minCmLength, maxCmLength) * skillModifier;

        length = (int)MathF.Floor(preciseLength);

        return isFish;
    }

    public record class ItemFishingLengthData
    {
        public List<int> LengthsCatched { get; set; } = new();
        public List<DateTime> DatesCatched { get; set; } = new();
    }
}