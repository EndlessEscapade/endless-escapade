/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;
using System;

namespace EEMod.Items.Weapons.Summon
{
    public class Tentalash : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentalash");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 13;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<TentalashBase>();
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.LocalPlayer.HasBuff(ModContent.BuffType<BabyHydrosBuff>());
        }
    }
    public class TentalashBase : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentalash");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 8;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 125;
            projectile.tileCollide = true;
        }
        private bool firstFrame;
        public override void AI()
        {
            if (firstFrame)
            {
                for(int i = 0; i < 18; i++)
                {
                    Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<TentalashMid>(), projectile.damage, projectile.knockBack);
                }
            }
            projectile.rotation = projectile.rotation.RotatedBy(MathHelper.ToRadians());
        }
    }
}*/