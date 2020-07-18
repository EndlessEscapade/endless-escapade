/*using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using EEMod.Projectiles.Melee;
using EEMod.Items.Materials;

namespace EEMod.Items.Weapons.Melee
{
    public class QuartzDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Dagger");
            Tooltip.SetDefault("The power of a brittle knife within your hands");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.damage = 16;
            item.melee = true;
            item.noMelee = true;
            item.consumable = false;
            item.noUseGraphic = true;
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 14;
            item.knockBack = 0f;
            item.UseSound = SoundID.Item18;
            item.autoReuse = true;
            item.maxStack = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.shoot = ModContent.ProjectileType<QuartzDaggerProj>();
            item.shootSpeed = 11f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ThrowingKnife, 50);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}*/
