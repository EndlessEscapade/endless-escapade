using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items;
using System;
using Terraria.ID;

namespace EEMod.Projectiles.TennisRackets
{
    public class TerracketProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            projectile.width = 134;
            projectile.height = 56;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
            projectile.alpha = 255;
            projectile.damage = 30;
        }

        private int frame;
        private readonly int numOfFrames = 7;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(frame);
            writer.Write(indexOfProjectile);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            frame = reader.ReadInt32();
            indexOfProjectile = reader.ReadInt32();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle(0, tex.Height / numOfFrames * frame, tex.Width, tex.Height / numOfFrames), lightColor * (1 - (projectile.alpha / 255f)), projectile.rotation, new Rectangle(0, tex.Height / numOfFrames * frame, tex.Width, tex.Height / numOfFrames).Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }

        private int indexOfProjectile;
        public Vector2 goTo = Main.MouseWorld;
        public int owner;

        private int d = 0;
        public override void AI()
        {
            d++;
            if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > 5 && d % 3 == 0)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center + new Vector2(Main.rand.Next(-32, 33), Main.rand.Next(-32, 33)), 178);
                dust.noGravity = true;
            }
            if (projectile.ai[1] > 0)
            {
                projectile.ai[1]--;
            }

            Player player = Main.player[projectile.owner];
            if (player.inventory[player.selectedItem].type != ModContent.ItemType<Terracket>())
            {
                projectile.Kill();
            }

            if (projectile.alpha > 0)
            {
                projectile.alpha--;
            }
            float radial = 75;
            float inverseSpeed = 100;
            float dampeningEffect = 0.07f;
            projectile.timeLeft = 100;
            if (Main.myPlayer == projectile.owner)
            {
                if (player.direction == 1)
                {
                    frame = (int)((projectile.Center.X - player.Center.X) / radial * (numOfFrames * 0.5f) + (numOfFrames * 0.5f));
                    projectile.rotation = 0;
                }
                else
                {
                    frame = (int)((player.Center.X - projectile.Center.X) / radial * (numOfFrames * 0.5f) + (numOfFrames * 0.5f));
                    projectile.rotation = MathHelper.Pi;
                }
            }
            frame = (int)MathHelper.Clamp(frame, 0, numOfFrames - 1);
            goTo = Main.MouseWorld;
            if (goTo.X < player.Center.X - radial)
            {
                goTo.X = player.Center.X - radial;
            }
            if (goTo.X > player.Center.X + radial)
            {
                goTo.X = player.Center.X + radial;
            }
            if (goTo.Y < player.Center.Y - radial)
            {
                goTo.Y = player.Center.Y - radial;
            }
            if (goTo.Y > player.Center.Y + radial)
            {
                goTo.Y = player.Center.Y + radial;
            }
            if (projectile.ai[0] == 1)
            {
                goTo = player.Center + new Vector2(0, 50);
            }
            if (Main.myPlayer == projectile.owner)
            {
                projectile.velocity += (goTo - projectile.Center) / inverseSpeed - (projectile.velocity * dampeningEffect);
                projectile.netUpdate = true;
            }
        }
    }
}