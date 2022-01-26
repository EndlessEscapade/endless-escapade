using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Utilities;

namespace EEMod.Extensions
{
    public static class RandExtensions
    {
        public static T NextArray<T>(this UnifiedRandom rand, params T[] array) => rand.Next(array);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Next<T>(this UnifiedRandom rand, Span<T> span) => ref span[rand.Next(span.Length)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Next<T>(this UnifiedRandom rand, ReadOnlySpan<T> span) => span[rand.Next(span.Length)];
    }
}