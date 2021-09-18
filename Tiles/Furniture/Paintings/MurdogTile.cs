using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Placeables.Furniture.Paintings;

namespace EEMod.Tiles.Furniture.Paintings
{
    public class MurdogTile : EETile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Murdog");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
            AnimationFrameHeight = 90;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 40, j * 40, 40, 40, ModContent.ItemType<Murdog>());
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (Main.xMas)
                frame = 1;
            else
                frame = 0;
        }
    }
}