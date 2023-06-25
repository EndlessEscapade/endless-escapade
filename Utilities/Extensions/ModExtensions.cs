using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Utilities.Extensions;

public static class ModExtensions
{
    /// <summary>Shorthand for <c>Language.GetTextValue($"Mods.{mod.Name}.{key}", args)</c>.</summary>
    public static string GetTextValue(this Mod mod, string key, params object[] args) {
        return Language.GetTextValue($"Mods.{mod.Name}.{key}", args);
    }
}