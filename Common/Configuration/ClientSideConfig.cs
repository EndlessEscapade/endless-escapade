using EndlessEscapade.Common.Config.Attributes;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Config;

public class ClientSideConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;
    
    [LocalizedLabel("ToggleAmbience")]
    [LocalizedTooltip("ToggleAmbience")]
    public bool ToggleAmbience { get; set; }
    
    [LocalizedLabel("ToggleAudioEffects")]
    [LocalizedTooltip("ToggleAudioEffects")]
    public bool ToggleAudioEffects { get; set; }
}