using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.Dalantinium
{
    [AutoloadEquip(EquipType.Body)]
    public class DalantiniumPlatemail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Platemail");
            Tooltip.SetDefault("9% increased all damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Green;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.09f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}