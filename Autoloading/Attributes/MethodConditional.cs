using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodConditional : Attribute, IMemberHandler
    {
        public MemberTypes HandlingMembers => MemberTypes.Method;

        /// <summary>
        /// By default returns that the method is static (<see cref="MethodBase.IsStatic"/>) and not abstract (<see cref="MethodBase.IsAbstract"/>) or generic (<see cref="MethodBase.IsGenericMethod"/>,<see cref="MethodBase.IsGenericMethodDefinition"/>) or the method body (<see cref="MethodBase.GetMethodBody"/>) byte array (<see cref="MethodBody.GetILAsByteArray"/>) is null
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns></returns>
        public virtual bool IsValidMethod(MethodInfo method) => method.IsStatic && !(method.IsAbstract || method.IsGenericMethod || method.IsGenericMethodDefinition || method.GetMethodBody()?.GetILAsByteArray() is null);

        /// <summary>
        /// How the method that has the attribute is invoked
        /// </summary>
        /// <param name="method">The method</param>
        public virtual void Invoke(MethodInfo method, object o) => method.Invoke(null, null);

        bool IMemberConditionalAttribute.IsValid(MemberInfo member) => member is MethodInfo m && IsValidMethod(m);

        void IMemberHandler.HandleMember(MemberInfo member, object o) => Invoke((MethodInfo)member, o);
    }
}
