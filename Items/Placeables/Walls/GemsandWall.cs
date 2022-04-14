using EEMod.Tiles.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Walls
{
    public class GemsandWall : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gemsand Wall");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.createWall = ModContent.WallType<GemsandWallTile>();
        }
    }
}