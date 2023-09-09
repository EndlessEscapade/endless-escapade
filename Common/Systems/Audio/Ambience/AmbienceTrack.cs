using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Ambience;

// TODO: Implement support for conditional sounds.
public abstract class AmbienceTrack : ModType
{
    public readonly struct AmbienceSoundData
    {
        public readonly SoundStyle Style;
        public readonly Func<Player, bool> Condition;

        public AmbienceSoundData(SoundStyle style, Func<Player, bool> condition) {
            Style = style;
            Condition = condition;
        }

        public AmbienceSoundData(SoundStyle style) : this(style, (x) => true) { }
    }
    
    private SlotId loopSlot;
    private SlotId soundSlot;

    private float volume;

    public List<AmbienceSoundData> Loops { get; protected set; }
    public List<AmbienceSoundData> Sounds { get; protected set; }

    public float Volume {
        get => volume;
        set => volume = MathHelper.Clamp(value, 0f, Main.ambientVolume);
    }

    protected virtual int Rate { get; set; } = 100;

    protected sealed override void Register() {
        Initialize();

        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    internal void Update() {
        UpdateVolume();
        
        UpdateLoop();
        UpdateSounds();
    }

    private void UpdateVolume() {
        var active = IsActive(Main.LocalPlayer);

        if (active) {
            Volume += 0.05f;
        }
        else {
            Volume -= 0.05f;
        }
    }

    private void UpdateLoop() {
        var active = IsActive(Main.LocalPlayer);
        var exists = SoundEngine.TryGetActiveSound(loopSlot, out var sound);
        
        foreach (var data in Loops) {
            if (!data.Style.IsLooped) {
                continue;
            }
            
            if (active && (!exists || sound == null)) {
                loopSlot = SoundEngine.PlaySound(data.Style);
                return;
            }

            if (exists && sound != null) {
                if (!active && Volume <= 0f) {
                    sound.Stop();
                    loopSlot = SlotId.Invalid;
                    return;
                }

                sound.Volume = Volume;
            }
        }
    }

    private void UpdateSounds() {
        if (!IsActive(Main.LocalPlayer) || SoundEngine.TryGetActiveSound(soundSlot, out _)) {
            return;
        }
        
        soundSlot = SlotId.Invalid;

        foreach (var data in Sounds) {
            if (!data.Style.IsLooped && data.Condition.Invoke(Main.LocalPlayer) && Main.rand.NextBool(Rate)) {
                soundSlot = SoundEngine.PlaySound(data.Style);
                break;
            }
        }
    }

    protected abstract void Initialize();

    protected abstract bool IsActive(Player player);
}
