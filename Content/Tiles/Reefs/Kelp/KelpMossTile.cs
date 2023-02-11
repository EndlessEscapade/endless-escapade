using System;
using EndlessEscapade.Content.Items.Reefs.Kelp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Kelp;

public class KelpMossTile : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        TileID.Sets.Conversion.Moss[Type] = true;
        
        AddMapEntry(new Color(235, 166, 0));

        MineResist = 1.5f;
        
        HitSound = SoundID.Tink;
        DustType = DustID.GemAmber;

        ItemDrop = ModContent.ItemType<KelpRockItem>();
    }
    
    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
        Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
        
        Vector2 zero = Main.drawToScreen ? Vector2.Zero : new(Main.offScreenRange);
        Vector2 position = new Vector2(i, j) * 16f - Main.screenPosition + zero;

        Tile tile = Framing.GetTileSafely(i, j);
        
        Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);

        spriteBatch.Draw(texture, position, frame, Color.White, 0f, default, 1f, SpriteEffects.None, 0f);
    }
}