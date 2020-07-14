using System;
using System.Collections.Generic;
using Terraria;
using Microsoft.Xna.Framework;

namespace EEMod
{
    public static partial class Helpers
    {
        // CLAMPS - Used to keep variables between certain values, mainly used for situations where the game keeps crashing or entity goes wild
        public static byte Clamp(byte value, byte minValue, byte maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static sbyte Clamp(sbyte value, sbyte minValue, sbyte maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static int Clamp(int value, int minValue, int maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static uint Clamp(uint value, uint minValue, uint maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static long Clamp(long value, long minValue, long maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static ulong Clamp(ulong value, ulong minValue, ulong maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static float Clamp(float value, float minValue, float maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static double Clamp(double value, double minValue, double maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static decimal Clamp(decimal value, decimal minValue, decimal maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T> =>
            value.CompareTo(min) < 0 ? min : // CompareTo would return a negative if the argument is bigger
            value.CompareTo(max) > 0 ? max : // CompareTo would return a positive if the argument is smaller
            value; // When CompareTo returns 0
        public static T Clamp<T>(T value, T min, T max, IComparer<T> comparer) =>
            comparer.Compare(value, min) < 0 ? min :
            comparer.Compare(value, max) > 0 ? max :
            value;
        public static void Clamp(ref byte value, byte min, byte max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref sbyte value, sbyte min, sbyte max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref int value, int min, int max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref uint value, uint min, uint max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref long value, long min, long max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref ulong value, ulong min, ulong max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref float value, float min, float max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref double value, double min, double max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp(ref decimal value, decimal min, decimal max) { if (value < min) value = min; else if (value > max) value = max; }
        public static void Clamp<T>(ref T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                value = min;
            else if (value.CompareTo(max) > 0)
                value = max;
        }


        // HALF CHANCE - Basically a coin flip.
        public static T ChooseRandom<T>(T obj1, T obj2) => Main.rand.NextBool(2) ? obj1 : obj2;


        public static bool IsEvenNumber(int num1) => num1 % 2 == 0;


        // RANGE - Useful for a number of things, such as getting the distance between two objects
        public static bool InRange(byte value, byte min, byte max) => value > min && value < max;
        public static bool InRange(sbyte value, sbyte min, sbyte max) => value > min && value < max;
        public static bool InRange(int value, int min, int max) => value > min && value < max;
        public static bool InRange(uint value, uint min, uint max) => value > min && value < max;
        public static bool InRange(long value, long min, long max) => value > min && value < max;
        public static bool InRange(ulong value, ulong min, ulong max) => value > min && value < max;
        public static bool InRange(float value, float min, float max) => value > min && value < max;
        public static bool InRange(double value, double min, double max) => value > min && value < max;
        public static bool InRange(decimal value, decimal min, decimal max) => value > min && value < max;
        public static bool InRange<T>(IComparable<T> value, T min, T max) where T : IComparable<T> => value.CompareTo(min) > 0 && value.CompareTo(max) < 0;
        public static bool InRange<T>(T value, T min, T max, IComparer<T> comparer) => comparer.Compare(value, min) > 0 && comparer.Compare(value, max) < 0;

        public static bool VectorInRange(Vector2 from, Vector2 to, float MaxRange) => Vector2.DistanceSquared(from, to) <= MaxRange * MaxRange;

        // ROTATION - Used for pointing towards things. Simple.
        public static float RotateTowards(float v4, float v5)
        {
            return (float)Math.Atan2(v4, v5);
        }

        public static int ShiftChance(bool boss, bool flag, bool flag2)
        {
            if (boss)
            {
                if (flag)
                {
                    if (flag2)
                    {
                        // Normal mode
                        return Main.rand.Next(1, 60);
                    }
                    // Expert mode
                    return Main.rand.Next(1, 55);
                }
                // Nightmare mode
                return Main.rand.Next(1, 50);
            }
            if (flag)
            {
                if (flag2)
                {
                    // Normal mode
                    return Main.rand.Next(1, 4000);
                }
                // Expert mode
                return Main.rand.Next(1, 1000);
            }
            // Genkai(?) mode
            return Main.rand.Next(1, 500);
        }

        //public static int ReverseNegativeInt(int val)
        //{
        //	// -50 - -50 = 0
        //	// 0 - -50 = 50
        //	// It's weird...
        //	return val - val - val;
        //}

        //public static float ReverseNegative(float val)
        //{
        //	return val - val - val;
        //}

        // MAGNET - Used for moving a target towards the player like a magnet. Examples of this are Slime God from Calamity, Duke Fishron/Eye of Cthulhu dashes and Phantasm Dragon
        public static Vector2 MoveTowardsPlayer(float speed, float currentX, float currentY, Player player, Vector2 issue, int direction)
        {
            // Speed - used for multiplication.
            float num1 = speed;

            // Player position - self explanatory.
            Vector2 vector3 = new Vector2(issue.X + (direction * 20), issue.Y + 6f);

            // Player center
            float num2 = player.position.X + player.width * 0.5f - vector3.X;

            // Player center - player position
            float num3 = player.Center.Y - vector3.Y;

            // Used to get the exact position of the player
            float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);

            // The multiplication I mentioned earlier
            float num5 = num1 / num4;
            num2 *= num5;
            num3 *= num5;

            // Speed arithmetic, possibly pointless
            currentX = (currentX * 58f + num2) / 58.8f;
            currentY = (currentY * 58f + num3) / 58.8f;

            // The final result
            return new Vector2(currentX, currentY);
        }

        public static Vector2 MoveTowardsNPC(float speed, float currentX, float currentY, NPC npc, Vector2 issue, int direction)
        {
            Vector2 vector3 = new Vector2(issue.X + (direction * 20), issue.Y + 6f);
            float num2 = npc.position.X + npc.width * 0.5f - vector3.X;
            float num3 = npc.Center.Y - vector3.Y;
            float length = (float)Math.Sqrt(num2 * num2 + num3 * num3);
            float num5 = speed / length;
            num2 *= num5;
            num3 *= num5;
            currentX = (currentX * 58f + num2) / 58.8f;
            currentY = (currentY * 58f + num3) / 58.8f;
            return new Vector2(currentX, currentY);
        }

        public static Vector2 MoveTowardsProjectile(float speed, float currentX, float currentY, Projectile projectile, Vector2 issue, int direction)
        {
            Vector2 vector3 = new Vector2(issue.X + (direction * 20), issue.Y + 6f);
            float num2 = projectile.position.X + projectile.width * 0.5f - vector3.X;
            float num3 = projectile.Center.Y - vector3.Y;
            float length = (float)Math.Sqrt(num2 * num2 + num3 * num3);
            float num5 = speed / length;
            num2 *= num5;
            num3 *= num5;
            currentX = (currentX * 58f + num2) / 58.8f;
            currentY = (currentY * 58f + num3) / 58.8f;
            return new Vector2(currentX, currentY);
        }
    }
}