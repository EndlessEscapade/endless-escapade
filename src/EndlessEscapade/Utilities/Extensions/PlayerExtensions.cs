using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///     Basic <see cref="Player"/> extension utilities.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    ///     Checks if the player has drowning collision.
    /// </summary>
    /// <remarks>This will not necessarily mean the player is drowning, only its collision.</remarks>
    /// <param name="player">The player to check.</param>
    /// <returns>Whether the player has drowning collision or not.</returns>
    public static bool IsDrowning(this Player player) {
        return Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
    }

    /// <summary>
    ///     Checks if the player is wearing a specified type of helmet.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="type">The type of the helmet.</param>
    /// <returns>Whether the player is wearing the specified helmet or not.</returns>
    public static bool HasHelmet(this Player player, int type) {
        return player.armor[0].type == type;
    }

    /// <summary>
    ///     Checks if the player is wearing a specified type of helmet.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <typeparam name="T">The type of the helmet.</typeparam>
    /// <returns>Whether the player is wearing the specified helmet or not.</returns>
    public static bool HasHelmet<T>(this Player player) where T : ModItem {
        return player.HasHelmet(ModContent.ItemType<T>());
    }
}
