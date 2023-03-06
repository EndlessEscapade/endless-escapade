using Terraria.ModLoader;
using Terraria.Utilities;

namespace EndlessEscapade.Utilities.Extensions;

public static class WeightedRandomExtensions
{
    public static void AddLocalizationRange(this WeightedRandom<string> random, Mod mod, string key, int range) {
        for (int i = 0; i < range; i++) {
            random.Add(mod.GetTextValue(key + i));
        }
    }
}