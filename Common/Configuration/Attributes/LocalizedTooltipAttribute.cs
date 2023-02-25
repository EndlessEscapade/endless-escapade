using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration.Attributes;

public class LocalizedTooltipAttribute : TooltipAttribute
{
    public LocalizedTooltipAttribute(string entryName) : base($"$Mods.{nameof(EndlessEscapade)}.Configuration.{entryName}.Tooltip") { }
}