using System;
using System.Reflection;
using EndlessEscapade.Common.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

[Autoload(Side = ModSide.Client)]
public class AudioEffectsSystem : ModSystem
{
    private static Action<SoundEffectInstance, float> highPassFilterAction;
    private static Action<SoundEffectInstance, float> lowPassFilterAction;
    private static Action<SoundEffectInstance, float> reverbAction;

    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported && ClientSideConfig.Instance.ToggleAudioEffects;
    }

    public override void OnModLoad() {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;

        reverbAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyReverb", flags).CreateDelegate<Action<SoundEffectInstance, float>>();
        lowPassFilterAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", flags).CreateDelegate<Action<SoundEffectInstance, float>>();
        highPassFilterAction = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyHighPassFilter", flags).CreateDelegate<Action<SoundEffectInstance, float>>();
    }
}