using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items;

public class ItemFishingLength : GlobalItem
{
    private static readonly SetFactory factory = new(ItemLoader.ItemCount);

    public static readonly bool[] IsSmallFish = factory.CreateBoolSet(
        ItemID.FrostMinnow,
        ItemID.GoldenCarp,
        ItemID.Hemopiranha,
        ItemID.NeonTetra,
        ItemID.PrincessFish,
        ItemID.RedSnapper,
        ItemID.Salmon,
        ItemID.Trout
    );

    public static readonly bool[] IsAverageFish = factory.CreateBoolSet(
        ItemID.FrostMinnow,
        ItemID.GoldenCarp,
        ItemID.Hemopiranha,
        ItemID.NeonTetra,
        ItemID.PrincessFish,
        ItemID.RedSnapper,
        ItemID.Salmon,
        ItemID.Trout
    );

    public static readonly bool[] IsBigFish = factory.CreateBoolSet(
        ItemID.ChaosFish,
        ItemID.Damselfish,
        ItemID.DoubleCod,
        ItemID.FlarefinKoi,
        ItemID.Prismite,
        ItemID.VariegatedLardfish
    );

    // TODO: Fisherman's Log.
}