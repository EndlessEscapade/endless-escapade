using System;
using EndlessEscapade.Common.Config;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Filters;

[Autoload(Side = ModSide.Client)]
public class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction;

    static LowPassSystem() {
        var type = typeof(SoundEffectInstance);
        var method = type.GetMethod("INTERNAL_applyLowPassFilter", ReflectionUtils.PrivateInstanceFlags);

        lowPassAction = method.CreateDelegate<Action<SoundEffectInstance, float>>();
    }

    public override bool IsLoadingEnabled(Mod mod) {
        var config = ModContent.GetInstance<AudioConfig>();

        return SoundEngine.IsAudioSupported && config.EnableLowPassFiltering;
    }

    public static void ApplyParameters(SoundEffectInstance instance, AudioParameters parameters) {
        var intensity = 1f - parameters.LowPass * 0.9f;

        lowPassAction.Invoke(instance, intensity);
    }
}
