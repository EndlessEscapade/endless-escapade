
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
using Terraria.Audio;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class ShimmerShotProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shimmer Shot");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 72;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            // Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
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
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 20 && Projectile.frame < 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

                if(Projectile.frame >= 3)
                {
                    whiteFlash = 1f;
                    SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
                }
            }

            if (whiteFlash > 0) { whiteFlash -= 0.2f; }
            if(whiteFlash <= 0.25f && whiteFlash >= 0.1f) { Projectile.ai[1] = 1; }

            if (Projectile.ai[1] == 1)
            {
                whiteFlash = 0.25f + ((float)Math.Sin(Main.GameUpdateCount / 5f) / 8f);
            }


            Player projOwner = Main.player[Projectile.owner];
            float progression = projOwner.itemAnimation / (float)projOwner.itemAnimationMax;

            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            Projectile.rotation = (Main.MouseWorld - projOwner.Center).ToRotation();
            Projectile.position.X = projOwner.Center.X - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - Projectile.height / 2;
            float speed = speedOfArrow;
            projOwner.bodyFrame.Y = 56 * (6 + (int)(gravAccel - minGrav));
            if (!projOwner.controlUseItem && Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 16;
                Projectile.ai[0] = 1;

                if (Projectile.frame >= 3)
                {
                    Vector2 comedy = Vector2.Normalize(Main.MouseWorld - projOwner.Center);
                    for (float i = 0; i < projCount; i++)
                    {
                        Projectile projectile2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), projOwner.Center, comedy.RotatedBy(-((projCount / 2) * projSpread) + (i * projSpread)) / Max * speed, newProj, 10, 10f, Main.myPlayer);

                        PrimitiveSystem.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Cyan, Color.Magenta, i / projCount), 40));
                    }
                }
                else
                {
                    Vector2 comedy = Vector2.Normalize(Main.MouseWorld - projOwner.Center);
                    Projectile projectile2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), projOwner.Center, comedy / Max * speed, newProj, 10, 10f, Main.myPlayer);

                    PrimitiveSystem.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Cyan, Color.Magenta, Main.rand.NextFloat(0, 1)), 40));
                }
            }

            if(projOwner.HeldItem.type != ModContent.ItemType<ShimmerShot>())
            {
                Projectile.Kill();
            }
            
            if(Projectile.ai[0] == 1)
            {
                Projectile.alpha += 16;
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

        public override void PostDraw(Color lightColor)
        {
            if(whiteFlash > 0)
            {
                Rectangle rect = new Rectangle(0, 0, 46, 72);
                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Weapons/Ranger/Longbows/ShimmerShotProjGlow").Value, new Rectangle((int)Projectile.Center.ForDraw().X, (int)Projectile.Center.ForDraw().Y, 46, 72), rect, Color.White * whiteFlash * ((255 - Projectile.alpha) / 255f), Projectile.rotation, rect.Size() / 2f, SpriteEffects.None, 0f);
            }
        }
    }
}