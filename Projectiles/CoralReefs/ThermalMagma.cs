using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class ThermalMagma : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thermal Magma");
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            // Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] <= 0)
            {
                Projectile.scale = Main.rand.NextFloat(0.6f, 1.1f);
            }
            Projectile.ai[0]++;

            if(Projectile.velocity.Y < 8) Projectile.velocity.Y += 0.05f;

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void PostDraw(Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, 4f, 2f, 0f);

            Texture2D tex = Mod.Assets.Request<Texture2D>("Projectiles/CoralReefs/ThermalMagma").Value;

            Main.spriteBatch.Draw(tex, new Rectangle((int)Projectile.Center.X - (int)Main.screenPosition.X, (int)Projectile.Center.Y - (int)Main.screenPosition.Y, Projectile.width, Projectile.height), tex.Bounds, Color.White, Projectile.rotation, new Vector2(19, 19), SpriteEffects.None, 0f);

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.2f));
            EEMod.MainParticles.SpawnParticles(Projectile.Center + new Vector2(Main.rand.Next(-17, 17), Main.rand.Next(-17, 17)), new Vector2(Main.rand.NextFloat(-1.25f, 1.25f), Main.rand.NextFloat(-1.25f, 1.25f)), Mod.Assets.Request<Texture2D>("Particles/Cross").Value, 30, 1, Color.Lerp(Color.OrangeRed, Color.LightGoldenrodYellow, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.97f), new SetMask(EEMod.Instance.Assets.Request<Texture2D>("Textures/RadialGradient").Value), new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)), new RotateTexture(0.02f), new AfterImageTrail(0.75f), new SetLighting(new Vector3(4f, 2f, 0f), 0.3f));
        }
    }
}