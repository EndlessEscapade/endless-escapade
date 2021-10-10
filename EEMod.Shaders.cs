using EEMod.Autoloading;
using EEMod.NPCs.Bosses.Akumo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using System.Diagnostics;
using EEMod.Skies;
using EEMod.Items.Dyes;
using ReLogic.Content;

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
        public static Effect White;
        public static Effect Effervescence;
        public static Effect Colorify;
        public static Effect hydrosIntro;
        public static Effect lightningShader;

        [LoadingMethod(LoadMode.Client)]
        internal static void ShaderLoading()
        {
            Main.QueueMainThreadAction(() =>
            {
                EEMod instance = ModContent.GetInstance<EEMod>();

                LightingBufferEffect = instance.Assets.Request<Effect>("Effects/LightingBuffer", AssetRequestMode.ImmediateLoad).Value;
                SolidOutline = instance.Assets.Request<Effect>("Effects/WhiteOutlineSolid", AssetRequestMode.ImmediateLoad).Value;
                RadialField = instance.Assets.Request<Effect>("Effects/RadialSurfacing", AssetRequestMode.ImmediateLoad).Value;
                PrismShader = instance.Assets.Request<Effect>("Effects/PrismShader", AssetRequestMode.ImmediateLoad).Value;
                SpireShader = instance.Assets.Request<Effect>("Effects/SpireShine", AssetRequestMode.ImmediateLoad).Value;
                Noise2DShift = instance.Assets.Request<Effect>("Effects/Noise2DShift", AssetRequestMode.ImmediateLoad).Value;
                ReflectionShader = instance.Assets.Request<Effect>("Effects/ReflectionShader", AssetRequestMode.ImmediateLoad).Value;
                WaterShader = instance.Assets.Request<Effect>("Effects/WaterShader", AssetRequestMode.ImmediateLoad).Value;
                TrailPractice = instance.Assets.Request<Effect>("Effects/NonBasicEffectShader", AssetRequestMode.ImmediateLoad).Value;
                NoiseSurfacing = instance.Assets.Request<Effect>("Effects/NoiseSurfacing", AssetRequestMode.ImmediateLoad).Value;
                White = instance.Assets.Request<Effect>("Effects/WhiteOutline", AssetRequestMode.ImmediateLoad).Value;
                Effervescence = instance.Assets.Request<Effect>("Effects/Effervescence", AssetRequestMode.ImmediateLoad).Value;
                hydrosIntro = instance.Assets.Request<Effect>("Effects/HydrosEmerge", AssetRequestMode.ImmediateLoad).Value;
                lightningShader = instance.Assets.Request<Effect>("Effects/LightningShader", AssetRequestMode.ImmediateLoad).Value;

                Debug.WriteLine(LightingBufferEffect == null);

                LightingBufferEffect.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

                Ref<Effect> screenRef = new(instance.Assets.Request<Effect>("Effects/PracticeEffect", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef2 = new(instance.Assets.Request<Effect>("Effects/Shockwave", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef3 = new(instance.Assets.Request<Effect>("Effects/Pause", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef4 = new(instance.Assets.Request<Effect>("Effects/WhiteFlash", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef5 = new(instance.Assets.Request<Effect>("Effects/Saturation", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef6 = new(Noise2D);
                Ref<Effect> screenRef7 = new(instance.Assets.Request<Effect>("Effects/SeaOpening", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef8 = new(instance.Assets.Request<Effect>("Effects/LightSource", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef9 = new(instance.Assets.Request<Effect>("Effects/ReflectionShader", AssetRequestMode.ImmediateLoad).Value);

                Ref<Effect> hydrosDye = new Ref<Effect>(instance.Assets.Request<Effect>("Effects/HydrosDye", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> MyTestShader = new Ref<Effect>(instance.Assets.Request<Effect>("Effects/MyTestShader", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> aquamarineDye = new Ref<Effect>(instance.Assets.Request<Effect>("Effects/AquamarineDye", AssetRequestMode.ImmediateLoad).Value);

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
                Filters.Scene["EEMod:SavingCutscene"] = new Filter(new SavingSkyData("FilterMiniTower").UseColor(0f, 0.20f, 1f).UseOpacity(0.3f), EffectPriority.High);
                Filters.Scene["EEMod:MyTestShader"] = new Filter(new ScreenShaderData(MyTestShader, "MyTestShaderFlot"), EffectPriority.High);
                Filters.Scene["EEMod:SunThroughWalls"] = new Filter(new ScreenShaderData(screenRef, "SunThroughWalls"), EffectPriority.High);
                Filters.Scene["EEMod:SeaTrans"] = new Filter(new ScreenShaderData(screenRef2, "SeaTrans"), EffectPriority.High);
                Filters.Scene["EEMod:Ripple"] = new Filter(new ScreenShaderData(screenRef3, "Ripple"), EffectPriority.High);

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
                Filters.Scene["EEMod:Ripple"].Load();
                Filters.Scene["EEMod:SeaTrans"].Load();
                Filters.Scene["EEMod:SunThroughWalls"].Load();
                Filters.Scene["EEMod:MyTestShader"].Load();

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
                SkyManager.Instance["EEMod:SavingCutscene"] = new SavingSky();

                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(hydrosDye, "HydrosDyeShader"));
                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(aquamarineDye, "AquamarineDyeShader"));

                //instance.Assets.Request<Effect>("Effects/Noise2D").Value.Parameters["noiseTexture"].SetValue(instance.Assets.Request<Texture2D>("Textures/Noise/noise").Value);
                
                //GameShaders.Misc["EEMod:SpireHeartbeat"] = new MiscShaderData(new Ref<Effect>(instance.Assets.Request<Effect>("Effects/SpireShine", AssetRequestMode.ImmediateLoad).Value), "SpireHeartbeat").UseImage0("Textures/Noise/WormNoisePixelated");
            });
        }
    }
}