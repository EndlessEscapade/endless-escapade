using System.Runtime.CompilerServices;
using EndlessEscapade.Core.Graphics;

namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///		Provides <see cref="SpriteBatch"/> extensions.
/// </summary>
public static class SpriteBatchExtensions
{
    /// <summary>
    ///		Captures the current state of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to capture.</param>
    /// <returns>The captured snapshot of the sprite batch.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpriteBatchSnapshot Capture(this SpriteBatch spriteBatch) {
        return new SpriteBatchSnapshot(
            spriteBatch.sortMode,
            spriteBatch.blendState,
            spriteBatch.samplerState,
            spriteBatch.depthStencilState,
            spriteBatch.rasterizerState,
            spriteBatch.spriteEffect,
            spriteBatch.transformMatrix
        );
    }

    /// <summary>
    ///		Begins a sprite batch from a snapshot.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to begin.</param>
    /// <param name="snapshot">The snapshot to begin the sprite batch with.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchSnapshot snapshot) {
        spriteBatch.Begin(
            snapshot.SpriteSortMode,
            snapshot.BlendState,
            snapshot.SamplerState,
            snapshot.DepthStencilState,
            snapshot.RasterizerState,
            snapshot.Effect,
            snapshot.TransformMatrix
        );
    }
}
