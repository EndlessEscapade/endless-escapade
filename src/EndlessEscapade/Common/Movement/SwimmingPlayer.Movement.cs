using EndlessEscapade.Utilities.Extensions;
using Terraria.DataStructures;

namespace EndlessEscapade.Common.Movement;

public sealed partial class SwimmingPlayer : ModPlayer
{
    private float bodyRotation;
    private float headRotation;

    private float targetBodyRotation;
    private float targetHeadRotation;

    public float Speed = 5f;

    public float Resistance = 0.1f;

    public override void ResetEffects() {
        base.ResetEffects();

        Speed = 5f;
        Resistance = 0.1f;
    }

    public override void PostUpdate() {
        base.PostUpdate();

        if (Player.IsUnderwater()) {
            var offset = Player.Center + Player.velocity - Player.Center;
            var rotation = offset.ToRotation();

            if (Player.direction == -1) {
                rotation = MathHelper.WrapAngle(rotation + MathHelper.Pi);
            }

            var maxHeadRotation = MathHelper.ToRadians(60f);
            var minHeadRotation = MathHelper.ToRadians(-60f);

            targetHeadRotation = MathHelper.Clamp(rotation, minHeadRotation, maxHeadRotation);
            targetBodyRotation = Player.velocity.ToRotation() + MathHelper.PiOver2;

            if (Player.velocity.LengthSquared() > 0f) {
                targetBodyRotation = Player.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else {
                targetBodyRotation = 0f;
            }

            var swimDirection = new Vector2(
                (Player.controlRight ? 1f : 0f) + (Player.controlLeft ? -1f : 0f),
                (Player.controlDown ? 1f : 0f) + (Player.controlUp ? -1f : 0f)
            );

            swimDirection = swimDirection.SafeNormalize(Vector2.Zero);

            if (swimDirection.LengthSquared() > 0f) {
                Player.velocity = Vector2.SmoothStep(Player.velocity, swimDirection * Speed, 0.2f);
            }
        }
        else {
            targetHeadRotation = 0f;
            targetBodyRotation = 0f;
        }

        headRotation = MathHelper.Lerp(headRotation, targetHeadRotation, 0.2f);
        bodyRotation = MathHelper.Lerp(bodyRotation, targetBodyRotation, 0.2f);
    }
}
