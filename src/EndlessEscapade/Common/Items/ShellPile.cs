using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items;

public sealed class ShellPile : GlobalItem
{
    public override void SetStaticDefaults() {
        ItemID.Sets.ExtractinatorMode[ItemID.ShellPileBlock] = ItemID.ShellPileBlock;
    }

    public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack) {
        if (extractType != ItemID.ShellPileBlock) {
            return;
        }

        ItemLoader.ExtractinatorUse(ref resultType, ref resultStack, ItemID.DesertFossil, extractinatorBlockType);

        if (!Main.rand.NextBool(10)) {
            return;
        }

        // TODO: Change to the shell pile gear when they're implemented.
        var accessories = new int[] {
            ItemID.Meowmere,
            ItemID.MechanicalWorm,
            ItemID.MechanicalEye
        };

        resultType = Main.rand.Next(accessories);
        resultStack = 1;
    }
}
