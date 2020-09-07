using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace EEMod.Extensions
{
    public static class NumericExtensions
    {
        public static Vector2 ForDraw(this Vector2 vec) => vec - Main.screenPosition;

        public static float ToRadians(this float num) => num * (float)(Math.PI / 180f);

        public static float ToRadians(this double num) => (float)(num * (Math.PI / 180f));

        public static float ToRadians(this int num) => num * (float)(Math.PI / 180f);
    }
}