using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Effects
{
    public abstract class EEEffect : Effect
    {
        /// <summary>
        /// If true, the effect won't apply the parameter's values on <see cref="Effect.OnApply"/><br />
        /// you will have to call <see cref="ApplyParameters"/> manually.<br />
        /// Useful if the effect overrides changes to the parameters before being applied.
        /// </summary>
        public bool SupressAutomaticApply { get; set; }

        protected EEEffect(Effect cloneSource) : base(cloneSource)
        {
            CacheParams();
        }

        protected EEEffect(GraphicsDevice graphicsDevice, byte[] effectCode) : base(graphicsDevice, effectCode)
        {
            CacheParams();
        }

        /// <summary>
        /// Allows for cacheing the effect parameters.
        /// </summary>
        public virtual void CacheParams()
        {

        }

        /// <summary>
        /// Apply the effect's parameters values to the effect, however don't modify the GraphicsDevice<br /> 
        /// there's <see cref="ApplyGraphicsDeviceEffects"/> for that.
        /// </summary>
        public virtual void ApplyParameters()
        {

        }

        /// <summary>
        /// When the effect has parameters that need to be set on the GraphicsDevice<br />
        /// e.g. textures or sampler states.
        /// </summary>
        public virtual void ApplyGraphicsDeviceEffects()
        {

        }

        protected override void OnApply()
        {
            if (!SupressAutomaticApply)
                ApplyParameters();
            base.OnApply();
        }
    }
}
