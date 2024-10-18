using Terraria.DataStructures;

namespace EndlessEscapade.Common.Movement;

public sealed partial class SwimmingPlayer : ModPlayer
{
    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        base.ModifyDrawInfo(ref drawInfo);

        if (Main.gameMenu) {
            return;
        }

        var drawPlayer = drawInfo.drawPlayer;

        drawPlayer.headRotation = headRotation;
        drawPlayer.fullRotation = bodyRotation;
        drawPlayer.fullRotationOrigin = drawPlayer.Size / 2f;

        var swimSpeedFactor = Player.velocity.Length();
        var swimArmRotation = MathF.Sin(Main.GameUpdateCount * 0.1f) * swimSpeedFactor;

        if (Player.direction == 1) {
            swimArmRotation += MathHelper.Pi;
        }

        drawPlayer.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, bodyRotation + swimArmRotation - MathHelper.PiOver2);
        drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, bodyRotation - swimArmRotation - MathHelper.PiOver2);
    }
}
