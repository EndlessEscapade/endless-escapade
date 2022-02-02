using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture
{
    public class MoyaiTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            //TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Moyai");
            ItemDrop = ModContent.ItemType<Moyai>();
            AddMapEntry(new Color(20, 60, 20), name);
            DisableSmartCursor = true;
            DustType = DustID.Dirt;
        }

        private bool isIntersecting;

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;
            Player player = Main.LocalPlayer;
            if (new Rectangle((int)player.position.X / 16, (int)player.position.Y / 16, 1, 2).Intersects(new Rectangle(i, j, 1, 1)) && !isIntersecting)
            {
                isIntersecting = true;
            }
            else
            {
                isIntersecting = false;
            }
        }
    }
}