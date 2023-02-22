using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

[Autoload(Side = ModSide.Client)]
public class AudioEffectsSystem : ModSystem
{
    private Action<SoundEffectInstance, float> highPassFilterAction;
    private Action<SoundEffectInstance, float> lowPassFilterAction;
    private Action<SoundEffectInstance, float> reverbAction;

    public override void OnModLoad() {
        if (!SoundEngine.IsAudioSupported) {
            return;
        }

        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;

        reverbAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyReverb", flags).CreateDelegate<Action<SoundEffectInstance, float>>();
        lowPassFilterAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", flags).CreateDelegate<Action<SoundEffectInstance, float>>();
        highPassFilterAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyHighPassFilter", flags).CreateDelegate<Action<SoundEffectInstance, float>>();
    }
}