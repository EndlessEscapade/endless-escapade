using EEMod.Autoloading;
using EEMod.NPCs.Bosses.Akumo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace EEMod
{
    public partial class EEMod
    {
        public static int noOfPasses = 1;
        public static int startingTermination = 1;
        public static int maxNumberOfLights = 1000;
        public static Effect Noise2DShift;
        public static Effect ReflectionShader;
        public static Effect WaterShader;
        public static Effect PrismShader;
        public static Effect SpireShader;
        public static Effect TrailPractice;
        public static Effect RadialField;
        public static Effect SolidOutline;
        public static Effect LightingBufferEffect;

        [LoadingMethod(LoadMode.Client)]
        internal static void ShaderLoading()
        {
            Main.QueueMainThreadAction(() =>
            {
                LightingBufferEffect = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/LightingBuffer").Value;
                SolidOutline = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/WhiteOutlineSolid").Value;
                RadialField = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/RadialSurfacing").Value;
                PrismShader = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/PrismShader").Value;
                SpireShader = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/SpireShine").Value;
                Noise2DShift = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/Noise2DShift").Value;
                ReflectionShader = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/ReflectionShader").Value;
                WaterShader = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/WaterShader").Value;
                TrailPractice = ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/NonBasicEffectShader").Value;

                //LightingBufferEffect.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

                Ref<Effect> screenRef = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/PracticeEffect").Value);
                Ref<Effect> screenRef2 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/Shockwave").Value);
                Ref<Effect> screenRef3 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/Pause").Value);
                Ref<Effect> screenRef4 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/WhiteFlash").Value);
                Ref<Effect> screenRef5 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/Saturation").Value);
                Ref<Effect> screenRef6 = new(Noise2D);
                Ref<Effect> screenRef7 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/SeaOpening").Value);
                Ref<Effect> screenRef8 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/LightSource").Value);
                Ref<Effect> screenRef9 = new(ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/ReflectionShader").Value);

                //ModContent.GetInstance<EEMod>().Assets.Request<Effect>("Effects/Noise2D").Value.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise").Value);

                Filters.Scene["EEMod:Akumo"] = new Filter(new AkumoScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.5f, 0.2f).UseOpacity(0.6f), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:Boom"] = new Filter(new ScreenShaderData(screenRef, "DeathAnimation"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:Shockwave"] = new Filter(new ScreenShaderData(screenRef2, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:Pause"] = new Filter(new ScreenShaderData(screenRef3, "Pauses"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:Saturation"] = new Filter(new ScreenShaderData(screenRef5, "Saturation"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:SmoothRight"] = new Filter(new ScreenShaderData(screenRef5, "RightPass"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:SmoothDown"] = new Filter(new ScreenShaderData(screenRef5, "DownPass"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:SmoothRight2"] = new Filter(new ScreenShaderData(screenRef5, "RightPass2"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:SmoothDown2"] = new Filter(new ScreenShaderData(screenRef5, "DownPass2"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:Noise2D"] = new Filter(new ScreenShaderData(screenRef6, "Noise2D"), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:SeaOpening"] = new Filter(new ScreenShaderData(screenRef7, "SeaOpening"), EffectPriority.VeryHigh);
                
                Filters.Scene["EEMod:Boom"].Load();
                Filters.Scene["EEMod:Shockwave"].Load();
                Filters.Scene["EEMod:Pause"].Load();
                Filters.Scene["EEMod:Saturation"].Load();
                Filters.Scene["EEMod:SmoothRight"].Load();
                Filters.Scene["EEMod:SmoothDown"].Load();
                Filters.Scene["EEMod:SmoothRight2"].Load();
                Filters.Scene["EEMod:SmoothDown2"].Load();
                Filters.Scene["EEMod:Noise2D"].Load();
                Filters.Scene["EEMod:SeaOpening"].Load();

                for (int i = 0; i < maxNumberOfLights; i++)
                {
                    Filters.Scene[$"EEMod:LightSource{i}"] = new Filter(new ScreenShaderData(screenRef8, "LightSource"), EffectPriority.VeryHigh);
                    Filters.Scene[$"EEMod:LightSource{i}"].Load();
                }
                for (int i = startingTermination; i <= noOfPasses; i++)
                {
                    Filters.Scene[$"EEMod:Filter{i}"] = new Filter(new ScreenShaderData(screenRef4, $"Filter{i}"), EffectPriority.VeryHigh);
                    Filters.Scene[$"EEMod:Filter{i}"].Load();
                }

                SkyManager.Instance["EEMod:Akumo"] = new AkumoSky();
            });
        }
    }
}