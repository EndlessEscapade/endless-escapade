using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///     Provides <see cref="Player"/> extension methods.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    ///     Checks whether the player is underwater or not.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <returns><c>true</c> if the player is underwater; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUnderwater(this Player player, bool includeSlopes = false) {
        return Collision.DrownCollision(player.position, player.width, player.height, player.gravDir, includeSlopes);
    }
}
