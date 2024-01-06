using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Globals.Items;

public sealed class ShellPile : GlobalItem
{
    public override void SetStaticDefaults() {
        ItemID.Sets.ExtractinatorMode[ItemID.ShellPileBlock] = ItemID.ShellPileBlock;
    }

    public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack) {
        if (extractType != ItemID.ShellPileBlock) {
            return;
        }

        resultType = ItemID.SandBlock;
        resultStack = 1;
    }
}
