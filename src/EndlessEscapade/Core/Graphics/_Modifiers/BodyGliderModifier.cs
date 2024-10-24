using Terraria.DataStructures;

namespace EndlessEscapade.Core.Graphics;

public sealed class BodyGliderModifier(float smoothness) : IPlayerDrawModifier
{
    void IPlayerDrawModifier.ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        var drawPlayer = drawInfo.drawPlayer;

        var fullRotationFactor = MathF.Sin(Main.GameUpdateCount * 0.1f) * 0.1f;
        var targetFullRotation = (drawPlayer.velocity.RotatedBy(fullRotationFactor)).ToRotation() + MathHelper.PiOver2;

        drawPlayer.fullRotation = drawPlayer.fullRotation.AngleLerp(targetFullRotation, smoothness);
        drawPlayer.fullRotationOrigin = drawPlayer.Size / 2f;
    }
}
