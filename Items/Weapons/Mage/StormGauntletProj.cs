using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using EEMod.Prim;
using EEMod.Extensions;
using Terraria.Audio;
using Terraria.DataStructures;
using EEMod.Projectiles.CoralReefs;
using Terraria.GameContent;
using EEMod.Items.Weapons.Melee;

namespace EEMod.Items.Weapons.Mage
{
    public class StormGauntletProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Gauntlet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.damage = 20;
            Projectile.knockBack = 3.5f;
            Projectile.ai[1] = 0;
        }

        NPC grabbed;
        bool flipped;

        bool clicking;
        bool oldClicking;

        int stunned;

        public override void AI()
        {
            if(Projectile.ai[1] == 0)
            {
                PrimitiveSystem.primitives.CreateTrail(new StormGauntletPrimTrail(Projectile, 18f, 1f));
                PrimitiveSystem.primitives.CreateTrail(new StormGauntletPrimTrail(Projectile, 18f, 1f));

                Projectile.ai[1] = 1;
            }
            if (Main.player[Projectile.owner].HeldItem.type != ModContent.ItemType<StormGauntlet>())
            {
                Projectile.Kill();
            }

            Vector2 desiredPos = Main.MouseWorld - Main.LocalPlayer.Center;

            Vector2 travelPos;

            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Main.MouseWorld) > 192 * 192)
                travelPos = Main.LocalPlayer.Center + (Vector2.Normalize(desiredPos) * 192f);
            else
                travelPos = Main.LocalPlayer.Center + (Vector2.Normalize(desiredPos) * Vector2.Distance(Main.LocalPlayer.Center, Main.MouseWorld));

            if(stunned <= 0) Projectile.velocity = (travelPos - Projectile.Center) + Main.LocalPlayer.velocity;

            if (Vector2.Distance(Projectile.Center, Main.LocalPlayer.Center) <= 128)
                if (Projectile.velocity.LengthSquared() > 16 * 16) Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16f * MathHelper.Clamp(-stunned / 3f, -1, 1);
            else
                if (Projectile.velocity.LengthSquared() > 64 * 64) Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 64f;


            Projectile.rotation = Vector2.Normalize(travelPos - Main.LocalPlayer.Center).ToRotation() + 1.57f;


            if (Projectile.Center.X - Main.LocalPlayer.Center.X > 0)
            {
                Main.LocalPlayer.direction = 1;
                flipped = true;
            }
            else
            {
                Main.LocalPlayer.direction = -1;
                flipped = false;
            }

            if (Math.Abs((Projectile.Center - Main.LocalPlayer.Center).ToRotation() - 3.14f) - 1.57f < -0.77f) Main.LocalPlayer.bodyFrame.Y = 3 * 56;
            else if (Math.Abs((Projectile.Center - Main.LocalPlayer.Center).ToRotation() - 3.14f) - 1.57f < 0.77f) Main.LocalPlayer.bodyFrame.Y = 4 * 56;
            else if (Math.Abs((Projectile.Center - Main.LocalPlayer.Center).ToRotation() - 3.14f) - 1.57f < 1.57f) Main.LocalPlayer.bodyFrame.Y = 3 * 56;
            else if (Math.Abs((Projectile.Center - Main.LocalPlayer.Center).ToRotation() - 3.14f) - 1.57f < 2.27f) Main.LocalPlayer.bodyFrame.Y = 2 * 56;
            else if (Math.Abs((Projectile.Center - Main.LocalPlayer.Center).ToRotation() - 3.14f) - 1.57f < 3.87f) Main.LocalPlayer.bodyFrame.Y = 1 * 56;
            else Main.LocalPlayer.bodyFrame.Y = 2 * 56;

            stunned--;


            if (clicking)
            {
                if (grabbed == null && collisionCooldown <= 0)
                {
                    int grabbedVal = Helpers.GetNearestNPC(Projectile.Center, false, false);

                    if (grabbedVal == -1) return;

                    grabbed = Main.npc[grabbedVal];

                    if (Projectile.velocity.Length() > 4 && Vector2.Distance(grabbed.Center, Projectile.Center) <= 32f)
                    {
                        grabbed.StrikeNPC((int)MathHelper.Clamp(Projectile.velocity.Length(), 6, 20), 3.5f, 0);
                        collisionCooldown = 8;
                        Projectile.velocity += -Projectile.velocity * 2f;
                        stunned = 3;
                    }

                    if (Vector2.Distance(grabbed.Center, Projectile.Center) > 32f || grabbed.boss == true || grabbed.knockBackResist <= 0f || grabbed.immortal == true || oldClicking) grabbed = null;
                }
            }
            else
            {
                if(grabbed != null)
                {
                    grabbed.velocity = Projectile.velocity + Main.LocalPlayer.velocity;

                    if (grabbed.velocity.Length() > 16f) grabbed.velocity = Vector2.Normalize(grabbed.velocity) * 16f;

                    grabbed = null;
                }
            }

            if(grabbed != null)
            {
                grabbed.Center = Projectile.Center;
            }

            Projectile.ai[0]--;
            if (Projectile.ai[0] <= 1) Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();

            collisionCooldown--;

            oldClicking = clicking;
            clicking = Main.LocalPlayer.controlUseItem;

            if (!Main.LocalPlayer.IsAlive()) Kill(0);

            //if (Main.LocalPlayer.altFunctionUse == 2) Kill(0);
        }

        int collisionCooldown;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Length() > 4 && grabbed != null && collisionCooldown <= 0)
            {
                grabbed.StrikeNPC((int)MathHelper.Clamp(Projectile.velocity.Length(), 6, 20), 0f, 0);

                grabbed = null;

                Projectile.ai[0] = 7;

                Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 4f, true, false, 8);

                DoTheThing(Projectile.Center);
            }

            collisionCooldown = 8;

            return false;
        }

        private void DoTheThing(Vector2 pos)
        {
            for (double i = 0; i < 6.28; i += Main.rand.NextFloat(1f, 2f))
            {
                int lightningproj = Projectile.NewProjectile(new EntitySource_Parent(Projectile), pos, new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 3f, ModContent.ProjectileType<AxeLightning>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (Main.netMode != NetmodeID.Server)
                {
                    PrimitiveSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj], 4, 0.75f));
                }
            }

            SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
        }

        public int frame;

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.LocalPlayer.controlUseItem) frame = 1;
            else frame = 0;

            Texture2D mask = ModContent.Request<Texture2D>("EEMod/Textures/Extra_49").Value;

            if(grabbed != null)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                ApplyIntroShader(1f, new Vector2(grabbed.width, grabbed.height), Vector2.Zero, Vector2.One, true, 1f);

                Texture2D tex = TextureAssets.Npc[grabbed.type].Value;

                Main.spriteBatch.Draw(tex, grabbed.Center - Main.screenPosition, grabbed.frame, Color.White, grabbed.rotation, grabbed.frame.Size() / 2f, 1f, grabbed.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Texture2D gauntlet = ModContent.Request<Texture2D>("EEMod/Items/Weapons/Mage/StormGauntletProj").Value;
            Texture2D glow = ModContent.Request<Texture2D>("EEMod/Items/Weapons/Mage/StormGauntletProjGlow").Value;

            Helpers.DrawAdditive(mask, Projectile.Center.ForDraw(), Color.Gold * 0.5f, 0.5f, Projectile.rotation);

            if (Main.LocalPlayer.controlUseItem)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                ApplyIntroShader(1f, new Vector2(34, 34), Vector2.Zero, Vector2.One, true, 1f);

                Texture2D gauntletalt = ModContent.Request<Texture2D>("EEMod/Items/Weapons/Mage/StormGauntletProjTrim").Value;

                for (int k = 0; k < 4; k++)
                {
                    Vector2 initRot = Vector2.UnitY * 2f;

                    Main.spriteBatch.Draw(gauntletalt, Projectile.Center + initRot.RotatedBy((k * 1.57f) + Projectile.rotation) - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(17, 17), 1f, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Main.spriteBatch.Draw(gauntlet, Projectile.Center.ForDraw(), new Rectangle(0, frame * 34, 34, 34), lightColor, Projectile.rotation, new Vector2(17, 17), 1f, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center.ForDraw(), new Rectangle(0, frame * 34, 34, 34), Color.White, Projectile.rotation, new Vector2(17, 17), 1f, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }

        public void ApplyIntroShader(float lerpVal, Vector2 scale, Vector2 offset, Vector2 timeMultiplier, bool invert = false, float alpha = 1f)
        {
            EEMod.HydrosEmerge.Parameters["newColor"].SetValue(new Vector4(Color.Gold.R / 255f, Color.Gold.G / 255f, Color.Gold.B / 255f, 1f));

            EEMod.HydrosEmerge.Parameters["lerpVal"].SetValue(lerpVal);
            EEMod.HydrosEmerge.Parameters["thresh"].SetValue(lerpVal);

            EEMod.HydrosEmerge.Parameters["time"].SetValue(new Vector2((((int)(Main.GameUpdateCount / 2) * 2f) / 480f) * timeMultiplier.X, (((int)(Main.GameUpdateCount / 2) * 2f) / 480f) * timeMultiplier.Y));

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