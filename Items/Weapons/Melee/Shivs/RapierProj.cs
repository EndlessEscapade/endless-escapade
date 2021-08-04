using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Items.Weapons.Melee.Shivs
{
    public class RapierProj : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapier");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.melee = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 3;
        }
        public Vector2 start;
        public Vector2 end;
        public Vector2 mid;
        public override void AI()
        {
            Projectile.ai[0] += (1 - Projectile.ai[0]) / 16f;
            Projectile.Center = Helpers.TraverseBezier(end, start, mid, Projectile.ai[0]);
        }
    }
}