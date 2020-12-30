using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class ThermalMagma : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thermal Magma");
        }

        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale = 1f;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.ai[0] <= 0)
            {
                projectile.scale = Main.rand.NextFloat(0.6f, 1.1f);
            }
            projectile.ai[0]++;

            if(projectile.velocity.Y < 8) projectile.velocity.Y += 0.05f;

            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Lighting.AddLight(projectile.Center, 4f, 2f, 0f);

            Texture2D tex = mod.GetTexture("Projectiles/CoralReefs/ThermalMagma");

            Main.spriteBatch.Draw(tex, new Rectangle((int)projectile.Center.X - (int)Main.screenPosition.X, (int)projectile.Center.Y - (int)Main.screenPosition.Y, projectile.width, projectile.height), tex.Bounds, Color.White, projectile.rotation, new Vector2(19, 19), SpriteEffects.None, 0f);

            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.3f));
            EEMod.Particles.Get("Main").SpawnParticles(projectile.Center + new Vector2(Main.rand.Next(-17, 17), Main.rand.Next(-17, 17)), new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)), Color.Lerp(Color.OrangeRed, Color.LightGoldenrodYellow, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.97f), new SetMask(EEMod.instance.GetTexture("Masks/RadialGradient")), new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)), new RotateTexture(0.02f), new AfterImageTrail(0.75f), new SetLighting(new Vector3(4f, 2f, 0f), 0.3f));
        }
    }
}