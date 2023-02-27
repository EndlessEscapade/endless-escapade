using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items;

public class ItemFishingSets : ModSystem
{
    public static bool[] IsSmallFish { get; private set; }

    public static bool[] IsMediumFish { get; private set; }
    
    public static bool[] IsBigFish { get; private set; }
    
    public override void PostSetupContent() {
        IsSmallFish = ItemID.Sets.Factory.CreateBoolSet(
            ItemID.FrostMinnow,
            ItemID.GoldenCarp,
            ItemID.Hemopiranha,
            ItemID.NeonTetra,
            ItemID.PrincessFish,
            ItemID.RedSnapper,
            ItemID.Salmon,
            ItemID.Trout
        );
        
        IsMediumFish = ItemID.Sets.Factory.CreateBoolSet(
            ItemID.FrostMinnow,
            ItemID.GoldenCarp,
            ItemID.Hemopiranha,
            ItemID.NeonTetra,
            ItemID.PrincessFish,
            ItemID.RedSnapper,
            ItemID.Salmon,
            ItemID.Trout,
            ItemID.Bass
        );

        IsBigFish = ItemID.Sets.Factory.CreateBoolSet(
            ItemID.ChaosFish,
            ItemID.Damselfish,
            ItemID.DoubleCod,
            ItemID.FlarefinKoi,
            ItemID.Prismite,
            ItemID.VariegatedLardfish
        );
    }
}