using EndlessEscapade.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Tiles;

[Autoload(Side = ModSide.Client)]
public sealed class LilypadCollisionEffects : ModPlayer
{
    public override void PreUpdateMovement() {
        var colliding = false;

        var bottomTile = Framing.GetTileSafely(Player.Bottom);
        var bottomLeftTile = Framing.GetTileSafely(Player.BottomLeft);
        var bottomRightTile = Framing.GetTileSafely(Player.BottomRight);

        colliding |= bottomTile.HasTile &&
                     (bottomTile.TileType == ModContent.TileType<Lilypad>() || bottomTile.TileType == ModContent.TileType<SmallLilypad>());

        colliding |= bottomLeftTile.HasTile &&
                     (bottomLeftTile.TileType == ModContent.TileType<Lilypad>() || bottomLeftTile.TileType == ModContent.TileType<SmallLilypad>());

        colliding |= bottomRightTile.HasTile &&
                     (bottomRightTile.TileType == ModContent.TileType<Lilypad>() || bottomRightTile.TileType == ModContent.TileType<SmallLilypad>());

        if (!colliding) {
            return;
        }

        Player.DryCollision(true, false);
    }
}
