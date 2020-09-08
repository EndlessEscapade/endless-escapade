using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class LythenWarhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Warhammer");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 40;
            item.useTime = 40;
            item.shootSpeed = 12f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<LythenWarhammerProj>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}