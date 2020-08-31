using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.OceanMap
{
    public class Cloud6 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Land");
        }

        public override void SetDefaults()
        {
            projectile.width = 144;
            projectile.height = 42;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 newPos = projectile.Center + Main.screenPosition;
            Rectangle rect = new Rectangle(0, 0, projectile.width, projectile.height);
            Color lightColour = Lighting.GetColor((int)newPos.X / 16, (int)newPos.Y / 16);
            spriteBatch.Draw(Main.projectileTexture[projectile.type], (projectile.Center + Main.screenPosition).ForDraw(), rect, lightColour * ((255 - projectile.alpha) / 255f), 0f, rect.Size()/2, projectile.scale,SpriteEffects.None,0f);
            return false;
        }
        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            projectile.position.X--;
        }
    }
}
