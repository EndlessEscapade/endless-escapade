using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Kelp;

public class KelpMoss : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Reefs.Kelp.KelpMoss>());
        Item.createTile = -1;
    }

    public override void HoldItem(Player player) {
        if (IsHoveringRock()) {
            player.cursorItemIconEnabled = true;
        }
    }

    public override bool? UseItem(Player player) {
        if (Main.netMode == NetmodeID.Server) {
            return false;
        }

        if (IsHoveringRock()) {
            WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, ModContent.TileType<Tiles.Reefs.Kelp.KelpMoss>(), false, true);
            
            player.cursorItemIconEnabled = true;
            player.inventory[player.selectedItem].stack--;
        }

        return true;
    }

    private static bool IsHoveringRock() {
        var tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

        return tile.HasTile && tile.TileType == ModContent.TileType<Tiles.Reefs.Kelp.KelpRock>();
    }
}