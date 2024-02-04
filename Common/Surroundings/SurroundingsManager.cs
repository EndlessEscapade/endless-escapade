using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace EndlessEscapade.Common.Surroundings;

// TODO: Support for checking whether a(specified) flag(s) is active or not.
public sealed class SurroundingsManager : ModSystem
{
    public delegate bool SurroundingsCallback(in SurroundingsInfo info);

    private static Dictionary<string, bool> flagsByName = new();
    private static Dictionary<string, SurroundingsCallback> callbacksByName = new();

    public override void Load() {
        foreach (var type in AssemblyManager.GetLoadableTypes(Mod.Code)) {
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                var attribute = method.GetCustomAttribute<SurroundingsCallbackAttribute>();

                if (attribute == null) {
                    continue;
                }

                var callback = method.CreateDelegate<SurroundingsCallback>();

                callbacksByName[attribute.Name] = callback;
            }
        }
    }

    public override void Unload() {
        flagsByName?.Clear();
        flagsByName = null;

        callbacksByName?.Clear();
        callbacksByName = null;
    }

    public override void PreUpdateWorld() {
        var info = new SurroundingsInfo {
            Player = Main.LocalPlayer,
            Metrics = Main.SceneMetrics
        };

        foreach (var (name, function) in callbacksByName) {
            flagsByName[name] = function.Invoke(in info);
        }
    }

    public static bool TryGetSurrounding(string name, out bool surrounding) {
        return flagsByName.TryGetValue(name, out surrounding);
    }
}
