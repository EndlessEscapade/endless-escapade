using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace EEMod
{
    public static partial class Helpers
    {
        // CLAMPS - Used to keep variables between certain values, mainly used for situations where the game keeps crashing or entity goes wild
        public static byte Clamp(byte value, byte minValue, byte maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;

        public static sbyte Clamp(sbyte value, sbyte minValue, sbyte maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;

        public static short Clamp(short value, short minValue, short maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;

        public static ushort Clamp(ushort value, ushort minValue, ushort maxValue) => value < minValue ? minValue : value > maxValue ? maxValue : value;

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

        public static void Clamp(ref byte value, byte min, byte max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref sbyte value, sbyte min, sbyte max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref short value, short min, short max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref ushort value, ushort min, ushort max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref uint value, uint min, uint max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref long value, long min, long max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref ulong value, ulong min, ulong max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref float value, float min, float max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref double value, double min, double max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp(ref decimal value, decimal min, decimal max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public static void Clamp<T>(ref T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                value = min;
            }
            else if (value.CompareTo(max) > 0)
            {
                value = max;
            }
        }

        public static float FloatLerp(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                        return 0f;
                    if (t > to)
                        return 1f;
                }
                else
                {
                    if (t < to)
                        return 1f;
                    if (t > from)
                        return 0f;
                }
            }
            return (t - from) / (to - from);
        }

        /// <summary>
        /// The opposite of <see cref="MathHelper.Lerp(float, float, float)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double InverseLerp(double value, double min, double max) => (value - min) / (max - min);

        public static double LerpByInverseLerp(double value1, double value2, double ammountval, double min, double max) => value1 + value2 * ((ammountval - min) / (max - min));


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

        public static bool PointInRectangle(Vector2 point, Vector4 rectangle) => PointInRectangle(point.X, point.Y, rectangle.X, rectangle.Y, rectangle.W, rectangle.Z);

        public static bool PointInRectangle(float pointX, float pointY, Vector4 rectangle) => PointInRectangle(pointX, pointY, rectangle.X, rectangle.Y, rectangle.W, rectangle.Z);

        public static bool PointInRectangle(Vector2 point, Vector2 pos, Vector2 size) => PointInRectangle(point.X, point.Y, pos.X, pos.Y, size.X, size.Y);

        public static bool PointInRectangle(Vector2 point, float x, float y, Vector2 size) => PointInRectangle(point.X, point.Y, x, y, size.X, size.Y);

        public static bool PointInRectangle(Vector2 point, Vector2 pos, float width, float height) => PointInRectangle(point.X, point.Y, pos.X, pos.Y, width, height);

        public static bool PointInRectangle(Vector2 point, float x, float y, float width, float height) => PointInRectangle(point.X, point.Y, x, y, width, height);

        public static bool PointInRectangle(float pointX, float pointY, Vector2 pos, Vector2 size) => PointInRectangle(pointX, pointY, pos.X, pos.Y, size.X, size.Y);

        public static bool PointInRectangle(float pointX, float pointY, float rectangleX, float rectangleY, Vector2 size) => PointInRectangle(pointX, pointY, rectangleX, rectangleY, size.X, size.Y);

        public static bool PointInRectangle(float pointX, float pointY, Vector2 rectanglepos, float width, float height) => PointInRectangle(pointX, pointY, rectanglepos.X, rectanglepos.Y, width, height);

        public static bool PointInRectangle(float pointX, float pointY, float rectangleX, float rectangleY, float width, float height) =>
            pointY >= rectangleY &&
            pointX >= rectangleX && pointX <= rectangleX + width &&
            pointY <= rectangleY + height;

        public static Vector2 DirectionTowardsClampLength(Vector2 from, Vector2 to, double? min = null, double? max = null)
        {
            double val2;
            double x = from.X - to.X;
            double y = from.Y - to.Y;
            double lengthSQ;
            if (min != null)
            {
                val2 = (double)min;
                if ((lengthSQ = x * x + y * y) < val2 * val2)
                {
                    val2 /= Math.Sqrt(lengthSQ);
                    x *= val2;
                    y *= val2;
                }
            }
            if (max != null)
            {
                val2 = (double)max;
                if ((lengthSQ = x * x + y * y) > val2 * val2)
                {
                    val2 /= Math.Sqrt(lengthSQ);
                    x *= val2;
                    y *= val2;
                }
            }
            return new Vector2((float)x, (float)y);
        }

        public static Vector2 DirectionTowards(Vector2 from, Vector2 to, double length = 1)
        {
            double x = from.X - to.X;
            double y = from.Y - to.Y;
            double val = length / Math.Sqrt(x * x + y * y); // Normalization consists on vector = vector * (newlength / currentlength)
            return new Vector2((float)(val * x), (float)(val * y));
        }

        public static Vector2 ClampLength(Vector2 vector, double? min = null, double? max = null)
        {
            double lengthSQ = vector.X * vector.X + vector.Y * vector.Y;
            double val;
            if (min != null)
            {
                val = (double)min;
                if (lengthSQ < val * val)
                {
                    val /= Math.Sqrt(lengthSQ);
                    return new Vector2((float)(vector.X * val), (float)(vector.Y * val));
                }
            }
            if (max != null)
            {
                val = (double)max;
                if (lengthSQ > val * val)
                {
                    val /= Math.Sqrt(lengthSQ);
                    return new Vector2((float)(vector.X * val), (float)(vector.Y * val));
                }
            }
            return vector;
        }

        public static Vector2 ChangeLength(Vector2 vector, double newlength = 1)
        {
            double val = newlength / Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return new Vector2((float)(vector.X * val), (float)(vector.Y * val));
        }

        public static Vector2 BezierCurve(float ammount, Vector2 a, Vector2 b, Vector2 c) => Vector2.Lerp(value1: Vector2.Lerp(a, b, ammount), value2: Vector2.Lerp(b, c, ammount), ammount);

        public static Vector2 BezierCurve(float ammount, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            Vector2 q1 = Vector2.Lerp(a, b, ammount);
            Vector2 q2 = Vector2.Lerp(b, c, ammount);
            Vector2 q3 = Vector2.Lerp(c, d, ammount);
            return Vector2.Lerp(value1: Vector2.Lerp(q1, q2, ammount), value2: Vector2.Lerp(q2, q3, ammount), ammount);
        }

        public static Vector2 BezierCurve(float ammount, params Vector2[] points)
        {
            if (points is null)
                return new Vector2();

            switch (points.Length)
            {
                case 0: return new Vector2();
                case 1: return points[0];
                case 2: return Vector2.Lerp(points[0], points[1], ammount);
                case 3: return BezierCurve(ammount, points[0], points[1], points[2]);
                case 4: return BezierCurve(ammount, points[0], points[1], points[2], points[3]);
            }

            Vector2[] copy = new Vector2[points.Length - 1]; // another array so it's not needed to use recursion (smaller capacity because it will copy the first interpolations instead of the values so it's not needed to copy + interpolate)
            /*
             * a ,b ,c , d 
             * would lerp a to b, b to c and c to d and save them in the previous position
             * because a will only be used once, it's safe to store the result on it while b and c are used twice
             * so a = lerp(a,b), a isn't used after this so the result can be saved in a
             * then b = lerp(b,c), b won't be used anymore after this so the result can be saved in b
             * d won't be used after first time either, so it'll just be there
             * q1,q2,q3, d
             * ...
             * in the end, the first element in the array will be the result of all interpolation
            */

            int i;

            for(i = 0; i < points.Length - 1; i++)
                copy[i] = Vector2.Lerp(points[i], points[i + 1], ammount);

            int n = copy.Length;

            while (n --> 0)
            {
                for (i = 0; i < n - 1; i++)
                    copy[i] = Vector2.Lerp(copy[i], copy[i + 1], ammount);
                // n: 4
                // i: 0, 1, 2
                // [0] = lerp([0], [1]) or a = lerp(a, b)
                // [1] = lerp([1], [2]) or b = lerp(b, c)
                // [2] = lerp([2], [3]) or c = lerp(c, d)
                // next one, n: 3
                // i: 0, 1 
                // [0] = lerp([0], [1])
                // [1] = lerp([1], [2])
                // next one, n: 2
                // i: 0
                // [0] = lerp([0], [1])
                
            }

            return copy[0];
            
        }

        public static IEnumerable<NPC> NPCForeach
        {
            get
            {
                for (int i = 0; i < Main.npc.Length - 1; i++)
                {
                    yield return Main.npc[i];
                }

                yield break;
            }
        }

        public static IEnumerable<Projectile> ProjectileForeach
        {
            get
            {
                for (int i = 0; i < Main.projectile.Length - 1; i++)
                {
                    yield return Main.projectile[i];
                }

                yield break;
            }
        }

        /// <summary>
        /// Shortcut for <code>Lighting.GetColor(worldCoords.X / 16, worldCoords.Y / 16)</code>.
        /// </summary>
        /// <param name="worldCoords"></param>
        /// <returns></returns>
        public static Color GetLightingColor(Vector2 worldCoords) => Lighting.GetColor((int)(worldCoords.X / 16), (int)(worldCoords.Y / 16));

        /// <summary>
        /// Gets the closest NPC to a point, for specifying which npcs count, use a predicate for <paramref name="searchPredicate"/>
        /// </summary>
        /// <param name="position">The point of position</param>
        /// <param name="minDistance">Minimum distance, -1 meaning there's no minimum</param>
        /// <param name="searchPredicate">Predicate for spcifying which NPCs count, e.g. <code>npc => npc.active </code></param>
        /// <returns>Index of the NPC, will be -1 if not a single one is found</returns>
        public static int ClosestNPCTo(Vector2 position, float minDistance = -1, Func<NPC, bool> searchPredicate = null)
        {
            float closestDistSQ = -1;
            int npcindex = -1;
            for (int i = 0; i < Main.npc.Length - 1; i++)
            {
                NPC npc = Main.npc[i];
                if (searchPredicate?.Invoke(npc) is false)
                {
                    continue;
                }

                float distSQ = npc.DistanceSQ(position);
                /*"there's no defined smallest distance (like the first iteration) or the distance is smaller than the current smallest
                 * and there's no minimum or the distance is smaller than the minimum"*/
                if ((closestDistSQ == -1 || distSQ < closestDistSQ) && (minDistance == -1 || distSQ < minDistance * minDistance))
                {
                    npcindex = i;
                    closestDistSQ = distSQ;
                }
            }
            return npcindex;
        }

        public static Vector2 VectorFromRotation(double radians, double length = 1) => new Vector2((float)(Math.Sin(radians) * length), (float)(Math.Cos(radians) * length));

        public static float RotationTo(this Vector2 from, Vector2 to) => (float)Math.Atan2(from.Y - to.Y, from.X - to.X);

        public static int ResolveProjectileIdentity(int projectileindex, int? forclient = null) => Main.projectileIdentity[forclient ?? Main.myPlayer, projectileindex];

        // ROTATION - Used for pointing towards things. Simple.
        public static float RotateTowards(float v4, float v5)
        {
            return (float)Math.Atan2(v4, v5);
        }

        public static void Move(NPC npc, Player player, float sped, float TR, Vector2 addOn, bool flip = true, int direction = 1)
        {
            Vector2 moveTo = player.Center + addOn;
            float speed = sped;
            Vector2 move = moveTo - npc.Center;
            float magnitude = move.Length(); // (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TR;

            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            npc.velocity = move;
            if (flip)
            {
                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = 1 * direction;
                }
                else
                {
                    npc.spriteDirection = -1 * direction;
                }
            }
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

        private static double area(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return Math.Abs((p1.X * (p2.Y - p3.Y) +
                             p2.X * (p3.Y - p1.Y) +
                             p3.X * (p1.Y - p2.Y)) / 2.0);
        }

        public static void ActivateShader(string Name, Entity entity)
        {
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[Name].IsActive())
            {
                Filters.Scene.Activate(Name, entity.Center).GetShader();
            }
        }

        public static bool IsInside(Vector2 p1, Vector2 p2, Vector2 p3,
                             Vector2 p)
        {
            double A = area(p1, p2, p3);

            double A1 = area(p, p2, p3);

            double A2 = area(p1, p, p3);

            double A3 = area(p1, p2, p);

            return A == A1 + A2 + A3;
        }

        // TagCompound
        public static bool TryGetIntArray(this TagCompound tag, string key, out int[] array)
        {
            if (tag.ContainsKey(key))
            {
                array = tag.GetIntArray(key);
                return true;
            }
            array = null;
            return false;
        }

        public static bool TryGetByteArrayRef(this TagCompound tag, string key, ref byte[] array)
        {
            if (tag.ContainsKey(key))
            {
                array = tag.GetByteArray(key);
                return true;
            }
            return false;
        }

        public static bool TryGetByteArray(this TagCompound tag, string key, out byte[] array)
        {
            if (tag.ContainsKey(key))
            {
                array = tag.GetByteArray(key);
                return true;
            }
            array = null;
            return false;
        }

        public static bool TryGetListRef<T>(this TagCompound tag, string key, ref IList<T> list)
        {
            if (tag.ContainsKey(key))
            {
                list = tag.GetList<T>(key);
                return true;
            }
            return false;
        }

        public static bool TryGetList<T>(this TagCompound tag, string key, out IList<T> list)
        {
            if (tag.ContainsKey(key))
            {
                list = tag.GetList<T>(key);
                return true;
            }
            list = null;
            return false;
        }

        public static bool TryGetRef<T>(this TagCompound tag, string key, ref T value)
        {
            if (tag.ContainsKey(key))
            {
                value = tag.Get<T>(key);
                return true;
            }
            return false;
        }

        public static bool TryGet<T>(this TagCompound tag, string key, out T value)
        {
            if (tag.ContainsKey(key))
            {
                value = tag.Get<T>(key);
                return true;
            }
            value = default;
            return false;
        }

        public static Vector2 DirectionTo(this Entity entity, Vector2 pos, float length) => Vector2.Normalize(entity.position - pos) * length;

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

        /// <summary>
        /// Returns an array of the specified size with random numbers from 0 to <paramref name="size"/> without duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <returns></returns>
        public static T[] FillPseudoRandomUniform<T>(int size) where T : struct
        {
            int number;
            List<int> listNumbers = new List<int>();
            for (int i = 0; i < size; i++)
            {
                do
                {
                    number = Main.rand.Next(0, size);
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
            }
            return listNumbers.ToArray() as T[];
        }

        /// <summary>
        /// Returns an array of the specified size with random numbers from 0 to <paramref name="size"/> without duplicates
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int[] FillPseudoRandomUniform2(this UnifiedRandom rand, int size)
        {
            int[] numbers = new int[size];
            if (size < 1)
            {
                return numbers;
            }

            numbers[0] = rand.Next(size);
            for (int i = 1; i < size; i++)
            {
                int num;
                do
                {
                    num = rand.Next(size);
                }
                while (Array.IndexOf(numbers, num, 0, i) >= 0);

                numbers[i] = num;
            }
            return numbers;
        }

        public static int[] FillUniformArray(int size, int min, int max)
        {
            int[] numbers = new int[size];

            int k = min;
            for(int i = 0; i < size; i++)
            {
                if (k >= max) k = min;

                numbers[i] = k;

                k++;
            }

            return numbers;
        }

        public static int[] FillPseudoRandomUniform2(int size) => FillPseudoRandomUniform2(Main.rand, size);

        public static Color ColorSmoothStep(Color value1, Color value2, float ammount) => Color.Lerp(value1, value2, ammount * ammount * (3f - 2f * ammount));

        public static double SmoothStepValueAsLerp(double val) => val * val * (3f - 2f * val);

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

        public static Color MultiLerpColor(float percent, params Color[] colors)
        {
            float per = 1f / ((float)colors.Length - 1);
            float total = per;
            int currentID = 0;
            while ((percent / total) > 1f && (currentID < colors.Length - 2))
            {
                total += per; currentID++;
            }
            return Color.Lerp(colors[currentID], colors[currentID + 1], (percent - (per * currentID)) / per);
        }

        public static Vector2 MultiLerpVector(float percent, params Vector2[] vectors)
        {
            float per = 1f / ((float)vectors.Length - 1);
            float total = per;
            int currentID = 0;
            while ((percent / total) > 1f && (currentID < vectors.Length - 2)) { total += per; currentID++; }
            return Vector2.Lerp(vectors[currentID], vectors[currentID + 1], (percent - (per * currentID)) / per);
        }
    }
}