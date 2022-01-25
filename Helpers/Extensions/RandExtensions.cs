using System;
using Terraria;
using Terraria.Utilities;

namespace EEMod.Extensions
{
    public static class RandExtensions
    {
        public static T NextArray<T>(this UnifiedRandom rand, params T[] array) => rand.Next(array);

        public static ref T Next<T>(this UnifiedRandom rand, Span<T> span) => ref span[rand.Next(span.Length)];

        public static T Next<T>(this UnifiedRandom rand, ReadOnlySpan<T> span) => span[rand.Next(span.Length)];
    }
}