using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience;

[Autoload(Side = ModSide.Client)]
public class SoundSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassDelegate = (Action<SoundEffectInstance, float>)Delegate.CreateDelegate(
        typeof(Action<SoundEffectInstance, float>),
        null,
        typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic)
    );
    
    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }
}