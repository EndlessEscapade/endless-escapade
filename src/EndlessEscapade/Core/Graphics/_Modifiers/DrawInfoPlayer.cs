using System.Collections.Generic;
using Terraria.DataStructures;

namespace EndlessEscapade.Core.Graphics;

/// <summary>
///     <para>
///         Provides a centralized rendering pipeline that allows for simultaneous visual effects across
///         different <see cref="ModPlayer"/> implementations utilizing <see cref="ModifyDrawInfo"/>.
///     </para>
///     <para>
///         Modifiers are registered through <see cref="AddModifier{T}"/> and will be cleared afterwards,
///         meaning that modifiers must be added for every render update.
///     </para>
/// </summary>
public sealed class DrawInfoPlayer : ModPlayer
{
    private readonly Dictionary<Type, IPlayerDrawModifier> modifiers = [];

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
        base.ModifyDrawInfo(ref drawInfo);

        if (modifiers.Count > 0) {
            foreach (var (_, modifier) in modifiers) {
                modifier.ModifyDrawInfo(ref drawInfo);
            }

            modifiers.Clear();
        }
        else {
            var drawPlayer = drawInfo.drawPlayer;

            drawPlayer.fullRotation = 0f;
            drawPlayer.fullRotationOrigin = default;
        }
    }

    /// <summary>
    ///     Adds a modifier to the list of modifiers that will modify the player's <see cref="PlayerDrawSet"/>
    ///     before drawing begins.
    /// </summary>
    /// <param name="value">The instance of the modifier to be added.</param>
    /// <typeparam name="T">The type of the modifier, which must implement <see cref="IPlayerDrawModifier"/>.</typeparam>
    public void AddModifier<T>(T value) where T : IPlayerDrawModifier {
        modifiers[typeof(T)] = value;
    }
}
