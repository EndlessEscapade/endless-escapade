using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Placeables.Furniture.Trophies;

namespace EEMod.Tiles.Furniture
{
    public class TrophiesTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            DisableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Trophy");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            int item = 0;
            switch (TileFrameX / 54)
            {
                case 0:
                    item = ModContent.ItemType<HydrosTrophy>();
                    break;

                case 1:
                    item = ModContent.ItemType<OmenTrophy>();
                    break;
            }
            Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, item);
        }
    }
}