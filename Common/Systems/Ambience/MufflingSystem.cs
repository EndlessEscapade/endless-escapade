using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience;

[Autoload(Side = ModSide.Client)]
public class MufflingSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassDelegate = (Action<SoundEffectInstance, float>)Delegate.CreateDelegate(
        typeof(Action<SoundEffectInstance, float>),
        null,
        typeof(SoundEffectInstance).GetMethod("INTERNAL_applyHighPassFilter", BindingFlags.Instance | BindingFlags.NonPublic)
    );
    
    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }

    public override void PostUpdateEverything() {
        On_SoundPlayer.Update += On_SoundPlayerOnUpdate;
    }

    private void On_SoundPlayerOnUpdate(On_SoundPlayer.orig_Update orig, SoundPlayer self) {
        var field = typeof(SoundPlayer).GetField("_trackedSounds", BindingFlags.Instance | BindingFlags.NonPublic);
        var value = (SlotVector<ActiveSound>)field.GetValue(self);

        foreach (var item in value) {
            if (item.Value.IsPlaying && !item.Value.Sound.IsDisposed) {
                lowPassDelegate.Invoke(item.Value.Sound, 1f);
            }
        }
        
        orig(self);
    }
}
