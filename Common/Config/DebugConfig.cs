using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Config
{
    internal class DebugConfig : ModConfig
    {
        internal static DebugConfig Instance => ModContent.GetInstance<DebugConfig>();

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public bool EnableReloadNRejoin { get; set; }

        public bool ReloadNRejoinExitNoSaveByDefault { get; set; }
    }
}
