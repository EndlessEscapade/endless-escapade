using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Extensions;
using System;

namespace EEMod.Tiles.Foliage.GlowshroomGrotto
{
    public class OrangeMushroom2x2 : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {

        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color chosen = Color.Lerp(Color.Gold, Color.Goldenrod, Main.rand.NextFloat(1f));

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.0075f));
            EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.5f, -0.1f)), Mod.Assets.Request<Texture2D>("Particles/SmallCircle").Value, 60, 0.75f, chosen, new SetMask(EEMod.Instance.Assets.Request<Texture2D>("Textures/RadialGradient").Value, 0.8f), new AfterImageTrail(1f), new SetLighting(chosen.ToVector3(), 0.4f));

            Tile tile = Framing.GetTileSafely(i, j);
            int frameX = tile.frameX;
            int frameY = tile.frameY;

            int styleOffset = (tile.frameY > 16 ? 32 : 0);

            float lerpVal = (i - (tile.frameX / 16f)) + (j - (tile.frameY / 16f));

            Color color = Color.White * (float)(0.8f + (Math.Sin(lerpVal + Main.GameUpdateCount / 20f) / 5f));

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);

            Vector2 offsetOrig = new Vector2(16 - frameX, 32 - frameY + styleOffset);

            Texture2D tex = Mod.Assets.Request<Texture2D>("Tiles/Foliage/GlowshroomGrotto/OrangeMushroom2x2Cap").Value;

            Main.spriteBatch.Draw(tex, position + offsetOrig, rect, Lighting.GetColor(i, j), (float)Math.Sin((Main.GameUpdateCount / 35f) + lerpVal) / 5f, offsetOrig, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, position + offsetOrig, rect, color, (float)Math.Sin((Main.GameUpdateCount / 35f) + lerpVal) / 5f, offsetOrig, 1f, SpriteEffects.None, 0f);
        }
    }
}