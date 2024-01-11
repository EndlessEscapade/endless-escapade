using System;
using System.Runtime.CompilerServices;
using Terraria.Utilities;

namespace EndlessEscapade.Utilities.Extensions
{
    internal static class UnifiedRandomExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Next<T>(this UnifiedRandom random, Span<T> span) => ref span[random.Next(span.Length)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Next<T>(this UnifiedRandom random, ReadOnlySpan<T> span) => ref span[random.Next(span.Length)];
    }
}
