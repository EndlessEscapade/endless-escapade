using EEMod.Autoloading;
using EEMod.NPCs.Bosses.Akumo;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace EEMod
{
    public partial class EEMod
    {
        public static int noOfPasses = 1;
        public static int startingTermination = 1;
        public static int maxNumberOfLights = 1000;

        [LoadingMethod(LoadMode.Client)]
        internal static void ShaderLoading()
        {
            //instance is a static field and this method is still inside the mod class
            Ref<Effect> screenRef = new Ref<Effect>(instance.GetEffect("Effects/PracticeEffect"));
            Ref<Effect> screenRef2 = new Ref<Effect>(instance.GetEffect("Effects/Shockwave"));
            Ref<Effect> screenRef3 = new Ref<Effect>(instance.GetEffect("Effects/Pause"));
            Ref<Effect> screenRef4 = new Ref<Effect>(instance.GetEffect("Effects/WhiteFlash"));
            Ref<Effect> screenRef5 = new Ref<Effect>(instance.GetEffect("Effects/Saturation"));
            Ref<Effect> screenRef6 = new Ref<Effect>(Noise2D);
            Ref<Effect> screenRef7 = new Ref<Effect>(instance.GetEffect("Effects/SeaOpening"));
            Ref<Effect> screenRef8 = new Ref<Effect>(instance.GetEffect("Effects/LightSource"));
            instance.GetEffect("Effects/Noise2D").Parameters["noiseTexture"].SetValue(EEMod.instance.GetTexture("noise"));
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
        }
    }
}