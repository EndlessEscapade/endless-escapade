using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader.Core;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class SignalsSystem : ModSystem
{
    public readonly struct SignalData(int mask, SignalUpdaterCallback callback)
    {
        public bool Enabled {
            get => HasFlag(Mask);
            set => SetFlag(Mask, value);
        }

        public readonly int Mask = mask;

        public readonly SignalUpdaterCallback Callback = callback;
    }

    public delegate bool SignalUpdaterCallback(in SignalContext context);

    private static readonly Dictionary<string, SignalData> Data = [];

    private static int flags;

    public override void Load() {
        base.Load();

        LoadModdedUpdaters(Mod);
        LoadVanillaUpdaters();
    }

    public override void PostUpdatePlayers() {
        base.PostUpdatePlayers();

        foreach (var (_, data) in Data) {
            data.Enabled = data.Callback?.Invoke(SignalContext.Default) ?? false;
        }
    }

    /// <summary>
    ///     Checks if a signal is active.
    /// </summary>
    /// <param name="name">The name of the signal to check.</param>
    /// <returns><c>true</c> if the signal was found and is active; otherwise, <c>false</c>.</returns>
    public static bool GetSignal(string name) {
        return Data[name].Enabled;
    }

    /// <summary>
    ///     Checks if any of the specified signals are active.
    /// </summary>
    /// <param name="names">The names of signals to check.</param>
    /// <returns><c>true</c> if any of the specified signals were found and are active; otherwise, <c>false</c>.</returns>
    public static bool GetSignal(params string[] names) {
        var success = false;

        for (var i = 0; i < names.Length; i++) {
            if (GetSignal(names[i])) {
                success = true;
                break;
            }
        }

        return success;
    }

    /// <summary>
    ///     Registers a new signal updater.
    /// </summary>
    /// <param name="name">The name of the signal to register.</param>
    /// <param name="callback">The callback of the signal to register.</param>
    public static void RegisterUpdater(string name, SignalUpdaterCallback? callback) {
        var mask = 1 << Data.Count;

        Data[name] = new SignalData(mask, callback);
    }

    private static void SetFlag(int mask, bool value) {
        if (value) {
            flags |= mask;
        }
        else {
            flags &= ~mask;
        }
    }

    private static bool HasFlag(int mask) {
        return (flags & mask) != 0;
    }

    private static void LoadModdedUpdaters(Mod mod) {
        foreach (var type in AssemblyManager.GetLoadableTypes(mod.Code)) {
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                var attribute = method.GetCustomAttribute<SignalUpdaterAttribute>();

                if (attribute == null) {
                    continue;
                }

                var callback = method.CreateDelegate<SignalUpdaterCallback>();
                var name = attribute.Name ?? method.Name;

                RegisterUpdater(name, callback);
            }
        }
    }

    private static void LoadVanillaUpdaters() {
        RegisterUpdater("Forest", static (in SignalContext context) => context.Player.ZonePurity);
        RegisterUpdater("Day", static (in SignalContext _) => Main.dayTime);
        RegisterUpdater("Night", static (in SignalContext _) => !Main.dayTime);
    }
}
