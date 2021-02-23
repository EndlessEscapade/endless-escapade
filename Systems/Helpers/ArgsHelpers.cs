namespace EEMod
{
    partial class Helpers
    {
        public static bool GetArgsOrDefault<T>(object[] args, out T val) => TryGetFromArray(args, 0, out val);

        public static bool GetArgsOrDefault<A, B>(object[] args, out A val, out B val2)
        {
            val2 = default;
            return TryGetFromArray(args, 0, out val) && TryGetFromArray(args, 1, out val2);
        }

        public static bool GetArgsOrDefault<A, B, C>(object[] args, out A val, out B val2, out C val3)
        {
            val2 = default; val3 = default;
            return TryGetFromArray(args, 0, out val) && TryGetFromArray(args, 1, out val2) && TryGetFromArray(args, 2, out val3);
        }

        public static bool GetArgsOrDefault<A, B, C, D>(object[] args, out A val, out B val2, out C val3, out D val4)
        {
            val2 = default; val3 = default; val4 = default;
            return TryGetFromArray(args, 0, out val) && TryGetFromArray(args, 1, out val2) && TryGetFromArray(args, 2, out val3) && TryGetFromArray(args, 3, out val4);
        }

        public static bool GetArgsOrDefault<A, B, C, D, E>(object[] args, out A val, out B val2, out C val3, out D val4, out E val5)
        {
            val2 = default; val3 = default; val4 = default; val5 = default;
            return TryGetFromArray(args, 0, out val) && TryGetFromArray(args, 1, out val2) && TryGetFromArray(args, 2, out val3) && TryGetFromArray(args, 3, out val4) && TryGetFromArray(args, 4, out val5);
        }

        public static bool TryGetFromArray(object[] array, int index, out object result)
        {
            if (index >= 0 && index < array.Length)
            {
                result = array[index];
                return true;
            }
            result = null;
            return false;
        }

        public static bool TryGetFromArray<T>(object[] array, int index, out T result)
        {
            if (index >= 0 && index < array.Length)
            {
                if (array[index] is T t)
                {
                    result = t;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}