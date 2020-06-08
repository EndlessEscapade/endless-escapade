using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Prophecy
{
    public class ProphecyConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static ProphecyConfigClient Instance;

        [DefaultValue(true)]
        [Label("$Mods.Prophecy.Common.ProphecyClassTips")]
        [Tooltip("$Mods.Prophecy.Common.ProphecyClassTipsInfo")]
        public bool ProphecyClassTooltips;
    }
}
