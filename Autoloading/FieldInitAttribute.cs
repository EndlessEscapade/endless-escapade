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
        internal bool hasGivenValue;
        internal bool lookForPrivateConstructor;
        internal object value;
        public FieldInitAttribute() { }
        public FieldInitAttribute(bool lookforprivateconstructor) => lookForPrivateConstructor = lookforprivateconstructor;
        public FieldInitAttribute(object val)
        {
            hasGivenValue = true;
            value = val;
        }
        internal LoadMode loadMode; // fields instantiating and method calling
    }
}