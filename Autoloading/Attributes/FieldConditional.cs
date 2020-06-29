using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldConditional : Attribute, IMemberHandler
    {
        public MemberTypes HandlingMembers => MemberTypes.Field;

        MemberTypes IMemberConditionalAttribute.HandlingMembers => MemberTypes.Field;

        /// <summary>
        /// By default returns that the field is static (<see cref="FieldInfo.IsStatic"/>) and not constant (<see cref="FieldInfo.IsLiteral"/>) or readonly (<see cref="FieldInfo.IsInitOnly"/>)
        /// </summary>
        /// <param name="field">The field</param>
        /// <returns></returns>
        public virtual bool IsValidField(FieldInfo field) => field.IsStatic && !(field.IsLiteral || field.IsInitOnly);

        /// <summary>
        /// Hook for doing something with the field
        /// </summary>
        /// <param name="field">The field</param>
        public virtual void ManipField(FieldInfo field, object o) { }

        void IMemberHandler.HandleMember(MemberInfo member, object o) => ManipField((FieldInfo)member, o);

        bool IMemberConditionalAttribute.IsValid(MemberInfo member) => member is FieldInfo f && IsValidField(f);
    }
}
