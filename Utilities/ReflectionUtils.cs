using System.Reflection;

namespace EndlessEscapade.Utilities;

public static class ReflectionUtils
{
    public const BindingFlags PrivateInstanceFlags = BindingFlags.Instance | BindingFlags.NonPublic;
    public const BindingFlags AnyFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
}
