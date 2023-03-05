using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration.Attributes;

public class LocalizedTooltipAttribute : TooltipAttribute
{
    public LocalizedTooltipAttribute(string key) : base($"$Mods.{nameof(EndlessEscapade)}.Configuration.{key}.Tooltip") { }
}