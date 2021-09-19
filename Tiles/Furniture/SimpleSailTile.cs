using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using EEMod.Items.Placeables.Furniture;

namespace EEMod.Tiles.Furniture
{
    public class SimpleSailTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.Platforms[Type] = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            // TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            AddMapEntry(new Color(200, 200, 200));
            DustType = DustID.t_LivingWood;
            ItemDrop = ModContent.ItemType<SimpleSail>();
            DisableSmartCursor = true;
            AdjTiles = new int[] { TileID.Platforms };
        }

        public override void PostSetDefaults()
        {
            Main.tileNoSunLight[Type] = false;
        }

        private int frame = 3;
        private int height = 0;
        private bool opening = true;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Framing.GetTileSafely(i + 1, j).type == ModContent.TileType<SimpleSailTile>() && Framing.GetTileSafely(i - 1, j).type != ModContent.TileType<SimpleSailTile>())
                frame = 1;
            else if (Framing.GetTileSafely(i - 1, j).type == ModContent.TileType<SimpleSailTile>() && Framing.GetTileSafely(i + 1, j).type != ModContent.TileType<SimpleSailTile>())
                frame = 2;
            else if (Framing.GetTileSafely(i - 1, j).type == ModContent.TileType<SimpleSailTile>() && Framing.GetTileSafely(i + 1, j).type == ModContent.TileType<SimpleSailTile>())
                frame = 0;
            else
                frame = 3;

            if (Main.time % 5 == 0)
            {
                if (opening && height < 4)
                    height++;
                if (!opening && height > 0)
                    height--;
            }

            Texture2D tex = Mod.Assets.Request<Texture2D>("Tiles/Furniture/SimpleSailSails").Value;
            Main.spriteBatch.Draw(tex, new Rectangle((i * 16) - (int)Main.screenPosition.X + (tex.Width / 2) + 32, (j * 16) - (int)Main.screenPosition.Y + tex.Height * 2, 16, 96), new Rectangle((frame * 80) + (height * 16), 0, 16, 96), Lighting.GetColor(i, j));
        }

        public override bool RightClick(int i, int j)
        {
            opening = !opening;
            return true;
        }
    }
}