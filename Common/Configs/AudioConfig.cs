using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configs;

public class AudioConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("Sounds")]
    [DefaultValue(true)]
    public bool EnableAmbienceSounds { get; set; } = true;
    
    [Header("Effects")]
    [DefaultValue(true)]
    public bool EnableLowPassFiltering { get; set; } = true;
}
