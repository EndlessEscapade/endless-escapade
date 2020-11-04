using EEMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles
{
    public class GreenKelpTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileBlendAll[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = false;
            AddMapEntry(new Color(68, 89, 195));
            //Main.tileCut[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<Kelp>();
            soundStyle = SoundID.Grass;
            mineResist = 1f;
            minPick = 0;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 0, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<GemsandTile>(), ModContent.TileType<GreenKelpTile>(), ModContent.TileType<LightGemsandTile>() };
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.addTile(Type);
            animationFrameHeight = 18;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {

        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j - 1);
            if (!tile.active() && Main.rand.Next(4) == 0)
            {
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<GreenKelpTile>());
                NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<GreenKelpTile>(), 0, 0, -1, -1);
            }
        }

        int b = Main.rand.Next(0, 9);
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 6)
            {
                b++;
                if (b >= 8)
                {
                    b = 0;
                }
                frame = b;
                frameCounter = 0;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);
            if (WorldGen.InWorld(i, j))
            {
                if (!tile.active()
                    || tile.type != ModContent.TileType<GemsandTile>()
                    && tile.type != ModContent.TileType<LightGemsandTile>()
                    && tile.type != ModContent.TileType<DarkGemsandTile>()
                    && tile.type != ModContent.TileType<LightGemsandstoneTile>()
                    && tile.type != ModContent.TileType<GemsandstoneTile>()
                    && tile.type != ModContent.TileType<DarkGemsandstoneTile>())
                {
                    WorldGen.KillTile(i, j);
                }
            }
            if (!Main.tileSolid[tile.type])
                return false;
            Vector2 pos = new Vector2((i + 12) * 16, (j + 14) * 16);
            Vector2 sprout = new Vector2((float)(Math.Sin(Main.time / 60f + i) * 20), 30 * (i * j % 10) + 50);
            Vector2 end = pos - sprout;
            Vector2 lerp = Vector2.Lerp(pos, end, 0.5f);
            float dist = (end - pos).Length();
            Texture2D tex = EEMod.instance.GetTexture("Tiles/GreenKelpTile");


            int noOfFrames = 10;
            int frame = (int)((Main.time / 10f + j * i) % noOfFrames);


            if (Main.tileSolid[tile.type] && tile.active())
            {
                Helpers.DrawBezier(Main.spriteBatch, tex, "", Lighting.GetColor(i, j), end, pos, pos - new Vector2(0, sprout.Y - 50), pos - new Vector2(0, sprout.Y - 50), (tex.Height / (noOfFrames * 2.2f)) / dist, 0f, frame, noOfFrames, 3);
                Helpers.DrawParticlesAlongBezier(end, pos, pos - new Vector2(0, sprout.Y - 50), pos - new Vector2(0, sprout.Y - 50), (tex.Height / (noOfFrames * 2.2f)) / dist, Color.Green, 2, 100);
            }

            return false;
        }
    }
}