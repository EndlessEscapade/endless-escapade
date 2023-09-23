using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EndlessEscapade.Common.Players;

namespace EndlessEscapade.Content.Items.Pearl
{
    public class PearlCannon : ModItem {
        
        public override void SetStaticDefaults() {

        }
        public override void SetDefaults() {
            Item.DamageType = DamageClass.Ranged;

            Item.useStyle = ItemUseStyleID.Shoot;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;

            Item.useTime = Item.useAnimation = 75;

            Item.width = 47;
            Item.height = 27;

            Item.damage = 10;
            Item.knockBack = 4;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 10;
        }

        int shoot = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            shoot++;
            Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().screenShake = 2.8f;

            player.velocity += velocity * -0.25f;

            SoundEngine.PlaySound(SoundID.Item61);

            Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<PearlCannonProjectile>(), 0, 0, player.whoAmI);

            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 50f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
                position += muzzleOffset;
            }

            if(shoot <= 9) {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 2f, ModContent.ProjectileType<PearlProjectile>(), damage, knockback, player.whoAmI);
            }
            else if(shoot == 10) {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 2f, ModContent.ProjectileType<BigPearlProjectile>(), damage *= 4, knockback *= 4, player.whoAmI);
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().screenShake = 5f;
                player.velocity += velocity * -1.12f;
                shoot = 0;
            }
                
            return false;
        }
    }
    public class PearlCannonProjectile : ModProjectile {

        public override string Texture => "Terraria/Images/Projectile_0";

        public Player owner => Main.player[Projectile.owner];

        private bool firstFrame;
        private Vector2 roationPos => Projectile.rotation.ToRotationVector2();

        private float offset = 30;
        public override void SetStaticDefaults() {

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.width = Projectile.height = 2;

            Projectile.aiStyle = -1;

            Projectile.penetrate = -1;

            Projectile.timeLeft = 999999;

            Projectile.friendly = true;

            Projectile.tileCollide = false;

            Projectile.ignoreWater = true;
        }
        public override void AI() {
            Projectile.Center = owner.MountedCenter; // Returns the player center.

            owner.heldProj = Projectile.whoAmI;

            if(owner.itemTime <= 1) {
                Projectile.active = false;
            }
            if (!firstFrame) {
                firstFrame = true;

                Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }
            if (Projectile.ai[0] == 2) {
                offset = 10;
            }
            if (Projectile.ai[0] > 2) {
                offset = Math.Clamp(MathHelper.SmoothStep(offset, 35, 0.09f), 0, 21);                
            }            
            Projectile.ai[0]++;
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/PearlCannonProj").Value;

            Vector2 drawPosition = (owner.MountedCenter + (roationPos * offset)) - Main.screenPosition;
            drawPosition.Y += owner.gfxOffY;
            drawPosition += new Vector2(0, 2 * owner.direction).RotatedBy(Projectile.rotation);

            float rotation = roationPos.ToRotation() + (owner.direction == 1 ? 0 : -MathF.PI);
            SpriteEffects spriteEffects = (owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            Vector2 origin = texture.Size() / 2;

            Main.spriteBatch.Draw(texture, drawPosition, null, lightColor, rotation, origin, 1f, spriteEffects, 0.0f);

            return false;
        }
        public override bool? CanDamage() { return false; }
    }
    public class PearlProjectile : ModProjectile {

        int[] dustTypes = { DustID.UnusedWhiteBluePurple, DustID.ShadowbeamStaff };
        public override void SetStaticDefaults() {
            
        }
        public override void SetDefaults() {

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = -1;

            Projectile.width = Projectile.height = 22;

            Projectile.friendly = true;

            Projectile.tileCollide = true;

            Projectile.penetrate = -1;

            Projectile.timeLeft = 180;      
        }
        public override void AI() {
            Projectile.velocity.X *= 0.986f;
            Projectile.velocity.Y += 0.3f;

            for (int i = 0; i < Main.rand.Next(2, 3); i++) {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, dustTypes[Main.rand.Next(2)], Main.rand.Next(-4, 4), Main.rand.Next(-4, 4));
                dust.alpha = 0;
                dust.noGravity = true;
                dust.scale = 2f;
            }
        }
        int bounce = 5;
        public override bool OnTileCollide(Vector2 oldVelocity) {
            if(bounce > 0) {
                bounce--;
                Projectile.velocity.Y = -oldVelocity.Y/2;
                return false;
            }
            else { return true; }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (bounce > 0) {
                bounce--;
                Projectile.velocity.Y = -Projectile.oldVelocity.Y/2;
            }
            else { Projectile.Kill(); }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D glow = Mod.Assets.Request<Texture2D>("Assets/Projectile_540").Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.Blue * 0.99f, Projectile.rotation, glow.Size() / 2, 2.3f, SpriteEffects.None, 0);

            return true;
        }
    }
    public class BigPearlProjectile : ModProjectile
    {
        int[] dustTypes = { DustID.UnusedWhiteBluePurple, DustID.ShadowbeamStaff };
        public override void SetStaticDefaults() {

        }
        public override void SetDefaults() {

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = -1;

            Projectile.width = Projectile.height = 44;

            Projectile.friendly = true;

            Projectile.tileCollide = true;

            Projectile.penetrate = -1;

            Projectile.timeLeft = 180;
        }
        public override void AI() {
            Projectile.velocity.X *= 0.986f;
            Projectile.velocity.Y += 0.65f;

            for (int i = 0; i < Main.rand.Next(6, 8); i++) {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, dustTypes[Main.rand.Next(2)], Main.rand.Next(-12, 12), Main.rand.Next(-12, 12));
                dust.alpha = 0;
                dust.noGravity = true;
                dust.scale = 2.4f;
            }


        }
        int bounce = 10;
        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (bounce > 0) {
                bounce--;
                Projectile.velocity.Y = -oldVelocity.Y / 2;
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().screenShake = 3f;
                return false;
            }
            else { return true; }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (bounce > 0) {
                bounce--;
                Projectile.velocity.Y = -Projectile.oldVelocity.Y / 2;
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().screenShake = 3f;

            }
            else { Projectile.Kill(); }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D glow = Mod.Assets.Request<Texture2D>("Assets/Projectile_540").Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.Blue * 0.99f, Projectile.rotation, glow.Size() / 2, 2.3f, SpriteEffects.None, 0);

            return true;
        }
    }
}
