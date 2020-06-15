using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Ranger
{
    public class DuneBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dune Blaster");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 11;
            item.useTime = 34;
            item.useAnimation = 34;
            item.width = 20;
            item.height = 20;
            item.shoot = 10;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 35f;
            item.UseSound = SoundID.Item11;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int projNum = 3;
            for (int i = 0; i < projNum; i++)
            {
                Vector2 pertSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                Projectile.NewProjectile(position.X, position.Y, pertSpeed.X, pertSpeed.Y, type, damage, knockBack, player.whoAmI);
            }

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HardenedSand, 20);
            //recipe.AddRecipeGroup("EEMod:SilverBar", 6);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}