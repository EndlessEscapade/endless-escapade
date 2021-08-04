using EEMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Foliage;

namespace EEMod.Items.Materials
{
    public class Kelp : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<BlueKelpTile>();
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.material = true;
        }
    }
}