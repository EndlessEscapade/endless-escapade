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
    /// Initializes a static field if the provided object is true or the field is null using the default constructor of the type<br />
    /// And sets the field to null if the provided object is false
    /// </summary>
    public class FieldAutoInitUnloadAttribute : FieldConditional
    {
        public override bool IsValidField(FieldInfo field) => base.IsValidField(field) && !field.FieldType.IsStruct();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="o">True = Initialize<br />False = Unload (set to null)<br/>Null = Initialize if the field is null</param>
        public override void ManipField(FieldInfo field, object o)
        {
            if (o is false) // if it's unloading
            {
                field.SetValue(null, null);
            }
            else if(o is true || field.GetValue(null) is null) // if the field's value is null
            {
                var constructor = field.FieldType.GetConstructor(AutoloadingManager.FLAGS_INSTANCE, null, Type.EmptyTypes, null);
                if (constructor != null)
                    field.SetValue(null, constructor.Invoke(null));
            }            
        }
    }
}
