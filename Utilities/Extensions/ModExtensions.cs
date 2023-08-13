using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Utilities.Extensions;

public static class ModExtensions
{
    /// <summary>Shorthand for <c>Language.GetTextValue($"Mods.{mod.Name}.{key}", args)</c>.</summary>
    public static string GetLocalizationValue(this Mod mod, string key) {
        return Language.GetTextValue($"Mods.{mod.Name}.{key}");
    }
}
