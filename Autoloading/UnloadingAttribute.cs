using System;

namespace EEMod.Autoloading
{
    /// <summary>
    /// This attribute is meant to be applied to static methods and fields. <br />
    /// For methods:
    ///  <list type="bullet">
    ///     <item>Static methods with this attribute will be called during <seealso cref="EEMod.Unload"/></item>
    ///     <item>Can't be applied to methods that are abstract or generic</item>
    ///  </list>
    ///     
    /// For fields: 
    /// <list type="bullet">
    ///     <item>Static fields with this attribute will be set to null during <seealso cref="EEMod.Unload"/></item>
    ///     <item>Can't be applied to fields which type isn't nullable</item>
    ///     <item><strong>DO NOT</strong> apply to constant/readonly fields</item>
    /// </list>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class UnloadingAttribute : Attribute
    {
        public UnloadingAttribute() => Loadmode = LoadingMode.Both;
        public UnloadingAttribute(LoadingMode mode) => Loadmode = mode;
        public LoadingMode Loadmode { get; set; }
    }
}