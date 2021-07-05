using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Autoloading;

namespace EEMod.Effects
{
    public static class EEEffects
    {
        public static Effect GetEEEffect(string name) => ModContent.GetInstance<EEMod>().GetEffect("Effects/" + name);

        public static void AddEffectFactory<TEffect>(Func<TEffect> factory) where TEffect : Effect
        {
            EEEffects<TEffect>.SetFactory(factory);
        }
    }
    public static class EEEffects<TEffect> where TEffect : Effect
    {
        static Func<TEffect> _factory;
        static TEffect _singleton;

        /// <summary>
        /// Shared effect singleton.<br />
        /// <strong>DO NOT DISPOSE THIS INSTANCE AS IT'S SHARED.</strong>
        /// </summary>
        public static TEffect Singleton // a single instance that can be used in a lot of places so it shouldn't be disposed
        {
            get
            {
                if (_singleton is null)
                    _singleton = _factory?.Invoke();
                return _singleton;
            }
        }

        /// <summary>
        /// Gets a new <typeparamref name="TEffect"/> instance from the factory.<br />
        /// This instance <strong>should</strong> be disposed once it's done with.
        /// </summary>
        /// <returns></returns>
        public static TEffect GetNew() => _factory?.Invoke();

        public static void SetFactory(Func<TEffect> factory) => _factory = factory;
        
        public static bool HasFactory => _factory != null;
    }
}
