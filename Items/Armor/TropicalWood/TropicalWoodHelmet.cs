using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.TropicalWood
{
    [AutoloadEquip(EquipType.Head)]
    public class TropicalWoodHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Helmet");
            Tooltip.SetDefault("2% increased summon damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TropicalWoodChestplate>() && legs.type == ModContent.ItemType<TropicalWoodBoots>();
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.02f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+1 max minions";
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.TropicalWood>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}