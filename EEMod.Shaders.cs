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
using System.IO;
using System.Reflection;
using System;

namespace EEMod
{
    public class StaticShaderLoadAttribute : Attribute
    {
        public AssetRequestMode RequestMode { get; private set; }
        public bool ScreenShader { get; set; }
        public StaticShaderLoadAttribute(bool ScreenShader = false, AssetRequestMode RequestMode = AssetRequestMode.ImmediateLoad)
        { this.RequestMode = RequestMode; this.ScreenShader = ScreenShader; }
    }

    public partial class EEMod
    {
        [StaticShaderLoad]
        public static Effect Noise2DShift;
        [StaticShaderLoad(true)]
        public static Effect ReflectionShader;
        [StaticShaderLoad]
        public static Effect WaterShader;
        [StaticShaderLoad]
        public static Effect PrismShader;
        [StaticShaderLoad]
        public static Effect SpireShine;
        [StaticShaderLoad]
        public static Effect NonBasicEffectShader;
        [StaticShaderLoad]
        public static Effect RadialSurfacing;
        [StaticShaderLoad]
        public static Effect WhiteOutlineSolid;
        [StaticShaderLoad]
        public static Effect LightingBuffer;
        [StaticShaderLoad]
        public static Effect WhiteOutline;
        [StaticShaderLoad]
        public static Effect Effervescence;
        [StaticShaderLoad]
        public static Effect Colorify;
        [StaticShaderLoad]
        public static Effect HydrosEmerge;
        [StaticShaderLoad]
        public static Effect LightningShader;
        [StaticShaderLoad]
        public static Effect ContinuousPrimTexShader;
        [StaticShaderLoad]
        public static Effect ShadowWarp;
        [StaticShaderLoad]
        public static Effect TornSailShader;
        [StaticShaderLoad]
        public static Effect PixelationShader;
        [StaticShaderLoad]
        public static Effect BloomShader;

        [StaticShaderLoad]
        public static Effect HydrosDye;
        [StaticShaderLoad]
        public static Effect AquamarineDye;

        public static BasicEffect BasicEffect;

        static void QuickLoadScreenShader(string Path)
        {
            string EffectPath = "Effects/ScreenShaders/" + Path;
            string DictEntry = "EEMod:" + Path;

            Ref<Effect> Reference = new Ref<Effect>(EEMod.Instance.Assets.Request<Effect>(EffectPath, AssetRequestMode.ImmediateLoad).Value);

            Filters.Scene[DictEntry] = new Filter(new ScreenShaderData(Reference, Reference.Value.Techniques[0].Passes[0].Name), EffectPriority.VeryHigh);
            Filters.Scene[DictEntry].Load();
        }

        static void LoadStaticFields()
        {
            FieldInfo[] fieldInfo = typeof(EEMod).GetFields();
            foreach (FieldInfo fi in fieldInfo)
            {
                StaticShaderLoadAttribute? att = fi.GetCustomAttribute(typeof(StaticShaderLoadAttribute)) as StaticShaderLoadAttribute;
                if (att != null)
                {
                    //can someone like... do an effect exists check. Its 2:38am and I literally want to die
                    string ScreenShaderPath = att.ScreenShader ? "ScreenShaders/" : "";
                    fi.SetValue(null, EEMod.Instance.Assets.Request<Effect>($"Effects/{ScreenShaderPath + fi.Name}", att.RequestMode).Value);
                }
            }
        }

        [LoadingMethod(LoadMode.Client)]
        internal static void ShaderLoading()
        {
            Main.QueueMainThreadAction(() =>
            {
                EEMod instance = EEMod.Instance;

                BasicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
                BasicEffect.VertexColorEnabled = true;

                string[] Shaders = Directory.GetFiles($@"{Main.SavePath}\Mod Sources\EEMod\Effects\ScreenShaders");
                for (int i = 0; i < Shaders.Length; i++)
                {
                    string filePath = Shaders[i];

                    if (filePath.Contains(".xnb") ||
                        filePath.Contains(".exe") ||
                        filePath.Contains(".dll")) continue;

                    string charSeprator = @"ScreenShaders\";
                    int Index = filePath.IndexOf(charSeprator) + charSeprator.Length;
                    string AlteredPath = filePath.Substring(Index);

                    QuickLoadScreenShader(AlteredPath.Replace(".fx", ""));
                }

                LoadStaticFields();

                LightingBuffer.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

                Filters.Scene["EEMod:Akumo"] = new Filter(new AkumoScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.5f, 0.2f).UseOpacity(0.6f), EffectPriority.VeryHigh);
                Filters.Scene["EEMod:SavingCutscene"] = new Filter(new SavingSkyData("FilterMiniTower").UseColor(0f, 0.20f, 1f).UseOpacity(0.3f), EffectPriority.High);

                SkyManager.Instance["EEMod:Akumo"] = new AkumoSky();
                SkyManager.Instance["EEMod:SavingCutscene"] = new SavingSky();

                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(new Ref<Effect>(HydrosDye), "HydrosDyeShader"));
                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(new Ref<Effect>(AquamarineDye), "AquamarineDyeShader"));

                //instance.Assets.Request<Effect>("Effects/Noise2D").Value.Parameters["noiseTexture"].SetValue(instance.Assets.Request<Texture2D>("Textures/Noise/noise").Value);

                //GameShaders.Misc["EEMod:SpireHeartbeat"] = new MiscShaderData(new Ref<Effect>(instance.Assets.Request<Effect>("Effects/SpireShine", AssetRequestMode.ImmediateLoad).Value), "SpireHeartbeat").UseImage0("Textures/Noise/WormNoisePixelated");
            });
        }
    }
}