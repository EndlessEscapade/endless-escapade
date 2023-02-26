using System.Collections.Generic;
using System.Linq;
using EndlessEscapade.Utilities.Extensions;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Items;

public class ItemFishingPlayer : ModPlayer
{
    public class ItemFishingData
    {
        public readonly List<int> Lengths = new();
    }
    
    public Dictionary<int, ItemFishingData> FishingDataByType { get; private set; } = new();

    public override void SaveData(TagCompound tag) {
        tag.Add("fishingDataKeys", FishingDataByType.Keys.ToList());
        tag.Add("fishingDataValues", FishingDataByType.Values.ToList());
    }

    public override void LoadData(TagCompound tag) {
        FishingDataByType = tag.GetDictionary<int, ItemFishingData>("fishingDataKeys", "fishingDataValues");
    }
}