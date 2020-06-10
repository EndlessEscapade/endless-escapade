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
    }
}
