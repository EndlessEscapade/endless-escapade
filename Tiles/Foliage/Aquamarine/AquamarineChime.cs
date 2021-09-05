using EEMod.Extensions;
using EEMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles.Foliage.Aquamarine
{
    public class AquamarineChime : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileCut[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = false;
            soundType = SoundID.Grass;
            dustType = DustID.PurpleCrystalShard;

            AddMapEntry(new Color(95, 143, 65));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);
            if (tile.active() && tile.type == Type)
            {
                WorldGen.KillTile(i, j + 1);
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            int frameX = 0;
            int frameY = (Main.tile[i, j + 1].type == ModContent.TileType<AquamarineChime>() ? 0 : 18);

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2((i * 16) + (float)Math.Sin(i + j + (Main.GameUpdateCount / 30f)), j * 16).ForDraw() + zero;

            Texture2D texture = ModContent.GetInstance<EEMod>().GetTexture("Tiles/Foliage/Aquamarine/AquamarineChime");
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);

            Main.spriteBatch.Draw(texture, position, rect, Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);

            if (Main.tile[i, j - 1].active() == false) WorldGen.KillTile(i, j);

            Lighting.AddLight(new Vector2(i, j) * 16, Color.Lerp(Color.Pink, Color.Cyan, Math.Sin(((i + j) / 20f) + Main.GameUpdateCount / 60f).PositiveSin()).ToVector3() / 3f);

            return false;
        }
    }
}