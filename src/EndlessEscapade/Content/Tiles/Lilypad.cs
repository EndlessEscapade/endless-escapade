using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EndlessEscapade.Content.Tiles;

public class Lilypad : ModTile
{
    public override void Load() {
        On_Main.DrawTileInWater += DrawTileInWaterHook;
    }

    public override void SetStaticDefaults() {
        Main.tileLighted[Type] = true;
        Main.tileCut[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileNoFail[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileID.Sets.TileCutIgnore.Regrowth[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);

        TileObjectData.newTile.WaterDeath = false;
        TileObjectData.newTile.LavaDeath = true;

        TileObjectData.newTile.WaterPlacement = LiquidPlacement.OnlyInLiquid;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;

        TileObjectData.newTile.Origin = Point16.Zero;

        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Height, 0);

        TileObjectData.addTile(Type);

        AddMapEntry(new Color(135, 196, 26));

        DustType = DustID.JungleGrass;
        HitSound = SoundID.Grass;
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
        return false;
    }

    private void DrawTileInWaterHook(On_Main.orig_DrawTileInWater orig, Vector2 drawoffset, int x, int y) {
        orig(drawoffset, x, y);

        var tile = Framing.GetTileSafely(x, y);

        if (!tile.HasTile || tile.TileType != Type) {
            return;
        }

        var effects = x % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        var texture = TextureAssets.Tile[Type].Value;

        var offset = tile.LiquidAmount / 16f - 10f;

        var zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
        var position = new Vector2(x, y) * 16f - Main.screenPosition + zero - new Vector2(0f, offset);

        var color = Lighting.GetColor(x, y);

        Main.spriteBatch.Draw(texture, position, null, color, 0f, default, 1f, effects, 0f);
    }
}
