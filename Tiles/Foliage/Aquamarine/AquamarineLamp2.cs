using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles.Enemy;
using EEMod.Extensions;
using Terraria.ID;
using EEMod.Prim;
using System.Linq;

namespace EEMod.Tiles.Foliage.Aquamarine
{
    public class AquamarineLamp2 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        private int frameCounter;
        private int frame;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;

            frameCounter++;
            if(frameCounter >= 6)
            {
                frame++;
                frameCounter = 0;
                if(frame > 7)
                {
                    frame = 0;
                }
            }

            if (frameX == 0 && frameY == 0)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }

                Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X + 18, j * 16 - (int)Main.screenPosition.Y + 24) + zero;
                Rectangle rect = new Rectangle(0, frame * 16, 14, 16);

                int bigTimeBetween = 200;
                int timeBetween = 70;
                float heartBeat = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));

                Helpers.DrawAdditiveFunky(mod.GetTexture("Textures/RadialGradient"), position + new Vector2(5, 5), Color.Lerp(Color.Cyan, Color.Magenta, Math.Sin(Main.GameUpdateCount / 20f).PositiveSin()), 1f, 0.8f);

                Main.spriteBatch.Draw(mod.GetTexture("Tiles/Foliage/Aquamarine/AquamarineLamp2Glow"), position, rect, Lighting.GetColor(i + 1, j + 1), 0f, default, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(mod.GetTexture("Tiles/Foliage/Aquamarine/AquamarineLamp2Glow"), position, rect, Color.White * heartBeat, 0f, default, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
