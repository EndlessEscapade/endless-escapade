using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class LunaJellyBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luna Jelly Bottle");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;

            item.createTile = ModContent.TileType<LunaJellyBottleTile>();
        }
    }
}