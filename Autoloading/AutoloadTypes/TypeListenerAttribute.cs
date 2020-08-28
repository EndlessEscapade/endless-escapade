using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading.AutoloadTypes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TypeListenerAttribute : Attribute
    {
        public TypeListenerAttribute() { }
        internal TypeListenerAttribute(LoadMode mode) => loadMode = mode;
        internal LoadMode loadMode;
    }
}
