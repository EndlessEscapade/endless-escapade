using EEMod.Buffs.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class PrismaticCane : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Cane");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.noMelee = true;
            // Item.autoReuse = false;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 13;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.width = 38;
            Item.height = 36;
            Item.rare = ItemRarityID.Pink;
            Item.knockBack = 0f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<PrismaticCaneProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 0 && Main.LocalPlayer.ownedProjectileCounts[Item.shoot] < Main.LocalPlayer.maxMinions + 3)
            {
                Item.useTime = 26;
                Item.useAnimation = 26;
                Item.useStyle = ItemUseStyleID.Shoot;

                if (Main.LocalPlayer.ownedProjectileCounts[Item.shoot] == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_Item(player, Item), player.Center, Vector2.Zero, ModContent.ProjectileType<PrismaticCaneProj>(), 10, 0f, Main.myPlayer);
                        proj.ai[1] = i;
                    }
                }
                else
                {
                    Projectile proj = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_Item(player, Item), player.Center, Vector2.Zero, ModContent.ProjectileType<PrismaticCaneProj>(), 10, 0f, Main.myPlayer);
                    proj.ai[1] = Main.LocalPlayer.ownedProjectileCounts[Item.shoot];
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
                        (proj.ModProjectile as PrismaticCaneProj).awake = true;
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