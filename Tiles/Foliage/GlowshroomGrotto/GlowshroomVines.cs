using EEMod.Extensions;
using EEMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles.Foliage.GlowshroomGrotto
{
    public class GlowshroomVines : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = false;
            SoundType = SoundID.Grass;
            DustType = DustID.Plantera_Green;

            AddMapEntry(new Color(95, 143, 65));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);
            if (tile.IsActive && tile.type == Type)
            {
                WorldGen.KillTile(i, j + 1);
            }
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileAbove = Framing.GetTileSafely(i, j - 1);
            int type = -1;
            if (tileAbove.IsActive)
            {
                type = tileAbove.type;
            }

            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            if (WorldGen.genRand.NextBool(15) && !tileBelow.IsActive && !(tileBelow.LiquidType == 1))
            {
                bool placeVine = false;
                int yTest = j;
                while (yTest > j - 10)
                {
                    Tile testTile = Framing.GetTileSafely(i, yTest);
                    /*if (testTile.bottomSlope())
                    {
                        break;
                    }
                    else */if (!testTile.IsActive || testTile.type != ModContent.TileType<GemsandstoneTile>() || testTile.type == ModContent.TileType<LightGemsandstoneTile>())
                    {
                        yTest--;
                        continue;
                    }
                    placeVine = true;
                    break;
                }
                if (placeVine)
                {
                    tileBelow.type = Type;
                    tileBelow.IsActive = true;
                    WorldGen.SquareTileFrame(i, j + 1, true);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
                    }
                }
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int step = 0;
            Tile tile = Framing.GetTileSafely(i, j);
            while(tile.type == Type)
            {
                step++;
                tile = Framing.GetTileSafely(i,j - step);
            }
            Vector2 position = new Vector2(i * 16 + (float)Math.Sin(Main.GameUpdateCount/(90f + i%10) + i)*(step * step * 0.1f), j * 16).ForDraw() + zero;
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Tiles/Foliage/GlowshroomGrotto/GlowshroomVines").Value;
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);
            Main.spriteBatch.Draw(texture, position, rect, Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);

                        Color chosen = Color.Lerp(Color.Gold, Color.Goldenrod, Main.rand.NextFloat(1f));

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.005f));
            EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.5f, -0.1f)), Mod.Assets.Request<Texture2D>("Particles/SmallCircle").Value, 60, 0.75f, chosen, new SetMask(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, 0.8f), new AfterImageTrail(1f), new SetLighting(chosen.ToVector3(), 0.4f));

            return false;
        }
    }
}