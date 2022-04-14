using EEMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.TropicalWood
{
    [AutoloadEquip(EquipType.Body)]
    public class TropicalWoodChestplate : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Chestplate");
            Tooltip.SetDefault("9% increased all damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            //player.allDamage += 0.09f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<TropicalWoodItem>(), 15).AddTile(TileID.Anvils).Register();
        }
    }
}