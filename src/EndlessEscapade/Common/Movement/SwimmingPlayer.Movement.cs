using EndlessEscapade.Utilities.Extensions;
using Terraria.DataStructures;

namespace EndlessEscapade.Common.Movement;

/// <summary>
///     Handles player movement and graphics while swimming.
/// </summary>
public sealed partial class SwimmingPlayer : ModPlayer
{
    private StatModifier speedModifier = new();
    private StatModifier accelerationModifier = new();

    private Vector2 velocity;

    private bool oldUnderwater;

    public override void PostUpdate() {
        base.PostUpdate();

        if (!oldUnderwater && Player.IsUnderwater()) {
            velocity = Player.velocity;
        }

        if (!Player.IsUnderwater()) {
            velocity = Vector2.Zero;
        }
        else {
            var direction = new Vector2(
                Player.controlRight.ToInt() + -Player.controlLeft.ToInt(),
                Player.controlDown.ToInt() + -Player.controlUp.ToInt()
            );

            direction = direction.SafeNormalize(Vector2.Zero);

            if (direction.LengthSquared() > 0f) {
                var acceleration = accelerationModifier.ApplyTo(0.25f);
                var speed = speedModifier.ApplyTo(4f);

                velocity += direction * acceleration;
                velocity = Vector2.Clamp(velocity, new Vector2(-speed), new Vector2(speed));
            }
            else {
                velocity *= 0.95f;
            }

            Player.velocity = velocity;
        }

        oldUnderwater = Player.IsUnderwater();
    }

    public ref StatModifier GetMovementSpeed() {
        return ref speedModifier;
    }

    public ref StatModifier GetMovementAcceleration() {
        return ref accelerationModifier;
    }
}
