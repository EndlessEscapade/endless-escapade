using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EEMod
{
    public class EEModConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static EEModConfigClient Instance;

        [DefaultValue(true)]
        [Label("$Mods.EEMod.Common.EEModClassTips")]
        [Tooltip("$Mods.EEMod.Common.EEModClassTipsInfo")]
        public bool EEModClassTooltips;

        [DefaultValue(false)]
        [Label("Particles")]
        [Tooltip("Enable Particle Effects, May have to restart world!")]
        public bool ParticleEffects;

        [DefaultValue(false)]
        [Label("Burning Simulator")]
        [Tooltip("EXTEREMLY WIP AND EXPERIMENTAL. I Recommend just using it for screen shots")]
        public bool BetterLighting;

        [DefaultValue(false)]
        [Label("Dynamic Camera Movement")]
        [Tooltip("Enable Camera Movement that moves as the player moves")]
        public bool CamMoveBool;

        [DefaultValue(false)]
        [Label("Debug Options")]
        [Tooltip("DEVELOPER ONLY")]
        public bool EEDebug;
    }
}