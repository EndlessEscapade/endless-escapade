using Terraria.ID;

namespace EndlessEscapade.Utilities.Sets;

public static class ItemSets
{
    public static readonly bool[] IsSmallFish = ItemID.Sets.Factory.CreateBoolSet(
        ItemID.FrostMinnow,
        ItemID.GoldenCarp,
        ItemID.Hemopiranha,
        ItemID.NeonTetra,
        ItemID.PrincessFish,
        ItemID.RedSnapper,
        ItemID.Salmon,
        ItemID.Trout
    );

    public static readonly bool[] IsMediumFish = ItemID.Sets.Factory.CreateBoolSet(
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

    public static readonly bool[] IsBigFish = ItemID.Sets.Factory.CreateBoolSet(
        ItemID.ChaosFish,
        ItemID.Damselfish,
        ItemID.DoubleCod,
        ItemID.FlarefinKoi,
        ItemID.Prismite,
        ItemID.VariegatedLardfish
    );
}