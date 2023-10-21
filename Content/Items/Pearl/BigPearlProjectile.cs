using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Pearl;

public class BigPearlProjectile : ModProjectile
{
    private static readonly int[] dustTypes = {
        DustID.UnusedWhiteBluePurple,
        DustID.ShadowbeamStaff
    };

    private int bounce = 10;

    public override void SetDefaults() {
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.width = 44;
        Projectile.height = 44;
        Projectile.aiStyle = -1;
        AIType = -1;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 180;
    }

    public override void AI() {
        Projectile.velocity.X *= 0.986f;
        Projectile.velocity.Y += 0.65f;

        for (var i = 0; i < Main.rand.Next(6, 8); i++) {
            var dust = Dust.NewDustDirect(Projectile.Center, 1, 1, dustTypes[Main.rand.Next(2)], Main.rand.Next(-12, 12), Main.rand.Next(-12, 12));
            dust.alpha = 0;
            dust.noGravity = true;
            dust.scale = 2.4f;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        if (bounce > 0) {
            bounce--;
            Projectile.velocity.Y = -oldVelocity.Y / 2;
            return false;
        }

        return true;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (bounce > 0) {
            bounce--;
            Projectile.velocity.Y = -Projectile.oldVelocity.Y / 2;
        }
        else {
            Projectile.Kill();
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        var glow = Mod.Assets.Request<Texture2D>("Assets/Projectile_540").Value;

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.Blue * 0.99f, Projectile.rotation, glow.Size() / 2, 2.3f, SpriteEffects.None);

        return true;
    }
}