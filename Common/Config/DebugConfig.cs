using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Config;

internal class DebugConfig : ModConfig
{
    internal static DebugConfig Instance => ModContent.GetInstance<DebugConfig>();

    public override ConfigScope Mode => ConfigScope.ClientSide;

    public bool EnableReloadNRejoin { get; set; }

    public bool ReloadNRejoinExitNoSaveByDefault { get; set; }
}
