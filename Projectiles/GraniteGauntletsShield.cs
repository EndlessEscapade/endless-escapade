using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class GraniteGauntletsShield : ModProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Shield");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.light = 0.3f;    // projectile light
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.timeLeft = 120;
        }

        private bool firstFrame = true;

        public override void AI()
        {
            Player ownerplayer = Main.player[projectile.owner];
            if (firstFrame)
            {
                ownerplayer.AddBuff(ModContent.BuffType<RechargingGauntlets>(), 15 * 60);
                firstFrame = false;
                ownerplayer.noKnockback = true;
            }
            ownerplayer.statDefense += 12;

            Vector2 origin = Main.LocalPlayer.Center;
            float radius = 48;
            int numLocations = 30;
            for (int i = 0; i < 30; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                Dust dust = Dust.NewDustPerfect(position, 13);
                dust.velocity = Vector2.Zero;
            }

            ownerplayer.position = ownerplayer.oldPosition;
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].statDefense -= 12;
            Main.player[projectile.owner].noKnockback = false;
        }
    }
}