using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture.Coral
{
   
    public class GroundGlowCoral3 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coral Lamp");
            AddMapEntry(new Color(0, 100, 200), name);
            dustType = DustID.Dirt;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.frameX < 18)
            {
                r = 0.05f;
                g = 0.05f;
                b = 0.05f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.003f));
            EEMod.Particles.Get("Main").SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), mod.GetTexture("Particles/Cross"), 30, 2, Color.Lerp(Color.Goldenrod, Color.Yellow, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.98f), new RotateVelocity(Main.rand.NextFloat(-.01f, .01f)), new RotateTexture(0.02f), new AfterImageTrail(0.7f));

            if (tile != null && tile.active() && tile.type == Type)
            {
                int frameX = tile.frameX;
                int frameY = tile.frameY;
                const int width = 20;
                const int offsetY = 0;
                const int height = 16;
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Color color = Color.White;
                Vector2 position = new Vector2(x: i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + 2, y: j * 16 - (int)Main.screenPosition.Y + offsetY) + zero;
                Rectangle rect = new Rectangle(frameX, frameY, width, height);
                color *= (float)Math.Sin(Main.GameUpdateCount / 60f + i + j) * 0.5f + 0.5f;
                for (int k = 0; k < 7; k++)
                {
                    Main.spriteBatch.Draw(EEMod.instance.GetTexture("Tiles/Furniture/Coral/GroundGlowCoralGlow3"), position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}