using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Kelp;

public class KelpMoss : ModItem
{
    public override void SetDefaults() {
        Item.autoReuse = true;
        Item.consumable = true;
        Item.useTurn = true;

        Item.maxStack = Item.CommonMaxStack;

        Item.width = 20;
        Item.height = 18;

        Item.useStyle = ItemUseStyleID.Swing;

        Item.useTime = 10;
        Item.useAnimation = 15;
    }

    public override bool? UseItem(Player player) {
        if (Main.netMode == NetmodeID.Server || !IsHoveringRock()) {
            return false;
        }

        WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, ModContent.TileType<Tiles.Kelp.KelpMoss>(), false, true);

        player.cursorItemIconEnabled = true;
        player.inventory[player.selectedItem].stack--;

        return true;
    }

    public override void HoldItem(Player player) {
        if (!IsHoveringRock()) {
            return;
        }

        player.cursorItemIconEnabled = true;
    }

    private static bool IsHoveringRock() {
        var tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

        return tile.HasTile && tile.TileType == ModContent.TileType<Tiles.Kelp.KelpRock>();
    }
}
