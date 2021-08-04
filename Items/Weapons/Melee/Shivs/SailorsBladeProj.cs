using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace EEMod.Items.Weapons.Melee.Shivs
{
    public class SailorsBladeProj : Shiv
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Blade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.melee = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < 4; i++)
            {
                // int num = Dust.NewDust(target.Center, 2, 2, 182, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 6, Color.Red, 1);
                // Main.dust[num].noGravity = false;
            }
        }

        public override List<int> exclude => new List<int> { 0, 3, 6, 1, 5 };

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length / 2);
                spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, new Rectangle(0, 0, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}