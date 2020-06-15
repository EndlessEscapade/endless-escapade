using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Armor.Quartz
{
    [AutoloadEquip(EquipType.Body)]
    public class QuartzChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Chestplate");
            Tooltip.SetDefault("7 defense\n12% increased weapon speed");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 16;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Pink;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.statDefense += 7;
            player.meleeSpeed *= 1.12f;
            player.GetModPlayer<EEPlayer>().isQuartzChestOn = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 9);
            recipe.AddIngredient(ItemID.PlatinumChainmail, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 9);
            recipe.AddIngredient(ItemID.GoldChainmail, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
