using EndlessEscapade.Core.Configuration;
using Microsoft.Xna.Framework.Audio;

namespace EndlessEscapade.Core.Audio;

public sealed class LowPassFilter : IAudioFilter
{
    void IAudioFilter.Apply(SoundEffectInstance instance, in AudioParameters parameters) {
        if (!ClientConfiguration.Instance.EnableLowPassFilter) {
            return;
        }

        var intensity = parameters.LowPass;

        if (intensity <= 0f || instance?.IsDisposed == true) {
            return;
        }

        instance.INTERNAL_applyLowPassFilter(1f - intensity);
    }
}
