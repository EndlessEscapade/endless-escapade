using EndlessEscapade.Core.Graphics;
using Terraria.DataStructures;

namespace EndlessEscapade.Common.Movement;

/// <summary>
///     Handles the player's movement and visual effects while using a glider.
/// </summary>
public sealed class GliderPlayer : ModPlayer
{
    /// <summary>
    ///     Whether the player is currently using a glider or not.
    /// </summary>
    public bool Enabled { get; set; }

    private float fullRotation;
    private float headRotation;

    public override void ResetEffects() {
        base.ResetEffects();

        Enabled = false;
    }

    public override void PostUpdate() {
        base.PostUpdate();

        if (!Enabled) {
            return;
        }

        Player.noFallDmg = true;

        if (Player.velocity.Y <= 0f) {
            return;
        }

        Player.velocity.Y = 1f;
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        base.ModifyDrawInfo(ref drawInfo);

        if (!Enabled) {
            return;
        }

        var drawPlayer = drawInfo.drawPlayer;

        if (!drawPlayer.TryGetModPlayer(out DrawInfoPlayer drawInfoPlayer)) {
            return;
        }

        drawInfoPlayer.AddModifier(new ArmsGliderModifier());
        drawInfoPlayer.AddModifier(new HeadGliderModifier());
        drawInfoPlayer.AddModifier(new BodyGliderModifier(0.2f));
    }
}
