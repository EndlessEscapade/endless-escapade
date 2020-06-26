using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading
{
    public interface IConditionalAttribute
    {
    }
    public interface IMemberConditionalAttribute : IConditionalAttribute
    {
        MemberTypes HandlingMembers { get; }
        bool IsValid(MemberInfo member);
    }
    public interface IMemberHandler : IMemberConditionalAttribute
    {
        void HandleMember(MemberInfo member);
    }
}
