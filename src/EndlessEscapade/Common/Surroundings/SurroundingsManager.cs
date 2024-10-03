using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace EndlessEscapade.Common.Surroundings;

public sealed class SurroundingsManager : ModSystem
{
    public delegate bool SurroundingsCallback(in SurroundingsInfo info);

    private static readonly Dictionary<string, bool> FlagsByName = new();
    private static readonly Dictionary<string, SurroundingsCallback> CallbacksByName = new();

    public override void Load() {
        foreach (var type in AssemblyManager.GetLoadableTypes(Mod.Code)) {
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                var attribute = method.GetCustomAttribute<SurroundingsCallbackAttribute>();

                if (attribute == null) {
                    continue;
                }

                var callback = method.CreateDelegate<SurroundingsCallback>();

                CallbacksByName[attribute.Name] = callback;
            }
        }
    }

    public override void PreUpdateWorld() {
        var info = new SurroundingsInfo {
            Player = Main.LocalPlayer,
            Metrics = Main.SceneMetrics
        };

        foreach (var (name, function) in CallbacksByName) {
            FlagsByName[name] = function.Invoke(in info);
        }
    }

    public static bool TryGetSurrounding(string name, out bool surrounding) {
        return FlagsByName.TryGetValue(name, out surrounding);
    }
}
