using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Items.Weapons.Melee
{
    public class AxeLightning : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Warhammer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.timeLeft = 30;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 3;
        }

        Vector2 initialVelocity = Vector2.Zero;

        public Vector2 DrawPos;
        public int boost;
        public override void AI()
        {
            if (initialVelocity == Vector2.Zero)
            {
                initialVelocity = Projectile.velocity;
            }

            if (Projectile.timeLeft == 0) Projectile.position -= Projectile.velocity;
            if (Projectile.timeLeft < 8) Projectile.velocity = initialVelocity.RotatedBy(Main.rand.NextFloat(-1f, 1f));

            DrawPos = Projectile.position;
        }
    }
}