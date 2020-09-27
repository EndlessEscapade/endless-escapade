using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class DesertRuneInsignia : ModProjectile
    {
        public override string Texture => EEMod.instance.GetTexture("Empty").ToString();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Rune Insignia");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 52;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 100000;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.aiStyle = -1;
            projectile.arrow = true;
            projectile.damage = 0;
        }

        public override void AI()
        {
            projectile.ai[0]++;
            if (projectile.ai[0] <= 60) //Desert rune insignia
            {
                Vector2 origin = Main.LocalPlayer.Center + new Vector2(0, -160);
                float radius = 48;
                int numLocations = 48;
                for (int i = 0; i < numLocations; i++)
                {
                    Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i * 2)) * radius;
                    Dust dust = Dust.NewDustPerfect(position, 13);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                    radius--;
                }
            }
            if (projectile.ai[0] >= 90 && projectile.ai[0] <= 120) //Bubble rune insignia
            {
                Vector2 origin = Main.LocalPlayer.Center + new Vector2(0, -160);
                float radius = 8;
                int numLocations = 8;
                for (int i = 0; i < numLocations; i++)
                {
                    Vector2 position = origin + new Vector2(0, -40) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                    Dust dust = Dust.NewDustPerfect(position, 13);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }

                for (int i = 0; i < 15; i++)
                {
                    Vector2 position = origin + new Vector2(0, i * 4);
                    Dust dust = Dust.NewDustPerfect(position, 13);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }

                for (int i = 0; i < 15; i++)
                {
                    Vector2 position = origin + new Vector2(i * 2, -i * 2);
                    Dust dust = Dust.NewDustPerfect(position, 13);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }

                for (int i = 0; i < 15; i++)
                {
                    Vector2 position = origin + new Vector2(-i * 2, -i * 2);
                    Dust dust = Dust.NewDustPerfect(position, 13);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }
            }
        }
    }
}