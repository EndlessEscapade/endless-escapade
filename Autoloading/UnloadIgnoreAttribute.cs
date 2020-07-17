using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Static fields with this attribute will <strong>NOT</strong> be set to null during unload
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class UnloadIgnoreAttribute : Attribute { }
}
