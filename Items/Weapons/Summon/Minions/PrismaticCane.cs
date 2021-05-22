using EEMod.Buffs.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class PrismaticCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Cane");
        }

        public override void SetDefaults()
        {
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 13;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 38;
            item.height = 36;
            item.rare = ItemRarityID.Pink;
            item.knockBack = 0f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<PrismaticCaneProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 0 && Main.LocalPlayer.ownedProjectileCounts[item.shoot] < Main.LocalPlayer.maxMinions + 3)
            {
                item.useTime = 26;
                item.useAnimation = 26;
                item.useStyle = ItemUseStyleID.HoldingOut;

                if (Main.LocalPlayer.ownedProjectileCounts[item.shoot] == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<PrismaticCaneProj>(), 10, 0f, Main.myPlayer);
                        proj.ai[1] = i;
                    }
                }
                else
                {
                    Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<PrismaticCaneProj>(), 10, 0f, Main.myPlayer);
                    proj.ai[1] = Main.LocalPlayer.ownedProjectileCounts[item.shoot];
                }
            }

            /*if (player.altFunctionUse == 2)
            {
                item.useTime = 5;
                item.useAnimation = 5;
                item.useStyle = ItemUseStyleID.HoldingOut;

                foreach (Projectile proj in Main.projectile)
                {
                    if(Main.player[proj.owner] == player && proj.type == ModContent.ProjectileType<PrismaticCaneProj>())
                    {
                        (proj.modProjectile as PrismaticCaneProj).awake = true;
                    }
                }
            }*/
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}