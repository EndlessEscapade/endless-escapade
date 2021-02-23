using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Types that implement this interface will have static fields <br />
    /// calling this method when they're unloaded
    /// </summary>
    interface IOnUnload
    {
        void Unloading(FieldInfo field, Type containingType);
    }
}
