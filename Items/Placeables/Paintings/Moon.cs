using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.DevPaintings;

namespace EEMod.Items.Placeables.Paintings
{
    public class Moon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.width = 12;
            item.height = 12;

            item.createTile = ModContent.TileType<MoonTile>();
        }
    }
}