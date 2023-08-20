using System.Reflection;

namespace EndlessEscapade.Utilities;

public static class ReflectionUtils
{
    public const BindingFlags PrivateInstanceFlags = BindingFlags.Instance | BindingFlags.NonPublic;
    public const BindingFlags Any = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

    // Shorthands for quickly accessing a field inside an object
    // these should preferiably be used for quick debugging or code that doesnt run often, for actual use, cache the FieldInfo for speed
    public static object GetField(object obj, string name) => obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(obj);
    public static T GetField<T>(object obj, string name) => (T)obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(obj);
    public static void GetField<T>(object obj, string name, out T value) => value = (T)obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(obj);
}
