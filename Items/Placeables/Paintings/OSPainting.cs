using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.DevPaintings;

namespace EEMod.Items.Placeables.DevPaintings
{
    public class OSPainting : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trillion Dollar Company");
        }

        public override void SetDefaults()
        {
            //item.useStyle = 1;
            //item.useTurn = true;
            //item.useAnimation = 15;
            //item.useTime = 10;
            //item.autoReuse = true;
            //item.maxStack = 99;
            //item.consumable = true;
            //item.createTile = 285 + type - 2174;
            //item.width = 12;
            //item.height = 12;

            item.CloneDefaults(ItemID.WoodenCrate);
            item.createTile = ModContent.TileType<OSPaintingTile>();
        }
    }
}