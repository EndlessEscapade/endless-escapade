using System;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Apply this to fields for instantiating them during <see cref="EEMod.Load"/><br />
    /// Can also be applied to methods (it will call them during field initializing)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    internal class FieldInitAttribute : Attribute
    {
        public FieldInitAttribute()
        {
        }
        public LoadMode LoadMode { get; set; }
    }
}