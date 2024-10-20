using EndlessEscapade.Utilities.Extensions;
using Terraria.DataStructures;

namespace EndlessEscapade.Common.Movement;

/// <summary>
///     Handles player movement and graphics while swimming.
/// </summary>
public sealed partial class SwimmingPlayer : ModPlayer
{
    private float bodyRotation;
    private float headRotation;

    private float targetBodyRotation;
    private float targetHeadRotation;

    public override void PostUpdateMiscEffects() {
        base.PostUpdateMiscEffects();

        if (Player.IsUnderwater()) {
            var rotation = Player.velocity.ToRotation();

            if (Player.direction == -1) {
                rotation = MathHelper.WrapAngle(rotation + MathHelper.Pi);
            }

            var maxHeadRotation = MathHelper.ToRadians(80f);
            var minHeadRotation = MathHelper.ToRadians(-80f);

            targetHeadRotation = MathHelper.Clamp(rotation, minHeadRotation, maxHeadRotation);
            targetBodyRotation = Player.velocity.ToRotation() + MathHelper.PiOver2;

            if (Player.velocity.LengthSquared() > 0f) {
                targetBodyRotation = Player.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else {
                targetBodyRotation = 0f;
            }
        }
        else {
            targetHeadRotation = 0f;
            targetBodyRotation = 0f;
        }

        headRotation = Utils.AngleLerp(headRotation, targetHeadRotation, 0.2f);
        bodyRotation = Utils.AngleLerp(bodyRotation, targetBodyRotation, 0.2f);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        base.ModifyDrawInfo(ref drawInfo);

        if (Main.gameMenu) {
            return;
        }

        var drawPlayer = drawInfo.drawPlayer;

        drawPlayer.headRotation = headRotation;
        drawPlayer.fullRotation = bodyRotation;
        drawPlayer.fullRotationOrigin = drawPlayer.Size / 2f;

        if (!Player.IsUnderwater() || (!Player.HeldItem.IsAir && Player.controlUseItem)) {
            return;
        }

        var swimSpeedFactor = Player.velocity.Length() * 0.25f;
        var swimArmRotation = MathF.Sin(Main.GameUpdateCount * 0.1f) * swimSpeedFactor;

        if (Player.direction == 1) {
            swimArmRotation += MathHelper.Pi;
        }

        drawPlayer.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, bodyRotation + swimArmRotation - MathHelper.PiOver2);
        drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, bodyRotation - swimArmRotation - MathHelper.PiOver2);
    }
}
