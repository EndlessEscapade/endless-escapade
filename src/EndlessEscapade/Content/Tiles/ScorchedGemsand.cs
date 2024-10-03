using EndlessEscapade.Content.Tiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles;

public class ScorchedGemsand : CompositeTile
{
    public override int HorizontalSheetCount { get; } = 4;

    public override int VerticalSheetCount { get; } = 2;

    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileID.Sets.Conversion.Sand[Type] = true;

        AddMapEntry(new Color(106, 96, 95));

        MineResist = 1f;
        HitSound = SoundID.Dig;
        DustType = DustID.Ash;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
        var texture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

        var zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
        var position = new Vector2(i, j) * 16f - Main.screenPosition + zero;

        var tile = Framing.GetTileSafely(i, j);

        var frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);

        spriteBatch.Draw(texture, position, frame, Color.White, 0f, default, 1f, SpriteEffects.None, 0f);
    }
}
