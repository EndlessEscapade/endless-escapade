using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Weapons.Mage
{
    public class DalantiniumKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Knives");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 13;
            item.magic = true;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 21);
            item.mana = 7;
            item.shootSpeed = 4f;
            item.useTime = 360;
            item.useAnimation = 360;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<DalantiniumFan>();
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        int powerUp;
        public override void HoldItem(Player player)
        {
            if(powerUp < 2000)
            powerUp++;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {  
            if (player.altFunctionUse == 0)
            {
                type = ModContent.ProjectileType<DalantiniumFan>();
                item.shoot = ModContent.ProjectileType<DalantiniumFan>();
                Projectile projectile = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                powerUp += 200;
                (projectile.modProjectile as DalantiniumFan).boost = powerUp;
                if (Main.netMode != NetmodeID.Server)
                {
                    EEMod.Prims.CreateTrail(projectile);
                }
            }
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<DalantiniumFanAlt>();
                item.shoot = ModContent.ProjectileType<DalantiniumFanAlt>();
                Projectile projectile = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                powerUp = 0;
                if (Main.netMode != NetmodeID.Server)
                {
                    EEMod.Prims.CreateTrail(projectile);
                }
            }
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return powerUp > 2000;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 7);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }
    }
}