using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.TennisRackets;

namespace EEMod.Projectiles.TennisRackets
{
    public class TennisRacketProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            Projectile.width = 114;
            Projectile.height = 50;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1;
            Projectile.alpha = 255;
            Projectile.damage = 30;
        }

        private int frame;
        private readonly int numOfFrames = 8;

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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, tex.Height / numOfFrames * frame, tex.Width, tex.Height / numOfFrames), lightColor * (1 - (Projectile.alpha / 255f)), Projectile.rotation, new Rectangle(0, tex.Height / numOfFrames * frame, tex.Width, tex.Height / numOfFrames).Size() / 2, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }

        private int indexOfProjectile;
        public Vector2 goTo = Main.MouseWorld;
        public int owner;

        public override void AI()
        {
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1]--;
            }

            Player player = Main.player[Projectile.owner];
            if (player.inventory[player.selectedItem].type != ModContent.ItemType<TennisRacket>())
            {
                Projectile.Kill();
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha--;
            }
            float radial = 75;
            float inverseSpeed = 100;
            float dampeningEffect = 0.07f;
            Projectile.timeLeft = 100;
            if (Main.myPlayer == Projectile.owner)
            {
                if (player.direction == 1)
                {
                    frame = (int)((Projectile.Center.X - player.Center.X) / radial * (numOfFrames * 0.5f) + (numOfFrames * 0.5f));
                    Projectile.rotation = 0;
                }
                else
                {
                    frame = (int)((player.Center.X - Projectile.Center.X) / radial * (numOfFrames * 0.5f) + (numOfFrames * 0.5f));
                    Projectile.rotation = MathHelper.Pi;
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
            if (Projectile.ai[0] == 1)
            {
                goTo = player.Center + new Vector2(0, 50);
            }
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.velocity += (goTo - Projectile.Center) / inverseSpeed - (Projectile.velocity * dampeningEffect);
                Projectile.netUpdate = true;
            }
        }
    }
}