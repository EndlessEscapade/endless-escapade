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
    public class ScreenShaderEffect : EEEffect
    {
        protected EffectParameter uColorParam;
        protected EffectParameter uSecondaryColorParam;
        protected EffectParameter uScreenResolutionParam;
        protected EffectParameter uScreenPositionParam;
        protected EffectParameter uTargetPositionParam;
        protected EffectParameter uDirectionParam;
        protected EffectParameter uOpacityParam;
        protected EffectParameter uTimeParam;
        protected EffectParameter uIntensityParam;
        protected EffectParameter uProgressParam;
        protected EffectParameter[] uImageSizeParams;
        //protected EffectParameter uImageSize1;
        //EffectParameter uImageSize2Param;
        //EffectParameter uImageSize3Param;
        protected EffectParameter uImageOffsetParam;
        protected EffectParameter uSaturationParam;
        protected EffectParameter uSourceRectParam;
        protected EffectParameter uZoomParam;

        protected Vector3 _uColor;
        protected float _uOpacity;
        protected Vector3 _uSecondaryColor;
        protected Vector2 _uTargetPosition;
        protected Vector2 _uImageOffset;
        protected float _uIntensity;
        protected float _uProgress;
        protected Vector2 _uDirection;
        protected Ref<Texture2D>[] _uTextures = new Ref<Texture2D>[3];
        protected SamplerState[] _samplerStates = new SamplerState[3];
        protected float[] _textureScales = new float[3];

        public Color uColor           { get => new Color(_uColor);          set => _uColor = value.ToVector3(); }
        public Vector3 uColorVector3  { get => _uColor;                     set => _uColor = value; }
        public Color SecondaryColor   { get => new Color(_uSecondaryColor); set => _uSecondaryColor = value.ToVector3(); }
        public Vector3 SecondaryColorVector3 { get => _uSecondaryColor;     set => _uSecondaryColor = value; }
        public float CombinedOpacity  { get => _uOpacity;                   set => _uOpacity = value; }
        public Vector2 TargetPosition { get => _uTargetPosition;            set => _uTargetPosition = value; }
        public Vector2 ImageOffset    { get => _uImageOffset;               set => _uImageOffset = value; }
        public Vector2 Direction      { get => _uDirection;                 set => _uDirection = value; }

        public ScreenShaderEffect(Effect cloneSource) : base(cloneSource)
        {

        }
        public ScreenShaderEffect(GraphicsDevice graphicsDevice, byte[] effectCode) : base(graphicsDevice, effectCode)
        {

        }

        public override void ApplyParameters()
        {
            Vector2 offScreenRange = new Vector2(Main.offScreenRange, Main.offScreenRange);
            Vector2 screenResolution = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
            Vector2 screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
            Vector2 screenPosition = Main.screenPosition + screenCenter * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);

            uColorParam?.SetValue(this._uColor); // optional params
            uOpacityParam?.SetValue(this.CombinedOpacity);
            uSecondaryColorParam?.SetValue(this._uSecondaryColor);
            uTargetPositionParam?.SetValue(this._uTargetPosition - offScreenRange);
            uImageOffsetParam?.SetValue(this._uImageOffset);
            uIntensityParam?.SetValue(this._uIntensity);
            uProgressParam?.SetValue(this._uProgress);
            uDirectionParam?.SetValue(this._uDirection);

            uTimeParam?.SetValue(Main.GlobalTime); // params with default implementation
            uScreenResolutionParam?.SetValue(screenResolution);
            uScreenPositionParam?.SetValue(screenPosition - offScreenRange);
            uZoomParam?.SetValue(Main.GameViewMatrix.Zoom);
        }

        public override void ApplyGraphicsDeviceEffects()
        {
            for (int i = 0; i < _uTextures.Length; i++)
            {
                if (_uTextures[i]?.Value != null)
                {
                    Texture2D texture = _uTextures[i].Value;
                    GraphicsDevice.Textures[i + 1] = texture;

                    if (_samplerStates[i] != null)
                    {
                        GraphicsDevice.SamplerStates[i + 1] = _samplerStates[i];
                    }
                    else if (Utils.IsPowerOfTwo(texture.Width) && Utils.IsPowerOfTwo(texture.Height))
                    {
                        GraphicsDevice.SamplerStates[i + 1] = SamplerState.LinearWrap;
                    }
                    else
                    {
                        GraphicsDevice.SamplerStates[i + 1] = SamplerState.AnisotropicClamp;
                    }

                    uImageSizeParams[i+1].SetValue(texture.Size() * _textureScales[i]);
                }
            }
            //base.Apply();
        }

        public override void CacheParams()
        {
            //int n = 0;
            uColorParam = Parameters["uColor"];
            uSecondaryColorParam = Parameters["uSecondaryColor"];
            uScreenResolutionParam = Parameters["uScreenResolution"];
            uScreenPositionParam = Parameters["uScreenPosition"];
            uTargetPositionParam = Parameters["uTargetPosition"];
            uDirectionParam = Parameters["uDirection"];
            uOpacityParam = Parameters["uOpacity"];
            uTimeParam = Parameters["uTime"];
            uIntensityParam = Parameters["uIntensity"];
            uProgressParam = Parameters["uProgress"];
            uImageSizeParams = new EffectParameter[3] 
            { 
                Parameters["uImageSize1"], 
                Parameters["uImageSize2"], 
                Parameters["uImageSize3"] 
            };
            //uImageSizeParam = new EffectParameter[3];
            //uImageSizeParam[0] = Parameters["uImageSize1"];
            //uImageSizeParam[1] = Parameters["uImageSize2"];
            //uImageSizeParam[2] = Parameters["uImageSize3"];
            //uImageSize1Param = Parameters["uImageSize1"];
            //uImageSize2Param = Parameters["uImageSize2"];
            //uImageSize3Param = Parameters["uImageSize3"];
            uImageOffsetParam = Parameters["uImageOffset"];
            uSaturationParam = Parameters["uSaturation"];
            uSourceRectParam = Parameters["uSourceRect"];
            uZoomParam = Parameters["uZoom"];
        }

    }
}
