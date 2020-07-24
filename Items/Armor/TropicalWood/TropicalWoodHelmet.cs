using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.TropicalWood
{
    [AutoloadEquip(EquipType.Head)]
    public class TropicalWoodHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Helmet");
            Tooltip.SetDefault("5% increased melee speed");
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
            player.meleeSpeed += 0.05f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When below 40% life, you gain 8 defense";
            if (player.statLife <= player.statLifeMax * 0.40f)
            {
                player.statDefense += 8;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}