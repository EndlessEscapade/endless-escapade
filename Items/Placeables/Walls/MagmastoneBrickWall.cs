using EEMod.Tiles.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Walls
{
    public class MagmastoneBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magmastone Brick Wall");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 15;
            item.useTime = 7;
            item.consumable = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.createWall = ModContent.WallType<MagmastoneBrickWallTile>();
        }
    }
}