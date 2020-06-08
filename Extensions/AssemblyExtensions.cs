using System;
using System.Reflection;

namespace EEMod.Extensions
{
    public static class AssemblyExtensions
    {
        public static Type[] GetTypesSafe(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types;
            }
        }
    }
}