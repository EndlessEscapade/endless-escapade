using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using System.Reflection;
using EEMod.Config;
using EEMod.Subworlds.CoralReefs;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public SoundEffect Dolphins { get; set; }
        public SoundEffectInstance DolphinsInstance { get; set; }
        public SoundEffect Seagulls { get; set; }
        public SoundEffectInstance SeagullsInstance { get; set; }
        public SoundEffect Seagulls2 { get; set; }
        public SoundEffectInstance Seagulls2Instance { get; set; }
        public SoundEffect Waves { get; set; }
        public SoundEffectInstance WavesInstance { get; set; }

        public float wavesVolume;

        public static void InitializeAmbience()
        {
            var mod = ModContent.GetInstance<EEMod>();

            if (!Main.dedServ)
            {
                mod.Dolphins = ModContent.Request<SoundEffect>("EEMod/Assets/Ambient/SurfaceReefsAmbienceDolphins", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                mod.DolphinsInstance = mod.Dolphins.CreateInstance();
                mod.Dolphins.Name = "Dolphins";

                mod.Seagulls = ModContent.Request<SoundEffect>("EEMod/Assets/Ambient/SurfaceReefsAmbienceSeagulls", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                mod.SeagullsInstance = mod.Seagulls.CreateInstance();
                mod.Seagulls.Name = "Seagulls";

                mod.Seagulls2 = ModContent.Request<SoundEffect>("EEMod/Assets/Ambient/SurfaceReefsAmbienceSeagulls2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                mod.Seagulls2Instance = mod.Seagulls2.CreateInstance();
                mod.Seagulls2.Name = "Seagulls2";

                mod.Waves = ModContent.Request<SoundEffect>("EEMod/Assets/Ambient/SurfaceReefsAmbienceWaves", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                mod.WavesInstance = mod.Waves.CreateInstance();
                mod.Waves.Name = "Waves";

                mod.WavesInstance.IsLooped = true;
            }
        }

        public static void UpdateAmbience()
        {
            if (Main.gameMenu) return;

            Player player = Main.LocalPlayer;

            var loader = ModContent.GetInstance<EEMod>();

            if (SubworldLibrary.SubworldSystem.IsActive<CoralReefs>() && player.Center.Y < ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
            {
                loader.WavesInstance.Volume = Main.ambientVolume;

                if (loader.WavesInstance.State != SoundState.Playing) { loader.WavesInstance.Play(); }

                if (Main.rand.NextBool(1800))
                {
                    loader.SeagullsInstance.Play();
                }
                else if (Main.rand.NextBool(1800))
                {
                    loader.Seagulls2Instance.Play();
                }
                else if (Main.rand.NextBool(1800))
                {
                    loader.DolphinsInstance.Play();
                }
            }
            else
            {
                loader.WavesInstance.Volume = 0;
            }
        }
    }
}
