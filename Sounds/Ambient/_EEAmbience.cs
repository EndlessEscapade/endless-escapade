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
            var loader = ModContent.GetInstance<EEMod>();

            if (!Main.dedServ)
            {
                loader.Dolphins = ModContent.GetSound("EEMod/Sounds/Ambient/SurfaceReefsAmbienceDolphins");
                loader.DolphinsInstance = loader.Dolphins.CreateInstance();
                loader.Dolphins.Name = "Dolphins";

                loader.Seagulls = ModContent.GetSound("EEMod/Sounds/Ambient/SurfaceReefsAmbienceSeagulls");
                loader.SeagullsInstance = loader.Seagulls.CreateInstance();
                loader.Seagulls.Name = "Seagulls";

                loader.Seagulls2 = ModContent.GetSound("EEMod/Sounds/Ambient/SurfaceReefsAmbienceSeagulls2");
                loader.Seagulls2Instance = loader.Seagulls2.CreateInstance();
                loader.Seagulls2.Name = "Seagulls2";

                loader.Waves = ModContent.GetSound("EEMod/Sounds/Ambient/SurfaceReefsAmbienceWaves");
                loader.WavesInstance = loader.Waves.CreateInstance();
                loader.Waves.Name = "Waves";

                loader.WavesInstance.IsLooped = true;
            }
        }

        public static void UpdateAmbience()
        {
            /*if (Main.gameMenu) return;

            Player player = Main.LocalPlayer;

            var loader = ModContent.GetInstance<EEMod>();

            if (player.GetModPlayer<EEPlayer>().ZoneCoralReefs && player.Center.Y < ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
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
            }*/
        }
    }
}
