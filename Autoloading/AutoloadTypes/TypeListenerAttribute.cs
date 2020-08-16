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
        internal LoadMode loadMode;
    }
}
