using System;
using EndlessEscapade.Common.Configs;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Filters;

[Autoload(Side = ModSide.Client)]
public sealed class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyLowPassFilter", ReflectionUtils.PrivateInstanceFlags)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    public static bool Enabled { get; private set; }

    public override void OnModLoad() {
        Enabled = SoundEngine.IsAudioSupported;
    }

    internal static void ApplyParameters(SoundEffectInstance instance, SoundModifiers parameters) {
        var intensity = ModContent.GetInstance<AudioConfig>().LowPassFilteringIntensity;
        var lowPass = parameters.LowPass * intensity;

        if (!Enabled || lowPass <= 0f || instance?.IsDisposed == true) {
            return;
        }

        lowPassAction.Invoke(instance, 1f - lowPass);
    }
}
