using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class PhantomTable : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Table");
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 32;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;

            Projectile.damage = 0;

            Projectile.timeLeft = 1000000;
        }

        public Vector2 desiredCenter;
        public Vector2 initCenter;
        public Vector2 oldCenter;

        public Vector2 falseVelocity;

        public int dyingTicks = 0;

        public override void AI()
        {
            //Taken from Spirit w/ permission

            if (dyingTicks <= 0)
            {
                if (Projectile.ai[0] <= 40)
                {
                    initCenter = Projectile.Center;
                }
                else if (Projectile.ai[0] <= 80 && Projectile.ai[0] > 40)
                {
                    Projectile.Center = Vector2.SmoothStep(initCenter, desiredCenter, (Projectile.ai[0] - 40) / 40f);
                }
                else
                {
                    Projectile.Center = desiredCenter;
                    //Projectile.velocity = desiredCenter - oldCenter;

                    if (Projectile.ai[1] == 0)
                    {
                        Main.NewText(desiredCenter);
                        Main.NewText(oldCenter);
                    }

                    foreach (Player player in Main.player)
                    {
                        if (!player.active || player.controlDown) return;

                        var playerBox = new Rectangle((int)player.position.X, (int)player.position.Y + player.height, player.width, 1);
                        var floorBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y - (int)falseVelocity.Y, Projectile.width, 8 + (int)Math.Max(player.velocity.Y, 0));

                        if (playerBox.Intersects(floorBox) && player.velocity.Y > 0 && !Collision.SolidCollision(player.Bottom, player.width, (int)Math.Max(1 + falseVelocity.Y, 0)))
                        {
                            player.gfxOffY = Projectile.gfxOffY;
                            player.position.Y = Projectile.position.Y - player.height;
                            player.velocity.Y = 0;
                            player.fallStart = (int)(player.position.Y / 16f);

                            if (player == Main.LocalPlayer)
                                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Main.LocalPlayer.whoAmI);
                        }
                    }
                }
            }
            else
            {
                if (dyingTicks <= 40)
                {
                    Projectile.Center = Vector2.SmoothStep(desiredCenter, initCenter, dyingTicks / 40f);
                }
                else
                {
                    Projectile.Center = initCenter;
                }

                dyingTicks++;
            }

            Projectile.ai[0]++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}