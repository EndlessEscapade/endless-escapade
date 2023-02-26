using System.Collections.Generic;
using System.Linq;
using EndlessEscapade.Utilities.Extensions;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Items;

public class ItemFishingPlayer : ModPlayer
{
    public Dictionary<int, int> FishingLengthByType { get; private set; } = new();

    public override void SaveData(TagCompound tag) {
        tag.Add("fishingLengthKeys", FishingLengthByType.Keys.ToList());
        tag.Add("fishingLengthValues", FishingLengthByType.Values.ToList());
    }

    public override void LoadData(TagCompound tag) {
        FishingLengthByType = tag.GetDictionary<int, int>("fishingLengthKeys", "fishingLengthValues");
    }
}