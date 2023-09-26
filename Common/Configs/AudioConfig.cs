using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configs;

public class AudioConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("Effects")]
    [Slider]
    [Range(0f, 1f)]
    [Increment(0.05f)]
    [DefaultValue(1f)]
    public float LowPassFilteringIntensity { get; set; } = 1f;
}
