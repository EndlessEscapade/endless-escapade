using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public abstract class Blade : EEProjectile
    {
        public virtual float rotationalCoverage => MathHelper.Pi;
        public virtual float RotationalOffset => MathHelper.PiOver2;
        protected float progression => projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
        public virtual float dirtSmashIntensity => 12;
        public virtual int shakeLength => 20;
        public virtual int shakeIntensity => 3;
        public virtual int AoE => 1000;
        public virtual bool canCrash => false;
        protected Player projOwner => Main.player[Projectile.owner];
        public virtual float damageIncreaseOverTime => 0.01f;
        public virtual float weight => 1;

        public float damageMultiplier = 1;

        public virtual List<int> exclude => new List<int> { };
        public float xDis;
        private readonly int width = 128;
        private readonly int height = 128;
        private readonly int frames = 5;
        private int SlashType;
        private readonly int Direction = Main.rand.Next(0, 2);
        private float rotation;
        private Vector2 offsetHoldout;

        public override void AI()
        {
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = projOwner.Center.X - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - Projectile.height / 2;
            if (projOwner.itemAnimation == projOwner.itemAnimationMax - 1)
            {
                SlashType = Main.rand.Next(1, 5);
                rotation = new Vector2(Main.MouseWorld.X - projOwner.Center.X, Main.MouseWorld.Y - projOwner.Center.Y).ToRotation();
                offsetHoldout = Vector2.Normalize(Main.MouseWorld - projOwner.Center) * 50;
            }
            if (projOwner.itemAnimation <= 1)
            {
                Projectile.Kill();
            }
            //Vector2 Norm = Vector2.Normalize(Main.MouseWorld - projOwner.Center); //unused
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int currentFrame = (int)(progression * frames);
            //Vector2 Norm = Vector2.Normalize(Main.MouseWorld - projOwner.Center); //unused
            if (Direction == 0)
            {
                spriteBatch.Draw(mod.GetTexture($"Projectiles/Slash{SlashType}"), Projectile.Center - Main.screenPosition + offsetHoldout, new Rectangle(0, height * currentFrame, width, height), Color.White, rotation, new Rectangle(0, 0, width, height).Size() / 2, 1, SpriteEffects.None, 0);
            }

            if (Direction == 1)
            {
                spriteBatch.Draw(mod.GetTexture($"Projectiles/Slash{SlashType}"), Projectile.Center - Main.screenPosition + offsetHoldout, new Rectangle(0, height * (frames - currentFrame), width, height), Color.White, rotation, new Rectangle(0, 0, width, height).Size() / 2, 1, SpriteEffects.None, 0);
            }

            return false;
        }
    }
}