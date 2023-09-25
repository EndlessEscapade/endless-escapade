using System;
using EndlessEscapade.Common.Configs;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Filters;

[Autoload(Side = ModSide.Client)]
public class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyLowPassFilter", ReflectionUtils.PrivateInstanceFlags)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    public static bool Enabled { get; private set; }

    public override void Load() {
        Enabled = SoundEngine.IsAudioSupported;

        if (!Enabled) {
            Mod.Logger.Error("Audio effects were not enabled: Sound engine does not support audio.");
        }
    }

    internal static void ApplyParameters(SoundEffectInstance instance, SoundModifiers parameters) {
        if (!Enabled || parameters.LowPass <= 0f || instance?.IsDisposed == true || !ModContent.GetInstance<AudioConfig>().EnableLowPassFiltering) {
            return;
        }

        lowPassAction.Invoke(instance, 1f - parameters.LowPass * 0.99f);
    }
}
