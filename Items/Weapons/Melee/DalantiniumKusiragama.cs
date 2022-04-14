using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class DalantiniumKusiragama : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Dagger");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 70;
            Item.useTime = 24;
            Item.shootSpeed = 4;
            Item.knockBack = 6.5f;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<DalantiniumKusiragamaProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumBar>(), 14).AddTile(TileID.Anvils).Register();
        }
    }
}