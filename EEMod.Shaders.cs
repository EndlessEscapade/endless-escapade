using EEMod.Autoloading;
using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using System.Diagnostics;
using EEMod.Items.Dyes;
using ReLogic.Content;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

namespace EEMod
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class StaticShaderLoadAttribute : Attribute
    {
        public AssetRequestMode RequestMode { get; private set; }
        public bool ScreenShader { get; set; }
        public StaticShaderLoadAttribute(bool ScreenShader = false, AssetRequestMode RequestMode = AssetRequestMode.ImmediateLoad)
        { 
            this.RequestMode = RequestMode; 
            this.ScreenShader = ScreenShader; 
        }
    }

    public partial class EEMod
    {
        [StaticShaderLoad] public static Effect
            Noise2DShift,
            WaterShader,
            SeamapReflectionShader,
            WaterShaderBase,
            SeamapBorderVignette,
            SeamapShadowShader,
            SeamapCloudShader,
            PrismShader,
            SpireShine,
            NonBasicEffectShader,
            RadialSurfacing,
            WhiteOutlineSolid,
            LightingBuffer,
            WhiteOutline,
            Effervescence,
            Colorify,
            HydrosEmerge,
            LightningShader,
            DarksaberShader,
            ContinuousPrimTexShader,
            ShadowWarp,
            TornSailShader,
            PixelationShader,
            BloomShader,
            HydrosDye,
            AquamarineDye,
            SeafoamShader,
            ShadowMagic,
            PolkaDot;

        public static Effect NoiseSurfacing;

        [StaticShaderLoad(true)]
        public static Effect ReflectionShader;

        public static BasicEffect BasicEffect;

        static void QuickLoadScreenShader(string Path)
        {
            string EffectPath = "Effects/ScreenShaders/" + Path;
            string DictEntry = "EEMod:" + Path;

            Ref<Effect> Reference = new Ref<Effect>(ModContent.GetInstance<EEMod>().Assets.Request<Effect>(EffectPath, AssetRequestMode.ImmediateLoad).Value);

            Filters.Scene[DictEntry] = new Filter(new ScreenShaderData(Reference, Reference.Value.Techniques[0].Passes[0].Name), EffectPriority.VeryHigh);
            Filters.Scene[DictEntry].Load();
        }

        static void LoadStaticFields()
        {
            FieldInfo[] fieldInfo = typeof(EEMod).GetFields();
            foreach (FieldInfo fi in fieldInfo)
            {
                StaticShaderLoadAttribute att = fi.GetCustomAttribute<StaticShaderLoadAttribute>();
                if (att != null)
                {
                    //can someone like... do an effect exists check. Its 2:38am and I literally want to die
                    string ScreenShaderPath = att.ScreenShader ? "ScreenShaders/" : "";
                    fi.SetValue(null, ModContent.GetInstance<EEMod>().Assets.Request<Effect>($"Effects/{ScreenShaderPath + fi.Name}", att.RequestMode).Value);
                }
            }
        }

        [LoadingMethod(LoadMode.Client)]
        internal static void ShaderLoading()
        {
            Main.QueueMainThreadAction(() =>
            {
                EEMod instance = ModContent.GetInstance<EEMod>();

                BasicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
                BasicEffect.VertexColorEnabled = true;

                foreach(string name in instance.GetFile().Select(entry=>entry.Name))
                {
                    const string prefix = "Effects/ScreenShaders/";
                    const string postfix = ".xnb";
                    if (!name.Contains(prefix) || !name.EndsWith(postfix))
                        continue;

                    // Effects/ScreenShaders/a.xnb -> a
                    int d = name.IndexOf(prefix);
                    string alteredPath = name.Substring(d + prefix.Length, name.LastIndexOf(postfix) - d - prefix.Length); 
                    QuickLoadScreenShader(alteredPath);
                }

                LoadStaticFields();

                LightingBuffer.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(new Ref<Effect>(HydrosDye), "HydrosDyeShader"));
                GameShaders.Armor.BindShader(ModContent.ItemType<AquamarineDye>(), new ArmorShaderData(new Ref<Effect>(AquamarineDye), "AquamarineDyeShader"));
            });
        }
    }
}