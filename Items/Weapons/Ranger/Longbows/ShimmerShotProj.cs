
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Items.Weapons.Ranger;
using EEMod.Extensions;
using Terraria.ID;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class ShimmerShotProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shimmer Shot");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 72;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.alpha = 0;

            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
        }

        private float speedOfArrow = 400f;
        private float minGrav = 0f;
        private int newProj = ModContent.ProjectileType<ShimmerShotProj1>();
        private bool showDots = false;
        private int projCount = 4;
        private float projSpread = 0.3f;

        private List<int> exclude = new List<int> { };
        private float Max = 100;
        private bool vanillaFlag;

        private float whiteFlash;

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 20 && projectile.frame < 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;

                if(projectile.frame >= 3)
                {
                    whiteFlash = 1f;
                    Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
                }
            }

            if (whiteFlash > 0) { whiteFlash -= 0.2f; }
            if(whiteFlash <= 0.25f && whiteFlash >= 0.1f) { projectile.ai[1] = 1; }

            if (projectile.ai[1] == 1)
            {
                whiteFlash = 0.25f + ((float)Math.Sin(Main.GameUpdateCount / 5f) / 8f);
            }


            Player projOwner = Main.player[projectile.owner];
            float progression = projOwner.itemAnimation / (float)projOwner.itemAnimationMax;

            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projectile.rotation = (Main.MouseWorld - projOwner.Center).ToRotation();
            projectile.position.X = projOwner.Center.X - projectile.width / 2;
            projectile.position.Y = projOwner.Center.Y - projectile.height / 2;
            float speed = speedOfArrow;
            projOwner.bodyFrame.Y = 56 * (6 + (int)(gravAccel - minGrav));
            if (!projOwner.controlUseItem && projectile.ai[0] == 0)
            {
                projectile.timeLeft = 16;
                projectile.ai[0] = 1;

                if (projectile.frame >= 3)
                {
                    Vector2 comedy = Vector2.Normalize(Main.MouseWorld - projOwner.Center);
                    for (float i = 0; i < projCount; i++)
                    {
                        Projectile projectile2 = Projectile.NewProjectileDirect(projOwner.Center, comedy.RotatedBy(-((projCount / 2) * projSpread) + (i * projSpread)) / Max * speed, newProj, 10, 10f, Main.myPlayer);

                        EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Cyan, Color.Magenta, i / projCount), 40));
                    }
                }
                else
                {
                    Vector2 comedy = Vector2.Normalize(Main.MouseWorld - projOwner.Center);
                    Projectile projectile2 = Projectile.NewProjectileDirect(projOwner.Center, comedy / Max * speed, newProj, 10, 10f, Main.myPlayer);

                    EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Cyan, Color.Magenta, Main.rand.NextFloat(0, 1)), 40));
                }
            }

            if(projOwner.HeldItem.type != ModContent.ItemType<ShimmerShot>())
            {
                projectile.Kill();
            }
            
            if(projectile.ai[0] == 1)
            {
                projectile.alpha += 16;
            }

            if(Main.MouseWorld.X <= projOwner.Center.X)
            {
                projOwner.direction = -1;
            }
            else
            {
                projOwner.direction = 1;
            }
        }

        private float gravAccel = 4;

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(whiteFlash > 0)
            {
                Rectangle rect = new Rectangle(0, 0, 46, 72);
                Main.spriteBatch.Draw(mod.GetTexture("Items/Weapons/Ranger/Longbows/ShimmerShotProjGlow"), new Rectangle((int)projectile.Center.ForDraw().X, (int)projectile.Center.ForDraw().Y, 46, 72), rect, Color.White * whiteFlash * ((255 - projectile.alpha) / 255f), projectile.rotation, rect.Size() / 2f, SpriteEffects.None, 0f);
            }
        }
    }
}