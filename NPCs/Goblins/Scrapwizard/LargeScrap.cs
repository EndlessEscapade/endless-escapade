using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class LargeScrap : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrap");
        }

        public override void SetDefaults()
        {
            Projectile.width = 158;
            Projectile.height = 16;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = true;

            Projectile.damage = 20;

            Projectile.timeLeft = 1000000000;

            Projectile.hide = true;
        }

        public ref float AttackPhase => ref Projectile.ai[0];

        public float initRotation;
        public float desiredRotation;
        public Vector2 desiredPosition;

        public float lastRotation;
        public Vector2 lastPosition;

        public Vector2 controlPoint;

        public float movementDuration = 30 + 30 + 65;
        public float movementTimer;
        public float delayDuration = 25;

        public Vector2 offset;

        public Vector2 fakeVelocity;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void AI()
        {
            switch(AttackPhase)
            {
                case -2:
                    Projectile.Center = desiredPosition;
                    Projectile.rotation = desiredRotation;

                    break;

                case -1:
                    if (desiredPosition != Vector2.Zero)
                    {
                        Projectile.Center = desiredPosition + offset.RotatedBy(desiredRotation);
                        Projectile.rotation = desiredRotation + initRotation;
                    }

                    break;

                case 0: //Embedded in the ground
                    lastPosition = Projectile.Center;
                    lastRotation = Projectile.rotation;

                    Projectile.velocity = Vector2.Zero;

                    movementTimer = 0;

                    break;

                case 1: //Moving towards a given position and rotation over a given duration
                    if (movementTimer < 30)
                    {
                        Projectile.Center -= Vector2.UnitX.RotatedBy(Projectile.rotation) * MathHelper.Clamp((30 - movementTimer) / 5f, 0, 15);
                    }
                    else if (movementTimer == 30)
                    {
                        lastPosition = Projectile.Center;
                        lastRotation = Projectile.rotation;
                    }
                    else if (movementTimer < 60)
                    {
                        Projectile.rotation = MathHelper.SmoothStep(lastRotation, (Main.LocalPlayer.Center - Projectile.Center).ToRotation(), (movementTimer - 30) / 30f);
                    }
                    else if (movementTimer < movementDuration)
                    {
                        Vector2 pos1 = Vector2.SmoothStep(lastPosition, controlPoint, MathHelper.Clamp((movementTimer - 60) / (movementDuration - 60), 0f, 1f));
                        Vector2 pos2 = Vector2.SmoothStep(controlPoint, desiredPosition, MathHelper.Clamp((movementTimer - 60) / (movementDuration - 60), 0f, 1f));

                        Projectile.Center = Vector2.SmoothStep(pos1, pos2, MathHelper.Clamp((movementTimer - 60) / (movementDuration - 60), 0f, 1f));

                        while (Math.Abs(Projectile.rotation - (Main.LocalPlayer.Center - Projectile.Center).ToRotation()) > MathHelper.Pi)
                        {
                            if (Projectile.rotation < 0) Projectile.rotation += MathHelper.TwoPi;
                            else Projectile.rotation -= MathHelper.TwoPi;
                        }

                        //Projectile.rotation += MathHelper.Clamp(((Main.LocalPlayer.Center - Projectile.Center).ToRotation() - Projectile.rotation) / 10f, -0.15f, 0.15f);

                        Projectile.rotation += MathHelper.Clamp(((Main.LocalPlayer.Center - Projectile.Center).ToRotation() - Projectile.rotation) / 5f, -0.45f, 0.45f);
                    }
                    else if (movementTimer >= movementDuration + delayDuration)
                    {
                        AttackPhase = 2;
                    }
                    else
                    {
                        Projectile.rotation += MathHelper.Clamp((desiredRotation - Projectile.rotation) / 5f, -0.45f, 0.45f);
                    }

                    movementTimer++;

                    if (movementTimer < 30 || movementTimer >= 90) fakeVelocity = Vector2.SmoothStep(lastPosition, desiredPosition, MathHelper.Clamp((movementTimer - 60) / (movementDuration - 60), 0f, 1f)) - Projectile.Center;
                    else fakeVelocity = new Vector2(0, 3f);

                    break;

                case 2: //Firing
                    PrimitiveSystem.primitives.ClearTrailsOn(Projectile);

                    Projectile.velocity = Vector2.UnitX.RotatedBy(Projectile.rotation) * 80f;

                    break;

                case 3: //Firing alt

                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if(AttackPhase == 1)
            {
               // Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapBloom").Value, Projectile.Center + new Vector2(0, -2) - Main.screenPosition, Color.Violet, 1f, Projectile.rotation);
                //Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapBloom").Value, Projectile.Center + new Vector2(0, 2) - Main.screenPosition, Color.Violet, 1f, Projectile.rotation);
                //Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapBloom").Value, Projectile.Center + new Vector2(-2, 0) - Main.screenPosition, Color.Violet, 1f, Projectile.rotation);
                //Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapBloom").Value, Projectile.Center + new Vector2(2, 0) - Main.screenPosition, Color.Violet, 1f, Projectile.rotation);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapBloom").Value, Projectile.Center - Main.screenPosition, Color.Violet * MathHelper.Clamp(fakeVelocity.Length() / 3f, 0f, 1f), 1f, Projectile.rotation + (movementTimer < 30 ? ((float)Math.Sin(movementTimer / 3f) * (((30 - movementTimer) / 50f))) : 0));

                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value, Projectile.Center + new Vector2(0, -2) - Main.screenPosition, null, Color.Violet * MathHelper.Clamp(fakeVelocity.Length() / 3f, 0f, 1f), Projectile.rotation + (movementTimer < 30 ? ((float)Math.Sin(movementTimer / 3f) * (((30 - movementTimer) / 50f))) : 0), ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value.TextureCenter(), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value, Projectile.Center + new Vector2(0, 2) - Main.screenPosition, null, Color.Violet * MathHelper.Clamp(fakeVelocity.Length() / 3f, 0f, 1f), Projectile.rotation + (movementTimer < 30 ? ((float)Math.Sin(movementTimer / 3f) * (((30 - movementTimer) / 50f))) : 0), ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value.TextureCenter(), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value, Projectile.Center + new Vector2(-2, 0) - Main.screenPosition, null, Color.Violet * MathHelper.Clamp(fakeVelocity.Length() / 3f, 0f, 1f), Projectile.rotation + (movementTimer < 30 ? ((float)Math.Sin(movementTimer / 3f) * (((30 - movementTimer) / 50f))) : 0), ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value.TextureCenter(), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value, Projectile.Center + new Vector2(2, 0) - Main.screenPosition, null, Color.Violet * MathHelper.Clamp(fakeVelocity.Length() / 3f, 0f, 1f), Projectile.rotation + (movementTimer < 30 ? ((float)Math.Sin(movementTimer / 3f) * (((30 - movementTimer) / 50f))) : 0), ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapOutline").Value.TextureCenter(), 1f, SpriteEffects.None, 0f);

                if(movementTimer >= movementDuration)
                {
                    Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                    Texture2D telegraphTex = ModContent.Request<Texture2D>("EEMod/Textures/TelegraphLineDouble").Value;

                    Main.spriteBatch.Draw(telegraphTex, desiredPosition + new Vector2(40, 0).RotatedBy(Projectile.rotation) - Main.screenPosition, null, Color.Pink * 0.75f * (1 - (((movementTimer - movementDuration) - 5) / (float)delayDuration)), Projectile.rotation + MathHelper.PiOver2, new Vector2(37 / 2f, 1200), 0.5f, SpriteEffects.None, 0f);

                    Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);
                }

                if (((int)(((movementTimer - movementDuration) / (float)(delayDuration)) * 5f)) >= 0) return false;
            }

            if (movementTimer < 30 && AttackPhase == 1)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrap").Value, Projectile.Center - Main.screenPosition, null, Color.Violet * MathHelper.Clamp(fakeVelocity.Length() / 3f, 0f, 1f), Projectile.rotation + ((float)Math.Sin(movementTimer / 3f) * (((30 - movementTimer) / 50f))), new Vector2(158 / 2, 8), 1f, SpriteEffects.None, 0f);

                return false;
            }
            else
            {
                lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
                return true;
            }
        }

        private int attackPhase2AnimTimer;

        public override void PostDraw(Color lightColor)
        {
            if (AttackPhase == 1)
            {
                if (((int)(((movementTimer - movementDuration) / (float)(delayDuration)) * 4f)) >= 0) 
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapAnim").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 96 * ((int)(((movementTimer - movementDuration) / (float)(delayDuration)) * 4f)) + 96, 340, 96), Color.White, Projectile.rotation, new Vector2(122, 32) + new Vector2(158 / 2, 16 / 2), 1f, SpriteEffects.None, 0f);
                
                attackPhase2AnimTimer = (int)(delayDuration / 4f);
            }
            if (AttackPhase == 2)
            {
                if (attackPhase2AnimTimer > 0)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/LargeScrapAnim").Value, desiredPosition - Main.screenPosition, new Rectangle(0, 96 * 6, 340, 96), Color.White, desiredRotation, new Vector2(122, 32) + new Vector2(158 / 2, 16 / 2), 1f, SpriteEffects.None, 0f);
                    attackPhase2AnimTimer--;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(AttackPhase == 2 || AttackPhase == 3)
            {
                bool collisionCheck = true;

                while (collisionCheck)
                {
                    if(Main.tile[(int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)].HasTile && Main.tileSolid[(int)Main.tile[(int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)].BlockType])
                    {
                        Projectile.Center -= Vector2.Normalize(Projectile.oldVelocity);
                    }
                    else
                    {
                        AttackPhase = 0;
                        collisionCheck = false;
                    }
                }
            }

            return false;
        }
    }
}