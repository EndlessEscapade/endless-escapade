/*
 * Inspiration taken from
 * https://github.com/Mirsario/TerrariaOverhaul
 */

using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

public sealed class LowPassFilter : IAudioFilter
{
    private static Action<SoundEffectInstance, float> lowPassAction;

    public void Load(Mod mod) {
        lowPassAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate<Action<SoundEffectInstance, float>>();

        if (lowPassAction == null) {
            throw new MissingMethodException(nameof(SoundEffectInstance), "INTERNAL_applyLowPassFilter");
        }
    }

    public void Unload() { }

    public void Apply(SoundEffectInstance instance, in AudioModifiers parameters) {
        var lowPass = parameters.LowPass;

        if (lowPass <= 0f || instance?.IsDisposed == true) {
            return;
        }

        lowPassAction.Invoke(instance, 1f - lowPass);
    }
}
