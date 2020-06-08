using System;

namespace EEMod.Autoloading
{
    /// <summary>
    /// This attribute is meant to be applied to static methods, those with the attribute will be called during <seealso cref="InteritosMod.Load"/> <br />
    /// Can't be applied to methods that are be abstract or generic.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LoadingAttribute : Attribute
    {
        public LoadingAttribute() => Loadmode = LoadingMode.Both;
        public LoadingAttribute(LoadingMode mode) => Loadmode = mode;
        public LoadingMode Loadmode { get; set; }
    }
}