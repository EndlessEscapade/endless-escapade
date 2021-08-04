using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;
using EEMod;

namespace EEMod.Items.Weapons.Mage
{
    public class AncientBubbleLarge : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 74;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120 + 128;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.magic = true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (Projectile.ai[0] < 120)
            {
                if (owner.controlUseItem)
                {
                    Projectile.Center = new Vector2(owner.Center.X + owner.DirectionTo(Main.MouseWorld).X * 32, owner.Center.Y - 24);
                    Projectile.ai[0]++;

                    owner.direction = Projectile.Center.X - owner.Center.X <= 0 ? -1 : 1;
                }
                else
                    Projectile.Kill();
            }
            else if(Projectile.ai[0] == 120)
            {
                Projectile.velocity += owner.DirectionTo(Main.MouseWorld) * 12;
                Projectile.ai[0]++;

                owner.direction = Projectile.Center.X - owner.Center.X <= 0 ? -1 : 1;
            }
            else
            {
                if (Projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && Main.LocalPlayer.controlUseTile)
                {
                    CombatText.NewText(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), new Color(255, 155, 0, 100), "Pop!");
                    Explode();
                }

                Projectile.velocity.Y -= 0.05f;

                Projectile.rotation = Projectile.velocity.X / 16f;

                Projectile.alpha += 2;
            }

            Projectile.scale = Helpers.Clamp(0.75f + (Projectile.ai[0] / 480f), 0.75f, 1f);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Projectile.ai[0] >= 120)
            {
                Texture2D tex = mod.GetTexture("Projectiles/Mage/AncientBubbleLargeFlash");
                spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, tex.Frame(), Color.White * Math.Sin(Main.GameUpdateCount / 5f).PositiveSin() * (1 - (Projectile.alpha / 255)), Projectile.rotation, tex.Frame().Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 position = Projectile.position + new Vector2(Main.rand.Next(-50, 51) * .05f - 1.5f, Main.rand.Next(-50, 51) * .05f - 1.5f);
                Dust dust = Main.dust[Dust.NewDust(position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default(Color), 2f * .4f)];
                dust.noGravity = true;
                if (dust.position != Projectile.Center)
                    dust.velocity = Projectile.DirectionTo(dust.position) * 8f;
            }
        }

        private void Explode()
        {
            var list = Main.npc.Where(x => Vector2.Distance(x.Center, Projectile.Center) <= 144);

            foreach (var enemy in list)
            {
                if (enemy.friendly == false) enemy.StrikeNPC(Projectile.damage, 0f, Projectile.Center.X - enemy.Center.X <= 0 ? -1 : 1);
            }

            Projectile.Kill();

            Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake = 10;
        }
    }
}