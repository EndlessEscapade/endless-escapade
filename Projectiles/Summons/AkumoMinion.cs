using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Projectiles.Summons
{
    public class AkumoMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minikumo");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        private int delay;
        public override void SetDefaults()
        {
            //projectile.netImportant = true;
            projectile.CloneDefaults(ProjectileID.Raven);
            projectile.width = 56;
            projectile.height = 56;
            //projectile.penetrate = -1;
            //projectile.timeLeft = 18000;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
        }

        public override void AI()
        {
            bool areYouHere = projectile.type == ModContent.ProjectileType<AkumoMinion>();
            Player player = Main.player[projectile.owner];
            player.AddBuff(ModContent.BuffType<AkumoBuff>(), 3600);
        }
    }
}
