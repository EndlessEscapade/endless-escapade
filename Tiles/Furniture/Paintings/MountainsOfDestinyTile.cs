using EEMod.Items.Placeables.Paintings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture.Paintings
{
    public class MountainsOfDestinyTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("OSPainting");
            AddMapEntry(new Color(255, 168, 28), name);
            dustType = 11;
            disableSmartCursor = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 24, j * 24, 24, 24, ModContent.ItemType<MountainsOfDestiny>());
        }
    }
}