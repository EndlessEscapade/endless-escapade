using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

[Autoload(Side = ModSide.Client)]
public sealed class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    public static bool Enabled { get; private set; }

    public override void OnModLoad() {
        if (lowPassAction == null) {
            throw new MissingMethodException(nameof(SoundEffectInstance), "INTERNAL_applyLowPassFilter");
        }

        Enabled = SoundEngine.IsAudioSupported;
    }

    internal static void ApplyParameters(SoundEffectInstance instance, SoundModifiers parameters) {
        var lowPass = parameters.LowPass;

        if (!Enabled || lowPass <= 0f || instance?.IsDisposed == true) {
            return;
        }

        lowPassAction.Invoke(instance, 1f - lowPass);
    }
}
