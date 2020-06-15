using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Armor.Quartz
{
    [AutoloadEquip(EquipType.Legs)]
    public class QuartzLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Greaves");
            Tooltip.SetDefault("6 defense\n15% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Pink;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.statDefense += 6;
            player.moveSpeed *= 1.15f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 8);
            recipe.AddIngredient(ItemID.PlatinumGreaves, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 8);
            recipe.AddIngredient(ItemID.GoldGreaves, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
