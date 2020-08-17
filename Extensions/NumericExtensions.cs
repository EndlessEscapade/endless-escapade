using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;

namespace EEMod.Extensions
{
    public static class NumericExtensions
    {
        public static Vector2 ForDraw(this Vector2 vec) => vec - Main.screenPosition;
    }
}
