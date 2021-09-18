using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using EEMod.Prim;
using EEMod.Extensions;
using Terraria.Audio;

namespace EEMod.Items.Weapons.Melee
{
    public class LythenWarhammerProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidebreaker");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

            Projectile.DamageType = DamageClass.Melee;
            // Projectile.tileCollide = false;
            Projectile.friendly = true;
            // Projectile.hostile = false;
            Projectile.damage = 20;
            Projectile.knockBack = 3.5f;
        }

        double radians = 0;
        int flickerTime = 0;
        float alphaCounter = 0;
        int chargeTime = 90;
        //ai[0] = charge
        //ai[1] = Whether or not thrown
        int height = 60;
        int width = 54;
        public override void AI()
        {
            alphaCounter += 0.08f;
            Player player = Main.player[Projectile.owner];
            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.4f));
            if (Projectile.ai[1] == 0)
            {
                Projectile.scale = MathHelper.Clamp(Projectile.ai[0] / 10, 0, 1);
                if (player.direction == 1)
                {
                    radians += (double)((Projectile.ai[0] + 10) / 200);
                }
                else
                {
                    radians -= (double)((Projectile.ai[0] + 10) / 200);
                }
                if (radians > 6.28)
                {
                    radians -= 6.28;
                }
                if (radians < -6.28)
                {
                    radians += 6.28;
                }
                player.itemAnimation -= (int)((Projectile.ai[0] + 50) / 6);

                while (player.itemAnimation < 3)
                {
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    player.itemAnimation += 320;
                }
                player.itemTime = player.itemAnimation;
                Projectile.velocity = Vector2.Zero;

                if (Projectile.ai[0] < chargeTime)
                {
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] == chargeTime)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
                    }
                }
                else
                {
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.1f));
                    EEMod.MainParticles.SpawnParticles(Projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * 2, 2, Color.Gold, new SlowDown(0.99f), new ZigzagMotion(10, 1.5f), new AfterImageTrail(0.99f));
                }
                Vector2 direction = Main.MouseWorld - player.position;
                direction.Normalize();
                double throwingAngle = direction.ToRotation() + 3.14;
                if (player.direction != 1)
                {
                    throwingAngle -= 6.28;
                }
                if (!player.channel && Math.Abs(radians - throwingAngle) < 1)
                {
                    Projectile.ai[1] = 1;
                    player.itemTime = 2;
                    player.itemAnimation = 2;

                    Projectile.tileCollide = true;
                    Projectile.timeLeft = 500;
                    Projectile.position = player.position;

                    direction *= 30;
                    Projectile.velocity = direction;
                }
                Projectile.position.Y = player.Center.Y - (int)(Math.Sin(radians * 0.96) * 40) - (Projectile.height / 2);
                Projectile.position.X = player.Center.X - (int)(Math.Cos(radians * 0.96) * 40) - (Projectile.width / 2);
            }



            else if (Projectile.ai[1] == 1)
            {
                if (Projectile.ai[0] < chargeTime)
                {
                    // Projectile.active = false;
                }
                else
                {
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.4f));
                    EEMod.MainParticles.SpawnParticles(Projectile.Center + (Projectile.velocity * 5), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * 2, 2, Color.Gold, new SlowDown(0.99f), new ZigzagMotion(10, 1.5f), new AfterImageTrail(0.99f));
                    Projectile.rotation = Projectile.velocity.ToRotation() + 0.78f;
                }
                if (Projectile.timeLeft == 450)
                {
                    Projectile.ai[1] = 2;
                }
            }
            else
            {
                // Projectile.tileCollide = false;
                Projectile.rotation -= 0.5f;
                Vector2 direction = player.position - Projectile.position;
                if (direction.Length() < 20 || player.statLife < 1)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                    // Projectile.active = false;
                }
                direction.Normalize();
                direction *= 20;
                Projectile.velocity = direction;
                if (Projectile.timeLeft == 170)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Projectile.ai[1] == 0)
            {
                Color color = lightColor;
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 0);
                if (Projectile.ai[0] >= chargeTime && Projectile.ai[1] == 0)
                {
                    Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height * 2, width, height), Color.White * 0.9f, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);

                    if (flickerTime < 16)
                    {
                        flickerTime++;
                        color = Color.White;
                        float flickerTime2 = (float)(flickerTime / 20f);
                        float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
                        if (alpha < 0)
                        {
                            alpha = 0;
                        }
                        Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), color * alpha, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);
                    }
                }
                return false;
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Projectile.ai[0] >= chargeTime)
            {
                float sineAdd = (float)Math.Sin(alphaCounter) + 2.5f;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D tex = ModContent.Request<Texture2D>("EEMod/Textures/SmoothFadeOut").Value;

                Main.spriteBatch.Draw(tex, Projectile.Center.ForDraw(), tex.Bounds, Color.Gold * sineAdd * 0.3f, 0, new Vector2(31, 23), 0.25f * (sineAdd + 1) * 2, SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }
            if (Projectile.ai[1] != 0)
            {
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, new Rectangle((int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), 54, 60), new Rectangle(0, height * 2, width, height), Color.White, Projectile.rotation, new Vector2(27, 30), SpriteEffects.None, 0);
            }
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] == 1 && Projectile.ai[0] >= chargeTime)
            {
                DoTheThing(Projectile.position);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] >= chargeTime)
                DoTheThing(Projectile.position + (Projectile.velocity * 2));
            return false;
        }

        private void DoTheThing(Vector2 pos)
        {
            Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 8f, true, false, 8);
            Projectile.timeLeft = 200;
            if (Projectile.ai[1] == 1)
            {
                for (double i = 0; i < 6.28; i += Main.rand.NextFloat(1f, 2f))
                {
                    int lightningproj = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), pos, new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 2.5f, ModContent.ProjectileType<AxeLightning>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    if (Main.netMode != NetmodeID.Server)
                    {
                        EEMod.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj]));
                    }
                }
                Projectile.ai[1] = 2;
            }
            SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
        }
    }
}