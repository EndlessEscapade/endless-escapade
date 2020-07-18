using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
//private bool hasGivenDefaultValue;
//private object defaultval;
//private bool lookForPrivateConstructor;

///// <summary>
///// This overload will target the default constructor
///// </summary>
//public FieldInit() { }

//public FieldInit(bool lookPrivate) => lookForPrivateConstructor = lookPrivate;

///// <summary>
///// This overload will take the given value for initializing the field
///// </summary>
///// <param name="val">Value for initializing the field</param>
//public FieldInit(object val)
//{
//    defaultval = val;
//    hasGivenDefaultValue = true;
//}

//public virtual void ManipField(FieldInfo field)
//{
//    if (!field.IsStatic || field.IsInitOnly || field.IsLiteral)
//        return;
//    Type fieldtype = field.FieldType;

//    if (hasGivenDefaultValue)
//    {
//        if (defaultval is null || fieldtype.IsAssignableFrom(defaultval.GetType()))
//            field.SetValue(null, defaultval);
//        return;
//    }

//    Type nullUnderlyingType = Nullable.GetUnderlyingType(fieldtype);
//    if(nullUnderlyingType != null) // for nullables
//    {
//        field.SetValue(null, System.Activator.CreateInstance(nullUnderlyingType));
//        return;
//    }

//    ConstructorInfo constructor = lookForPrivateConstructor
//        ? fieldtype.GetConstructor(AutoloadingManager.FLAGS_INSTANCE, null, Type.EmptyTypes, null)
//        : fieldtype.GetConstructor(Type.EmptyTypes);
//    if (constructor != null)
//        field.SetValue(null, constructor.Invoke(null));
//}

//void IMemberManipAttribute.ManipulateMember(MemberInfo member)
//{
//    if (member is FieldInfo f)
//        ManipField(f);
//}
//if (!field.IsStatic || field.IsInitOnly || field.IsLiteral)
//    return;
//Type fieldtype = field.FieldType;
//Type underlyingNullType = Nullable.GetUnderlyingType(fieldtype);
//if(underlyingNullType != null) // if it's a nullable struct
//{
//    field.SetValue(null, System.Activator.CreateInstance(underlyingNullType)); // assign a value
//    return;
//}
//ConstructorInfo constructor = LookForPrivateConstructor
//    ? fieldtype.GetConstructor(AutoloadingManager.FLAGS_INSTANCE, null, Type.EmptyTypes, null)
//    : fieldtype.GetConstructor(Type.EmptyTypes);
//if(constructor != null)
//{
//    field.SetValue(null, constructor.Invoke(null));
//}
