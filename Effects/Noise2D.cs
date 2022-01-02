using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Effects
{
    public class Noise2D : ScreenShaderEffect
    {
        public static string EffectPath { [MethodImpl(MethodImplOptions.NoInlining)] get => "Effects/Noise2D"; }

        protected EffectParameter noiseTextureParam;

        protected Ref<Texture2D> _noiseTexture = new Ref<Texture2D>();

        public Texture2D NoiseTexture { get => _noiseTexture.Value; set => _noiseTexture.Value = value; }

        public Noise2D() : base(EEMod.Instance.Assets.Request<Effect>(EffectPath).Value) { }
        public Noise2D(Effect cloneSource) : base(cloneSource) { }
        public Noise2D(Noise2D cloneSource) : base(cloneSource) { }

        public override void ApplyParameters()
        {
            base.ApplyParameters();
            noiseTextureParam?.SetValue(_noiseTexture.Value);
        }

        public override void CacheParams()
        {
            base.CacheParams();
            noiseTextureParam = Parameters["noiseTexture"];
        }

        static Noise2D()
        {
            EEEffects.AddEffectFactory(Factory);
        }
        private static Noise2D Factory() => new Noise2D();
    }
}
