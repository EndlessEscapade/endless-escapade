namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///     Provides <see cref="Mod"/> extension methods.
/// </summary>
public static class ModExtensions
{
    public static string GetLocalizationValue(this Mod mod, string key) {
        return mod.GetLocalization(key).Value;
    }
}
