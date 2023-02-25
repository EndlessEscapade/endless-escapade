using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration.Attributes;

public class LocalizedLabelAttribute : LabelAttribute
{
    public LocalizedLabelAttribute(string entryName) : base($"$Mods.{nameof(EndlessEscapade)}.Configuration.{entryName}.Label") { }
}