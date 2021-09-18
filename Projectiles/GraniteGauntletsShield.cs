using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class GraniteGauntletsShield : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Shield");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            // Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.light = 0.3f;    // projectile light
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 120;
        }

        private bool firstFrame = true;

        public override void AI()
        {
            Player ownerplayer = Main.player[Projectile.owner];
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
            Main.player[Projectile.owner].statDefense -= 12;
            // Main.player[Projectile.owner].noKnockback = false;
        }
    }
}