using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Items;

public class ItemFishingPickup : GlobalItem
{
    public bool HasBeenCatched { get; private set; }

    public override bool InstancePerEntity { get; } = true;

    public override void SaveData(Item item, TagCompound tag) {
        tag.Add("hasBeenCatched", HasBeenCatched);
    }

    public override void LoadData(Item item, TagCompound tag) {
        HasBeenCatched = tag.GetBool("hasBeenCatched");
    }

    public override bool OnPickup(Item item, Player player) {
        bool isFish = TryGetFishLength(item, player, out int length);
        bool hasPlayer = player.TryGetModPlayer(out ItemFishingPlayer fishingPlayer);
        
        if (!HasBeenCatched && isFish && hasPlayer) {
            if (fishingPlayer.FishingLengthByType.TryGetValue(item.type, out var data)) {
                data.Add(length);
            }
            else {
                data = new List<int>();
                data.Add(length);
                
                fishingPlayer.FishingLengthByType.Add(item.type, data);
            }

            HasBeenCatched = true;
        }

        return true;
    }

    private static bool TryGetFishLength(Item item, Player player, out int length) {
        bool isFish = ItemFishingSets.IsSmallFish[item.type] || ItemFishingSets.IsMediumFish[item.type] || ItemFishingSets.IsBigFish[item.type];

        float minCmLength = 0f;
        float maxCmLength = 0f;

        if (ItemFishingSets.IsSmallFish[item.type]) {
            minCmLength = 8f;
            maxCmLength = 17f;
        }

        if (ItemFishingSets.IsMediumFish[item.type]) {
            minCmLength = 12f;
            maxCmLength = 33f;
        }

        if (ItemFishingSets.IsBigFish[item.type]) {
            minCmLength = 18f;
            maxCmLength = 45f;
        }

        float skillModifier = 1f + player.fishingSkill / 100f;
        float preciseLength = Main.rand.NextFloat(minCmLength, maxCmLength) * skillModifier;

        length = (int)MathF.Floor(preciseLength);

        return isFish;
    }
}