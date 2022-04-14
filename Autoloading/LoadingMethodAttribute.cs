using System;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Methods with this attribute will be called during <see cref="EEMod.Load"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class LoadingMethodAttribute : Attribute
    {
        public LoadingMethodAttribute()
        {
        }
        public LoadingMethodAttribute(LoadMode loadMode)
        {
            LoadMode = loadMode;
        }
        public LoadMode LoadMode { get; set; }
    }
}