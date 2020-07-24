using System;

namespace EEMod.Autoloading
{
    [AttributeUsage(AttributeTargets.Field)]
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
    }
}