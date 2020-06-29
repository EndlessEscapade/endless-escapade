using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EEMod.Autoloading.Attributes;
using EEMod.Extensions;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Sets a static and nullable field to null
    /// </summary>
    public class FieldUnloadAttribute : FieldConditional
    {
        public override bool IsValidField(FieldInfo field) => base.IsValidField(field) && !field.FieldType.IsStruct(); // not structs since they're not nullable
        public override void ManipField(FieldInfo field, object o)
        {
            field.SetValue(null, null);
        }
    }
}
