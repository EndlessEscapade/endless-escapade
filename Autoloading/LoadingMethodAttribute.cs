using System;
using System.Reflection;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Appliable to methods for calling them during <see cref="EEMod.Load"/><br />
    /// Apply only to static methods without parameters
    /// </summary>
    public class LoadingMethodAttribute : MethodConditional
    {
        public LoadingMethodAttribute() => Loadmode = LoadingMode.Both;
        public LoadingMethodAttribute(LoadingMode mode) => Loadmode = mode;
        public LoadingMode Loadmode { get; set; }
        public override bool IsValidMethod(MethodInfo method) => LoadH.ValidCurrent(Loadmode) && base.IsValidMethod(method) && (method.GetParameters()?.Length ?? 0) == 0;
    }
}