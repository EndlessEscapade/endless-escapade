using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using System;
//TODO:
//-Shimmer on blade itself
//-Proper right click motion
//-Proper hovering on daggers
//-Dagger dust
//-Sound effects
namespace EEMod.Items.Weapons.Melee.Swords
{
    public class PrismaticBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Blade");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4f; // 5 and 1/4
            item.useTime = 35;
            item.useAnimation = 35;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.damage = 50;
            item.width = 54;
            item.height = 54;
            item.UseSound = SoundID.Item1;
        }
        int swordsActive = 0;
        int[] swordArray = new int[9];
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = ItemUseStyleID.Stabbing;
                item.useAnimation = 15;
                item.noMelee = true;
                if (swordArray[0] != default)
                {
                    for (int i = 0; i < swordsActive; i++)
                    {
                        if (Main.projectile[swordArray[i]].active)
                        {
                            Main.projectile[swordArray[i]].friendly = true;
                            if (Main.netMode != NetmodeID.Server)
                            {
                                EEMod.prims.CreateTrail(Main.projectile[swordArray[i]]);
                            }
                        }
                        swordArray[i] = 0;
                    }
                    swordsActive = 0;
                }
            }
            else
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.useAnimation = 35;
                item.noMelee = false;
            }
            return base.UseItem(player);
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            int damage2 = damage;
            if (damage2 > target.lifeMax)
            {
                damage2 = target.lifeMax;
            }
            if (swordsActive < 9 && Main.rand.NextBool())
            {
                float angle = (float)(swordsActive * 0.7f);
                swordArray[swordsActive] = Projectile.NewProjectile(target.position, Vector2.Zero, ModContent.ProjectileType<PrismDagger>(), damage2, 0, player.whoAmI, angle);
                swordsActive++;
            }
        }
    }
}