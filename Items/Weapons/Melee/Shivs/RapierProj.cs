using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Items.Weapons.Melee.Shivs
{
    public class RapierProj : ModProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapier");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.damage = 0;
            projectile.timeLeft = 120;
            projectile.alpha = 255;
            projectile.extraUpdates = 3;
        }
        public Vector2 start;
        public Vector2 end;
        public Vector2 mid;
        public override void AI()
        {
            projectile.ai[0] += (1 - projectile.ai[0]) / 16f;
            projectile.Center = Helpers.TraverseBezier(end, start, mid, projectile.ai[0]);
        }
    }
}