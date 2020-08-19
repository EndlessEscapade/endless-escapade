using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Autoloading.AutoloadTypes
{
    /// <summary>
    /// Do not inherit from this class, use <seealso cref="AutoloadTypeManager{T}"/> instead
    /// </summary>
    public abstract class AutoloadTypeManager
    {
        public static T GetManager<T>() where T : AutoloadTypeManager => ModContent.GetInstance<T>();

        protected abstract void EvaluateType(Type type);

        public virtual void Initialize()
        {
        }

        // cus it's protected
        internal static void Evaluate(AutoloadTypeManager instance, Type type) => instance.EvaluateType(type);
    }

    /// <summary>
    /// Manages instances of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AutoloadTypeManager<T> : AutoloadTypeManager where T : IAutoloadType
    {
        /// <summary>
        /// Use this for creating instances and other stuff, like subscribing methods to events, adding intances to lists, etc.
        /// </summary>
        /// <param name="type">The type of the class</param>
        public virtual void CreateInstance(Type type)
        {
        }

        protected sealed override void EvaluateType(Type type)
        {
            if(type.IsSubclassOf(typeof(T)))
            CreateInstance(type);
        }
    }
}
