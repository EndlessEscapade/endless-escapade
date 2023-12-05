using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

public sealed class LowPassFilter : IAudioFilter
{
    private static Action<SoundEffectInstance, float> lowPassAction;

    void ILoadable.Load(Mod mod) {
        if (!SoundEngine.IsAudioSupported) {
            mod.Logger.Warn($"Disabled '{nameof(LowPassFilter)}'! {nameof(SoundEngine)}.{nameof(SoundEngine.IsAudioSupported)} returned false.");
            return;
        }

        lowPassAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate<Action<SoundEffectInstance, float>>();

        if (lowPassAction == null) {
            mod.Logger.Warn($"Disabled '{nameof(LowPassFilter)}'! Missing internal FNA members.");
        }
    }

    void ILoadable.Unload() { }

    public void Apply(SoundEffectInstance instance, in AudioParameters parameters) {
        var lowPass = parameters.LowPass;

        if (lowPass <= 0f || instance?.IsDisposed == true) {
            return;
        }

        lowPassAction.Invoke(instance, 1f - lowPass);
    }
}
