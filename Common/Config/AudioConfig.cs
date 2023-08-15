using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Config;

public class AudioConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    [Header($"$Mods.{nameof(EndlessEscapade)}.Config.Audio.Header")]
    [LabelKey($"$Mods.{nameof(EndlessEscapade)}.Config.Audio.{nameof(EnableLowPassFiltering)}.Label")]
    [TooltipKey($"$Mods.{nameof(EndlessEscapade)}.Config.Audio.{nameof(EnableLowPassFiltering)}.Tooltip")]
    public bool EnableLowPassFiltering { get; set; } = true;
}
