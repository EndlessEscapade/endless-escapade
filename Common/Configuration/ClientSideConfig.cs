using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration;

public class ClientSideConfig : ModConfig
{
    public static ClientSideConfig Instance => ModContent.GetInstance<ClientSideConfig>();

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    [Header($"$Mods.{nameof(EndlessEscapade)}.Configuration.Miscellaneous.Header")]
    [Label($"$Mods.{nameof(EndlessEscapade)}.Configuration.Miscellaneous.EnableWindowTitles.Label")]
    [Tooltip($"$Mods.{nameof(EndlessEscapade)}.Configuration.Miscellaneous.EnableWindowTitles.Tooltip")]
    public bool EnableWindowTitles { get; set; }
}