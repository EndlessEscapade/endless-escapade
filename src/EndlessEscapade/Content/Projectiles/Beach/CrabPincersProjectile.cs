using System.Collections.Generic;
using Terraria.DataStructures;

namespace EndlessEscapade.Content.Projectiles.Beach;

public class CrabPincersProjectile : ModProjectile
{
    private float leftRotation;
    private float rightRotation;
    private float scale;

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
        Projectile.hide = true;

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

    public override void OnSpawn(IEntitySource source) {
        var tileCoordinates = Projectile.position.ToTileCoordinates();
        var collisionPosition = new Vector2(tileCoordinates.X, tileCoordinates.Y + 1) * 16f;

        Collision.HitTiles(collisionPosition, Projectile.velocity, Projectile.width, Projectile.height);
    }

    public override void AI() {
        Projectile.velocity.Y += 0.3f;

        scale = MathHelper.Lerp(scale, 1f, 0.1f);

        leftRotation = leftRotation.AngleLerp(0f, 0.1f);
        rightRotation = rightRotation.AngleLerp(0f, 0.1f);
    }

    public override void DrawBehind(
        int index,
        List<int> behindNPCsAndTiles,
        List<int> behindNPCs,
        List<int> behindProjectiles,
        List<int> overPlayers,
        List<int> overWiresUI
    ) {
        behindNPCsAndTiles.Add(index);
    }

    public override bool PreDraw(ref Color lightColor) {
        var position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

        var eyes = ModContent.Request<Texture2D>(Texture + "_Eyes").Value;

        Main.EntitySpriteDraw(
            eyes,
            position + new Vector2(0f, eyes.Height),
            null,
            lightColor,
            0f,
            new Vector2(eyes.Width / 2f, eyes.Height),
            new Vector2(1f, scale),
            SpriteEffects.None
        );

        var leftPincer = ModContent.Request<Texture2D>(Texture + "_Left").Value;

        Main.EntitySpriteDraw(
            leftPincer,
            position + new Vector2(-24f, leftPincer.Height / 2f),
            null,
            lightColor,
            leftRotation,
            new Vector2(leftPincer.Width / 2f, leftPincer.Height),
            new Vector2(1f, scale),
            SpriteEffects.None
        );

        var rightPincer = ModContent.Request<Texture2D>(Texture + "_Right").Value;

        Main.EntitySpriteDraw(
            rightPincer,
            position + new Vector2(24f, rightPincer.Height / 2f),
            null,
            lightColor,
            rightRotation,
            new Vector2(rightPincer.Width / 2f, rightPincer.Height),
            new Vector2(1f, scale),
            SpriteEffects.None
        );

        return true;
    }
}
