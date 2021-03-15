using System;

namespace EEMod.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Initializes the array's elements with the default constructor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void InitializeNew<T>(this T[] t) where T : new()
        {
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = new T();
            }
        }

        /// <summary>
        /// Initializes elements that are null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void InitializeNulls<T>(this T[] t) where T : class, new()
        {
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] is null)
                {
                    t[i] = new T();
                }
            }
        }

        public static void SetAllDefault<T>(this T[] t)
        {
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = default;
            }
        }

        public static Array CloneAndCopy(this Array array)
        {
            int[] lengths = new int[array.Rank];
            for (int i = 0; i < array.Rank; i++)
                lengths[i] = array.GetLength(i);
            Array copy = Array.CreateInstance(array.GetType().GetElementType(), lengths);
            Array.Copy(array, copy, array.Length);
            return copy;
        }
    }
}