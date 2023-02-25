using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public abstract class AmbienceTrack : ModType
{
    public SoundStyle LoopSoundStyle { get; protected set; }
    public SlotId LoopSoundSlot { get; protected set; }

    public float Volume { get; protected set; }

    public abstract bool Condition { get; }

    public abstract void Initialize();
    
    protected sealed override void Register() {
        Initialize();

        ModTypeLookup<AmbienceTrack>.Register(this);
    }
    
    public void Update() {
        UpdateVolume();
        UpdateLoop();
    }

    private void UpdateLoop() {
        ActiveSound? activeSound = null;
        bool isPlaying = TryGetActiveSound(out activeSound, false);
        
        if (!isPlaying && Condition) { 
            isPlaying = TryGetActiveSound(out activeSound, true);
        }
        else if (isPlaying) {
            if (Volume <= 0f) {
                activeSound.Volume = 0f;
                activeSound.Stop();
                return;
            }
            
            activeSound.Volume = Volume;
        }
    }
    
    private void UpdateVolume() {
        const float fadeRate = 0.005f;
        
        if (Condition) {
            Volume += fadeRate;
        }
        else if (!Condition) {
            Volume -= fadeRate;
        }

        Volume = MathHelper.Clamp(Volume, 0f, Main.ambientVolume);
    }
    
    private bool TryReplaySound() {
        if (LoopSoundSlot.IsValid && SoundEngine.TryGetActiveSound(LoopSoundSlot, out ActiveSound? activeSound)) { 
            SoundEffectInstance instance = activeSound.Sound;
            
            if (instance != null && instance.State != SoundState.Stopped) {
                activeSound.Stop();
            }
        }
        
        LoopSoundSlot = SoundEngine.PlaySound(LoopSoundStyle);
        
        return LoopSoundSlot.IsValid;
    }
    
    private bool TryGetActiveSound([NotNullWhen(true)] out ActiveSound? result, bool tryPlaying) {
        if (!LoopSoundSlot.IsValid || !SoundEngine.TryGetActiveSound(LoopSoundSlot, out result) && tryPlaying) {
            TryReplaySound();
            
            return SoundEngine.TryGetActiveSound(LoopSoundSlot, out result);
        }
        
        return result != null;
    }
}