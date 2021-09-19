using EEMod.Items.Placeables.Furniture.Paintings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture.Paintings
{
    public class OSPaintingTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("OSPainting");
            AddMapEntry(new Color(255, 168, 28), name);
            DustType = DustID.Silver;
            DisableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 24, j * 24, 24, 24, ModContent.ItemType<OSPainting>());
        }
    }
}