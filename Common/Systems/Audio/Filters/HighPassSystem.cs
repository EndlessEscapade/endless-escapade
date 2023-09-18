using System;
using EndlessEscapade.Common.Configs;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Filters;

[Autoload(Side = ModSide.Client)]
public class HighPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> highPassAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyHighPassFilter", ReflectionUtils.PrivateInstanceFlags)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    public static bool Enabled { get; private set; }

    public override void Load() {
        Enabled = false;

        if (!SoundEngine.IsAudioSupported) {
            Mod.Logger.Error("Audio effects were disabled: Sound engine does not support audio.");
            return;
        }

        Enabled = true;
    }

    internal static void ApplyParameters(SoundEffectInstance instance, SoundModifiers parameters) {
        if (!Enabled || parameters.HighPass <= 0f || instance?.IsDisposed == true || !ModContent.GetInstance<AudioConfig>().EnableHighPassFiltering) {
            return;
        }

        highPassAction.Invoke(instance, parameters.HighPass * 0.99f);
    }
}
