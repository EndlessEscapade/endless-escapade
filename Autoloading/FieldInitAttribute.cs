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
    /// Initializes a static field using the default constructor
    /// </summary>
    public class FieldInitAttribute : FieldConditional
    {
        public override bool IsValidField(FieldInfo field) => base.IsValidField(field) && !field.FieldType.IsStruct() && field.FieldType.GetConstructor(AutoloadingManager.FLAGS_INSTANCE, null, Type.EmptyTypes, null) != null;
        public override void ManipField(FieldInfo field, object o)
        {
            field.SetValue(null, Activator.CreateInstance(field.FieldType)); // initialize it
        }
    }
}
