using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Pearl;

public class PearlCannonProjectile : ModProjectile
{
    private bool firstFrame;

    private float offset = 30;

    public override string Texture => "Terraria/Images/Projectile_0";

    public Player owner => Main.player[Projectile.owner];
    private Vector2 roationPos => Projectile.rotation.ToRotationVector2();

    public override void SetStaticDefaults() {
        Projectile.DamageType = DamageClass.Ranged;

        Projectile.width = 2;
        Projectile.height = 2;

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

        if (owner.itemTime <= 1) {
            Projectile.active = false;
            return;
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
        var texture = Mod.Assets.Request<Texture2D>("Assets/PearlCannonProj").Value;

        var drawPosition = owner.MountedCenter + roationPos * offset - Main.screenPosition;
        drawPosition.Y += owner.gfxOffY;
        drawPosition += new Vector2(0, 2 * owner.direction).RotatedBy(Projectile.rotation);

        var rotation = roationPos.ToRotation() + (owner.direction == 1 ? 0 : -MathF.PI);
        var spriteEffects = owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        var origin = texture.Size() / 2;

        Main.spriteBatch.Draw(texture, drawPosition, null, lightColor, rotation, origin, 1f, spriteEffects, 0.0f);

        return false;
    }

    public override bool? CanDamage() {
        return false;
    }
}