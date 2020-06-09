using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Utilities;

namespace EEMod.Extensions
{
    public static class RandExtensions
    {
        public static T NextArray<T>(this UnifiedRandom rand, params T[] array) => rand.Next(array);
    }
}
