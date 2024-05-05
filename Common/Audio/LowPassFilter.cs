using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

public sealed class LowPassFilter : IAudioFilter
{
    private static readonly Action<SoundEffectInstance, float> ApplyLowPassFilterAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    void ILoadable.Load(Mod mod) {
        if (!SoundEngine.IsAudioSupported) {
            mod.Logger.Warn($"{nameof(AudioManager)} was disabled: {nameof(SoundEngine)}.{nameof(SoundEngine.IsAudioSupported)} was false.");
            return;
        }

        if (ApplyLowPassFilterAction != null) {
            return;
        }

        mod.Logger.Warn($"{nameof(LowPassFilter)} was disabled: Failed to find internal FNA methods.");
    }

    void ILoadable.Unload() { }

    void IAudioFilter.Apply(SoundEffectInstance instance, in AudioParameters parameters) {
        var lowPass = parameters.LowPass;

        if (lowPass <= 0f || instance?.IsDisposed == true) {
            return;
        }

        ApplyLowPassFilterAction.Invoke(instance, 1f - lowPass);
    }
}
