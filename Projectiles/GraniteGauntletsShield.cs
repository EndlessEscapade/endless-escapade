using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Buffs.Buffs;

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
            //Get 30 locations in a circle around 'origin'
            int numLocations = 30;
            for (int i = 0; i < 30; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                Dust.NewDustPerfect(position, 13);
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
