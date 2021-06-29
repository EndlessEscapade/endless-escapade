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

namespace EEMod.Tiles.Foliage
{
    public class SeagrassTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = true;

            AddMapEntry(new Color(28, 78, 47));
            //Main.tileCut[Type] = true;
            dustType = DustID.Grass;
            drop = ModContent.ItemType<Kelp>();
            soundStyle = SoundID.Grass;
            mineResist = 1f;
            minPick = 0;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 0, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<CoralSandTile>(), ModContent.TileType<SeagrassTile>() };
            TileObjectData.newTile.AnchorTop = default;

            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };

            TileObjectData.addTile(Type);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {

        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (Main.tile[i, j + 1].type != ModContent.TileType<SeagrassTile>() && Main.tile[i, j + 1].type != ModContent.TileType<CoralSandTile>())
            {
                Main.tile[i, j].ClearTile();
            }

            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j - 1);
            if (!tile.active() && Main.rand.Next(4) == 0)
            {
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<SeagrassTile>());
                NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<SeagrassTile>(), 0, 0, -1, -1);
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);

            Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Tiles/Foliage/SeagrassTile");

            int frame = 1;

            if(tile.type == ModContent.TileType<SeagrassTile>() && (!Framing.GetTileSafely(i, j - 1).active() || Framing.GetTileSafely(i, j - 1).type != ModContent.TileType<SeagrassTile>()))
            {
                frame = 0;
            }

            if (tile.type != ModContent.TileType<SeagrassTile>() && Framing.GetTileSafely(i, j - 1).type == ModContent.TileType<SeagrassTile>())
            {
                frame = 2;
            }

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;

            Rectangle rect = new Rectangle(0, frame * 16, 16, 16);

            Main.spriteBatch.Draw(tex, position, rect, Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);

            return false;
        }
    }
}