using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Banners;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Projectiles.CoralReefs;
using Terraria.ID;
using EEMod.Prim;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class TeslaBall : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.damage = 20;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 0;
            Projectile.hide = false;
        }

        //public Color lythenGold = new Color(231, 197, 60);
        public Color lythenGold = Color.Gold;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D ring = ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/TeslaRing").Value;
            Texture2D ball = ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/TeslaBall").Value;
            Texture2D mask = ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            //ApplyIntroShader(1f, new Vector2(256, 256), Vector2.Zero, new Vector2(1f, 1f), false, 0.75f);

            //Main.spriteBatch.Draw(ring, Projectile.Center - Main.screenPosition, null, Color.White * 0.75f, 0f, ring.Bounds.Size() / 2f, 1f, SpriteEffects.None, 0f);

            Helpers.DrawAdditive(ring, Projectile.Center - Main.screenPosition, lythenGold, 1f, 0f);

            Helpers.DrawAdditive(mask, Projectile.Center - Main.screenPosition, lythenGold, 2f, 0f);

            Helpers.DrawAdditive(ball, Projectile.Center - Main.screenPosition, lythenGold, 1f, 0f);

            if (Main.GameUpdateCount % 10 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int lightningproj = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<TeslaCoralProj>(), 20, 3f);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        PrimitiveSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj], 3f));
                    }

                    TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                    zappy.target = Projectile.Center + Vector2.UnitY.RotatedByRandom(6.28f) * 128f;
                }
            }

            return false;
        }

        public override void AI()
        {

        }

        public void ApplyIntroShader(float lerpVal, Vector2 scale, Vector2 offset, Vector2 timeMultiplier, bool invert = false, float alpha = 1f)
        {
            EEMod.HydrosEmerge.Parameters["newColor"].SetValue(new Vector4(lythenGold.R / 255f, lythenGold.G / 255f, lythenGold.B / 255f, 1f));

            EEMod.HydrosEmerge.Parameters["lerpVal"].SetValue(lerpVal);
            EEMod.HydrosEmerge.Parameters["thresh"].SetValue(lerpVal);

            EEMod.HydrosEmerge.Parameters["time"].SetValue(new Vector2((Main.GameUpdateCount / 300f) * timeMultiplier.X, (Main.GameUpdateCount / 300f) * timeMultiplier.Y));

            EEMod.HydrosEmerge.Parameters["invert"].SetValue(invert);

            EEMod.HydrosEmerge.Parameters["alpha"].SetValue(alpha);

            EEMod.HydrosEmerge.Parameters["offset"].SetValue(((Projectile.Center / 600f) / 2) * 2f);

            EEMod.HydrosEmerge.Parameters["frames"].SetValue(1);

            EEMod.HydrosEmerge.Parameters["noiseBounds"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelatedBloom").Value.Bounds.Size());
            EEMod.HydrosEmerge.Parameters["imgBounds"].SetValue(scale);

            EEMod.HydrosEmerge.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelatedBloom").Value);

            EEMod.HydrosEmerge.CurrentTechnique.Passes[0].Apply();
        }
    }
}