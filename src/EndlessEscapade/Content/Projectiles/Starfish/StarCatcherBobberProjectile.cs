namespace EndlessEscapade.Content.Projectiles.Starfish;

public class StarCatcherBobberProjectile : ModProjectile
{
    public float Intensity { get; private set; }

    public override void SetDefaults() {
        Projectile.netImportant = true;
        Projectile.bobber = true;

        Projectile.width = 14;
        Projectile.height = 14;

        Projectile.aiStyle = ProjAIStyleID.Bobber;
    }

    public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor) {
        lineOriginOffset = new Vector2(46, -36);
    }

    public override void AI() {
        if (Projectile.wet) {
            Intensity += 0.1f;
        }
        else {
            Intensity -= 0.1f;
        }
    }

    // TODO: Implement proper visuals.
    public override bool PreDraw(ref Color lightColor) {
        var texture = ModContent.Request<Texture2D>(Texture + "_Outline").Value;
        var effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        var offsetX = 0;
        var offsetY = 0;
        var originX = (texture.Width - Projectile.width) / 2f + Projectile.width / 2f;

        ProjectileLoader.DrawOffset(Projectile, ref offsetX, ref offsetY, ref originX);

        var x = Projectile.position.X - Main.screenPosition.X + originX + offsetX;
        var y = Projectile.position.Y - Main.screenPosition.Y + Projectile.height / 2f + Projectile.gfxOffY;

        var frame = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
        var origin = new Vector2(originX, Projectile.height / 2f + offsetY);

        Main.EntitySpriteDraw(
            texture,
            new Vector2(x, y),
            frame,
            Projectile.GetAlpha(Color.White),
            Projectile.rotation,
            origin,
            Projectile.scale,
            effects
        );

        return true;
    }
}
