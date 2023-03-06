using System.ComponentModel;
using EndlessEscapade.Common.Configuration.Attributes;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration;

[LocalizedLabel("Miscellaneous")]
public class MiscellaneousConfig : ModConfig
{
    public static MiscellaneousConfig Instance => ModContent.GetInstance<MiscellaneousConfig>();

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    [LocalizedLabel("Miscellaneous.EnableWindowTitles")]
    [LocalizedTooltip("Miscellaneous.EnableWindowTitles")]
    public bool EnableWindowTitles { get; set; }
}