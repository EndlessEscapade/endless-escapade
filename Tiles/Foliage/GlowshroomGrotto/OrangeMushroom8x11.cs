using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Extensions;
using System;
using EEMod.Systems;

namespace EEMod.Tiles.Foliage.GlowshroomGrotto
{
    public class OrangeMushroom8x11 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 11;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {

        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            Tile t = Main.tile[i, j];
            if (t.frameX == 0 && t.frameY == 0)
            {
                Main.specX[nextSpecialDrawIndex] = i;
                Main.specY[nextSpecialDrawIndex] = j;
                nextSpecialDrawIndex++;
            }
        }

        private string path = "Tiles/Foliage/GlowshroomGrotto/";
        private string[] tentacleTex = new string[]
        {
            "glowvinetop",
            "glowvinemid1",
            "glowvinemid2",
            "glowvinebottom"
        };

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 0)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }

                Tile tile = Main.tile[i, j];

                Color color = Color.White * (float)(0.8f + (Math.Sin((i - (tile.frameX / 18f)) + (j - (tile.frameY / 18f)) + Main.GameUpdateCount / 20f) / 5f));

                for (int l = 0; l < 3; l++)
                {
                    Vector2 orig = Vector2.Zero;
                    if(l == 0) orig = new Vector2((i * 16) + 26, (j * 16) + 70 - 6);

                    if(l == 1) orig = new Vector2((i * 16) + 76, (j * 16) + 56 - 6);

                    if (l == 2) orig = new Vector2((i * 16) + 106, (j * 16) + 36 - 6);

                    Vector2 prev = new Vector2(0, 18);
                    for (int k = 0; k < 4; k++)
                    {
                        Texture2D tex = mod.GetTexture(path + tentacleTex[k]);

                        Vector2 next = prev + new Vector2(0, 18).RotatedBy((-Math.Sin((Main.GameUpdateCount / 40f) + l + i + j).PositiveSin() / 5f) * (k / 2f) - (l * 0.1f));

                        Vector2 pos = (orig + next).ForDraw() + zero;

                        float rot = (prev - next).ToRotation();

                        Main.spriteBatch.Draw(tex, pos, tex.Bounds, color, rot + MathHelper.PiOver2, new Vector2(11, 0), 1f, SpriteEffects.None, 0f);

                        prev = next;
                    }
                }

                Vector2 drawPos = new Vector2(i * 16, j * 16).ForDraw() + zero;

                Main.spriteBatch.Draw(mod.GetTexture("Tiles/Foliage/GlowshroomGrotto/OrangeMushroom8x11Mushroots"), new Rectangle((int)drawPos.X, (int)drawPos.Y, 128, 176), new Rectangle(0, 0, 128, 176), Lighting.GetColor(i, j));

                Main.spriteBatch.Draw(mod.GetTexture("Tiles/Foliage/GlowshroomGrotto/OrangeMushroom8x11Cap"), new Rectangle((int)drawPos.X, (int)drawPos.Y, 128, 176), new Rectangle(0, 0, 128, 176), color);
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color chosen = Color.Lerp(Color.Gold, Color.Goldenrod, Main.rand.NextFloat(1f));

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.005f));
            EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.5f, -0.1f)), mod.GetTexture("Particles/SmallCircle"), 60, 0.75f, chosen, new SetMask(ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient"), 0.5f), new AfterImageTrail(1f), new SetLighting(chosen.ToVector3(), 0.2f));

            if (Framing.GetTileSafely(i, j).frameX >= 80 && Framing.GetTileSafely(i, j).frameY >= 144)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }

                Tile tile = Framing.GetTileSafely(i, j);
                int frameX = tile.frameX;
                int frameY = tile.frameY;

                Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
                Rectangle rect = new Rectangle(frameX - 80, frameY - 144, 16, 16);

                Vector2 offsetOrig = new Vector2(24 - (frameX - 80), 32 - (frameY - 144));

                Texture2D tex = mod.GetTexture("Tiles/Foliage/GlowshroomGrotto/OrangeMushroom8x11Minishroom");

                float lerpVal = (i - (tile.frameX / 16f)) + (j - (tile.frameY / 16f));

                Color color = Color.White * (float)(0.8f + (Math.Sin(lerpVal + Main.GameUpdateCount / 35f) / 5f));

                Main.spriteBatch.Draw(tex, position + offsetOrig, rect, color, (float)Math.Sin((Main.GameUpdateCount / 60f) + lerpVal) / 5f + 0.1f, offsetOrig, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}