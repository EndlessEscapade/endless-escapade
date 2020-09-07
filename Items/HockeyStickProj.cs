/*using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Projectiles;
using System.IO;
using Terraria.ID;

namespace EEMod.Items
{
    public class HockeyStickProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            projectile.width = 114;
            projectile.height = 50;
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

        int frame;
        int numOfFrames = 8;
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
        int indexOfProjectile;
        public Vector2 goTo = Main.MouseWorld;
        public int owner;
        public override void AI()
        {
            if (projectile.ai[1] > 0)
            {
                projectile.ai[1]--;
            }

            Player player = Main.player[projectile.owner];
            if(player.inventory[player.selectedItem].type != ModContent.ItemType<TennisRachet>())
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
                    projectile.rotation = (float)MathHelper.Pi;
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
*/