using Terraria.DataStructures;

namespace EndlessEscapade.Core.Graphics;

public sealed class HeadGliderModifier : IPlayerDrawModifier
{
    void IPlayerDrawModifier.ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        var drawPlayer = drawInfo.drawPlayer;

        var targetHeadRotation = drawPlayer.AngleTo(Main.MouseWorld);

        var minHeadRotation = MathHelper.ToRadians(-40f);
        var maxHeadRotation = MathHelper.ToRadians(40f);

        if (drawPlayer.direction == -1) {
            targetHeadRotation += MathHelper.Pi;
        }

        targetHeadRotation = MathHelper.Clamp(targetHeadRotation, minHeadRotation, maxHeadRotation);

        drawPlayer.headRotation = targetHeadRotation;
    }
}
