using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio;

[Autoload(Side = ModSide.Client)]
public class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }

    public static void ApplyModifiers(SoundEffectInstance instance, AudioModifiers modifiers) {
        lowPassAction?.Invoke(instance, 1f - modifiers.LowPass);
    }
}
