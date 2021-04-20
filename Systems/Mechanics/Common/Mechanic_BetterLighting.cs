using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using EEMod.Systems;
using EEMod.Config;

namespace EEMod.Systems
{
    public class BetterLighting : Mechanic
    {
        public override void PostDrawProjectiles()
        {
            if (!EEModConfigClient.Instance.BetterLighting)
                return;

        }
    }
}
        /*public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WhiteBlock");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 60000;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
            projectile.light = 0;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
        }*/