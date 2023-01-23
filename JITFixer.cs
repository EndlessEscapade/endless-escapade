using Terraria.ModLoader;
using System.Reflection;

namespace EEMod
{
    public class JITFixer : PreJITFilter
    {
        public override bool ShouldJIT(MemberInfo member) => member.Module.Assembly == typeof(EEMod).Assembly;
    }
}