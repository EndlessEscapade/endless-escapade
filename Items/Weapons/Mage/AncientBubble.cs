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
    public class AncientBubbleLarge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 74;
            projectile.height = 74;
            projectile.aiStyle = -1;
            projectile.penetrate = 1;
            projectile.timeLeft = 120 + 128;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (projectile.ai[0] < 120)
            {
                if (owner.controlUseItem)
                {
                    projectile.Center = new Vector2(owner.Center.X + owner.DirectionTo(Main.MouseWorld).X * 32, owner.Center.Y - 24);
                    projectile.ai[0]++;

                    owner.direction = projectile.Center.X - owner.Center.X <= 0 ? -1 : 1;
                }
                else
                    projectile.Kill();
            }
            else if(projectile.ai[0] == 120)
            {
                projectile.velocity += owner.DirectionTo(Main.MouseWorld) * 12;
                projectile.ai[0]++;

                owner.direction = projectile.Center.X - owner.Center.X <= 0 ? -1 : 1;
            }
            else
            {
                if (projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && Main.LocalPlayer.controlUseTile)
                {
                    CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), new Color(255, 155, 0, 100), "Pop!");
                    Explode();
                }

                projectile.velocity.Y -= 0.05f;

                projectile.rotation = projectile.velocity.X / 16f;

                projectile.alpha += 2;
            }

            projectile.scale = Helpers.Clamp(0.75f + (projectile.ai[0] / 480f), 0.75f, 1f);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] >= 120)
            {
                Texture2D tex = mod.GetTexture("Projectiles/Mage/AncientBubbleLargeFlash");
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, tex.Frame(), Color.White * Math.Sin(Main.GameUpdateCount / 5f).PositiveSin() * (1 - (projectile.alpha / 255)), projectile.rotation, tex.Frame().Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 position = projectile.position + new Vector2(Main.rand.Next(-50, 51) * .05f - 1.5f, Main.rand.Next(-50, 51) * .05f - 1.5f);
                Dust dust = Main.dust[Dust.NewDust(position, projectile.width, projectile.height, 165, 0f, -2f, 0, default(Color), 2f * .4f)];
                dust.noGravity = true;
                if (dust.position != projectile.Center)
                    dust.velocity = projectile.DirectionTo(dust.position) * 8f;
            }
        }

        private void Explode()
        {
            var list = Main.npc.Where(x => Vector2.Distance(x.Center, projectile.Center) <= 144);

            foreach (var enemy in list)
            {
                if (enemy.friendly == false) enemy.StrikeNPC(projectile.damage, 0f, projectile.Center.X - enemy.Center.X <= 0 ? -1 : 1);
            }

            projectile.Kill();

            Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake = 10;
        }
    }
}