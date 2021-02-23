using Terraria;
using Terraria.Utilities;

namespace EEMod.Extensions
{
    public static class RandExtensions
    {
        public static T NextArray<T>(this UnifiedRandom rand, params T[] array) => rand.Next(array);
    }
}