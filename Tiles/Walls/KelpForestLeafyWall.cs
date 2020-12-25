using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Walls;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Tiles.Walls
{
    public class KelpForestLeafyWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(67, 47, 155));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<MagmastoneSlabWall>();
            soundStyle = 1;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 pos = new Vector2(i * 16, j * 16);
            float randParam = i * j;
            for (int a = 0; a < 4; a++)
            { 
                for (int b = 0; b < 4; b++)
                {
                    Helpers.Draw(EEMod.instance.GetTexture("ForegroundParticles/Leaf"), pos.ForDraw() + zero + new Vector2(a*6 + ((randParam*a + a)%8 - 4) ,b*6 - 4 + ((randParam * b + b) % 8 )), Color.White, 0.7f);
                }
            }
        }
    }
}