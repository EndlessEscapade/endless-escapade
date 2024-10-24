using Terraria.DataStructures;

namespace EndlessEscapade.Core.Graphics;

public sealed class ArmsGliderModifier : IPlayerDrawModifier
{
    void IPlayerDrawModifier.ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        var drawPlayer = drawInfo.drawPlayer;

        var targetHeadRotation = drawPlayer.AngleTo(Main.MouseWorld);

        var minArmRotation = MathHelper.ToRadians(-30f);
        var maxArmRotation = MathHelper.ToRadians(30f);

        if (drawPlayer.direction == -1) {
            targetHeadRotation += MathHelper.Pi;
        }

        var armRotation = MathHelper.Clamp(targetHeadRotation, minArmRotation, maxArmRotation) + MathHelper.Pi;

        drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotation);
        drawPlayer.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armRotation);
    }
}
