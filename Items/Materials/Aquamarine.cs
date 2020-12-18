using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Ores;

namespace EEMod.Items.Materials
{
    public class Aquamarine : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20; //
            item.height = 20; //
            item.maxStack = 999; //
            item.value = Item.buyPrice(0, 0, 18, 0); //
            item.rare = ItemRarityID.Pink; //
            item.useAnimation = 15; //
            item.useTime = 10; //
            item.autoReuse = true;
            item.consumable = true;
            item.material = true;
            item.createTile = ModContent.TileType<AquamarineTile>();
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
        }
    }
}