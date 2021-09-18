using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Enemy
{
    public class MechanicalMissile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Missile");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 8;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            // Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.spriteDirection = -1;
        }

        public override void AI()
        {
            Vector2 closestPlayer = new Vector2();
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Vector2.DistanceSquared(Main.player[i].Center, Projectile.Center) <= Vector2.DistanceSquared(closestPlayer, Projectile.Center))
                {
                    closestPlayer = Main.player[i].Center;
                }
            }

            Projectile.velocity = Vector2.Normalize(closestPlayer - Projectile.Center) * 10;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.position, 0, 0, DustID.Lava);
                Main.dust[dust].velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Projectiles/Enemy/MechanicalMissileGlow").Value, Projectile.Center - Main.screenPosition, Projectile.getRect(), Color.White, Projectile.rotation, Projectile.getRect().Size() / 2, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
    }
}