using EEMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Boomerangs
{
    public class FeatheredChakram : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            Item.damage = 116;
            Item.melee = true;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 60;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FeatheredChakramProjectileAlt>();
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.ownedProjectileCounts[Item.shoot] < 4;
            //int no = 0;
            //for (int i = 0; i < Main.projectile.Length; i++)
            //{
            //    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<FCHandler>())
            //    {
            //        no++;
            //    }
            //}
            //if(no == 0)
            //Projectile.NewProjectile(Main.LocalPlayer.Center, Vector2.Zero, ModContent.ProjectileType<FCHandler>(), item.damage, item.knockBack, Main.myPlayer, MathHelper.Pi);
            //return true;
        }
    }
}