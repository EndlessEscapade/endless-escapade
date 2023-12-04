using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace EndlessEscapade.Common.Surroundings;

public sealed class SurroundingsSystem : ModSystem
{
    public delegate bool SurroundingsUpdater(in SurroundingsInfo info);

    private static readonly Dictionary<string, bool> flagsByName = new();
    private static readonly Dictionary<string, SurroundingsUpdater> updatersByName = new();

    public override void Load() {
        foreach (var type in AssemblyManager.GetLoadableTypes(Mod.Code)) {
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                var attribute = method.GetCustomAttribute<SurroundingsUpdaterAttribute>();

                if (attribute == null) {
                    continue;
                }

                var function = method.CreateDelegate<SurroundingsUpdater>();

                updatersByName[attribute.Name ?? method.Name] = function;
            }
        }
    }

    public override void PostUpdateEverything() {
        var info = new SurroundingsInfo {
            Player = Main.LocalPlayer,
            Metrics = Main.SceneMetrics
        };

        foreach (var (name, function) in updatersByName) {
            flagsByName[name] = function.Invoke(in info);
        }
    }
}
