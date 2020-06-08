using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod
{
    public class JSHelper
    {
        // CLAMPS - Used to keep variables between certain values, mainly used for situations where the game keeps crashing or entity goes wild
        public static int Clamp(int v1, int minValue, int maxValue)
        {
            if (v1 < minValue)
            {
                v1 = minValue;
            }
            if (v1 > maxValue)
            {
                v1 = maxValue;
            }
            return v1;
        }

        public static float ClampFloat(float v1, float minValue, float maxValue)
        {
            if (v1 < minValue)
            {
                v1 = minValue;
            }
            if (v1 > maxValue)
            {
                v1 = maxValue;
            }
            return v1;
        }

        public static double ClampDouble(double v1, double minValue, double maxValue)
        {
            if (v1 < minValue)
            {
                v1 = minValue;
            }
            if (v1 > maxValue)
            {
                v1 = maxValue;
            }
            return v1;
        }

        public static ulong ClampULong(ulong v1, ulong minValue, ulong maxValue)
        {
            if (v1 < minValue)
            {
                v1 = minValue;
            }
            if (v1 > maxValue)
            {
                v1 = maxValue;
            }
            return v1;
        }

        // HALF CHANCE - Basically a random boolean.
        public static int Choose(int num1, int num2)
        {
            if (Main.rand.Next(1, 5) == 3)
            {
                return num1;
            }
            return num2;
        }

        public static bool IsEvenNumber(int num1)
        {
            double num2 = (double)num1 / 2;
            if (Math.Ceiling(num2) != num1)
            {
                return false;
            }
            return true;
        }

        // RANGE - Useful for a number of things, such as getting the distance between two objects
        public static int Difference(int num1, int num2)
        {
            return (num1 - num2);
        }

        public static float GetDistance(float v2, float v3)
        {
            return (v2 - v3);
        }

        public static bool WithinRange(float v6, float v7, float v8)
        {
            return (v6 > v7 && v6 < v8);
        }

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
                        return Main.rand.Next(1, 20);
                    }
                    // Expert mode
                    return Main.rand.Next(1, 45);
                }
                // Nightmare mode
                return Main.rand.Next(1, 80);
            }
            if (flag)
            {
                if (flag2)
                {
                    // Normal mode
                    return Main.rand.Next(1, 250);
                }
                // Expert mode
                return Main.rand.Next(1, 500);
            }
            // Nightmare mode
            return Main.rand.Next(1, 1000);
        }

        public static int ReverseNegativeInt(int val)
        {
            // -50 - -50 = 0
            // 0 - -50 = 50
            // It's weird...
            return (val - val) - val;
        }

        public static float ReverseNegative(float val)
        {
            return (val - val) - val;
        }

        // MAGNET - Used for moving a target towards the player like a magnet. Examples of this are Slime God from Calamity, Duke Fishron/Eye of Cthulhu dashes and Phantasm Dragon
        public static Vector2 MoveTowardsPlayer(float speed, float currentX, float currentY, Player player, Vector2 issue, int direction)
        {
            // Speed - used for multiplication.
            float num1 = speed;

            // Player position - self explanatory.
            Vector2 vector3 = new Vector2(issue.X + (float)(direction * 20), issue.Y + 6f);

            // Player center
            float num2 = player.position.X + (float)player.width * 0.5f - vector3.X;

            // Player center - player position
            float num3 = player.Center.Y - vector3.Y;

            // Used to get the exact position of the player
            float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));

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
            float num1 = speed;
            Vector2 vector3 = new Vector2(issue.X + (float)(direction * 20), issue.Y + 6f);
            float num2 = npc.position.X + (float)npc.width * 0.5f - vector3.X;
            float num3 = npc.Center.Y - vector3.Y;
            float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
            float num5 = num1 / num4;
            num2 *= num5;
            num3 *= num5;
            currentX = (currentX * 58f + num2) / 58.8f;
            currentY = (currentY * 58f + num3) / 58.8f;
            return new Vector2(currentX, currentY);
        }

        public static Vector2 MoveTowardsProjectile(float speed, float currentX, float currentY, Projectile projectile, Vector2 issue, int direction)
        {
            float num1 = speed;
            Vector2 vector3 = new Vector2(issue.X + (float)(direction * 20), issue.Y + 6f);
            float num2 = projectile.position.X + (float)projectile.width * 0.5f - vector3.X;
            float num3 = projectile.Center.Y - vector3.Y;
            float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
            float num5 = num1 / num4;
            num2 *= num5;
            num3 *= num5;
            currentX = (currentX * 58f + num2) / 58.8f;
            currentY = (currentY * 58f + num3) / 58.8f;
            return new Vector2(currentX, currentY);
        }
    }
}
