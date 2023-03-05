using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration.Attributes;

public class LocalizedLabelAttribute : LabelAttribute
{
    public LocalizedLabelAttribute(string key) : base($"$Mods.{nameof(EndlessEscapade)}.Configuration.{key}.Label") { }
}