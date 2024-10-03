using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

public sealed class LowPassFilter : IAudioFilter
{
    void IAudioFilter.Apply(SoundEffectInstance instance, in AudioParameters parameters) {
        var intensity = parameters.LowPass;

        if (intensity <= 0f || instance?.IsDisposed == true) {
            return;
        }

        instance.INTERNAL_applyLowPassFilter(1f - intensity);
    }
}
