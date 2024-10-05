using System.IO;

namespace EndlessEscapade.Content.Projectiles.Starfish;

public class SpinnerFishProjectile : ModProjectile
{
    private Vector2 offset;
    private ref float Target => ref Projectile.ai[0];
    private ref float Timer => ref Projectile.ai[1];

    public bool StickingToNPC { get; private set; }
    public bool StickingToTile { get; private set; }

    public bool StickingToAnything => StickingToNPC || StickingToTile;

    public override void SetDefaults() {
        Projectile.usesLocalNPCImmunity = true;
        Projectile.friendly = true;

        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.aiStyle = -1;

        Projectile.timeLeft = 180;
        Projectile.penetrate = -1;
        Projectile.localNPCHitCooldown = 30;
    }

    public override void SendExtraAI(BinaryWriter writer) {
        writer.Write(StickingToNPC);
        writer.Write(StickingToTile);
    }

    public override void ReceiveExtraAI(BinaryReader reader) {
        StickingToNPC = reader.ReadBoolean();
        StickingToTile = reader.ReadBoolean();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        Projectile.scale = 1.25f;
        Projectile.rotation += MathHelper.ToRadians(Main.rand.NextFloat(5f, 15f));

        if (StickingToAnything) {
            return;
        }

        offset = target.Center - Projectile.Center + Projectile.velocity;

        Target = target.whoAmI;
        StickingToNPC = true;

        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        if (StickingToAnything) {
            return false;
        }

        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

        StickingToTile = true;

        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);

        return false;
    }

    public override void AI() {
        Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.2f);

        if (Projectile.timeLeft < 255 / 25) {
            Projectile.alpha += 25;
        }

        UpdateTargetStick();
        UpdateTileStick();

        if (StickingToAnything) {
            return;
        }

        UpdateGravity();

        Projectile.rotation += Projectile.velocity.X * 0.1f;
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

        Main.EntitySpriteDraw(texture, new Vector2(x, y), frame, Projectile.GetAlpha(Color.White), Projectile.rotation, origin, Projectile.scale, effects);

        return true;
    }

    private void UpdateTargetStick() {
        if (!StickingToNPC) {
            return;
        }

        var target = Main.npc[(int)Target];

        if (!target.active) {
            Projectile.Kill();
            return;
        }

        Projectile.tileCollide = false;

        Projectile.Center = target.Center - offset;
        Projectile.gfxOffY = target.gfxOffY;
    }

    private void UpdateTileStick() {
        if (!StickingToTile) {
            return;
        }

        Projectile.velocity *= 0.5f;
    }

    private void UpdateGravity() {
        Timer++;

        if (Timer < 10f) {
            return;
        }

        Projectile.velocity.Y += 0.2f;
    }
}
