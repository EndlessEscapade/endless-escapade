using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Projectiles.Summon;

public class CrabPincers : ModProjectile
{
    private float leftRotation;
    private float rightRotation;

    public override void SetStaticDefaults() {
        ProjectileID.Sets.MinionSacrificable[Type] = true;
        ProjectileID.Sets.MinionTargettingFeature[Type] = true;
    }

    public override void SetDefaults() {
        Projectile.usesLocalNPCImmunity = true;
        Projectile.netImportant = true;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;
        Projectile.sentry = true;
        
        Projectile.width = 50;
        Projectile.height = 30;

        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        Projectile.localNPCHitCooldown = 30;

        Projectile.timeLeft = Projectile.SentryLifeTime;
    }
    
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        leftRotation += MathHelper.ToRadians(30f);
        rightRotation -= MathHelper.ToRadians(30f);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        return false;
    }
    
    public override void AI() {
        Projectile.velocity.Y += 0.3f;

        leftRotation = leftRotation.AngleLerp(0f, 0.1f);
        rightRotation = rightRotation.AngleLerp(0f, 0.1f);
    }

    public override bool PreDraw(ref Color lightColor) {
        var leftTexture = ModContent.Request<Texture2D>(Texture + "_Left").Value;
        var rightTexture = ModContent.Request<Texture2D>(Texture + "_Right").Value;
        
        var position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

        Main.EntitySpriteDraw(leftTexture,
            position + new Vector2(-24f, leftTexture.Height / 2f),
            null,
            lightColor,
            leftRotation,
            new Vector2(leftTexture.Width / 2f, leftTexture.Height),
            Projectile.scale,
            SpriteEffects.None,
            0f
        );
        
        Main.EntitySpriteDraw(rightTexture,
            position + new Vector2(24f, rightTexture.Height / 2f),
            null,
            lightColor,
            rightRotation,
            new Vector2(rightTexture.Width / 2f, rightTexture.Height),
            Projectile.scale,
            SpriteEffects.None,
            0f
        );
        
        return false;
    }
}
