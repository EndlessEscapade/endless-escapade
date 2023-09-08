using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Ambience;

// TODO: Implement support for conditional sounds.
public abstract class AmbienceTrack : ModType
{
    private SlotId loopSlot;
    private SlotId soundSlot;

    public SoundStyle Loop { get; protected set; }
    public SoundStyle Sounds { get; protected set; }
    
    private float volume;

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
            Volume += 0.005f;
        }
        else {
            Volume -= 0.005f;
        }
    }

    private void UpdateLoop() {
        var active = IsActive(Main.LocalPlayer);
        var exists = SoundEngine.TryGetActiveSound(loopSlot, out var sound);

        if (active && (!exists || sound == null)) {
            loopSlot = SoundEngine.PlaySound(Loop);
            return;
        }
        else if (exists && sound != null) {
            if (!active && Volume <= 0f) {
                sound.Stop();
                loopSlot = SlotId.Invalid;
                return;
            }
            
            sound.Volume = Volume;
        }
    }

    private void UpdateSounds() {
        if (!IsActive(Main.LocalPlayer) || SoundEngine.TryGetActiveSound(soundSlot, out _)) {
            return;
        }

        soundSlot = SlotId.Invalid;

        if (Main.rand.NextBool(Rate)) {
            soundSlot = SoundEngine.PlaySound(Sounds);
        }
    }

    protected abstract void Initialize();

    protected abstract bool IsActive(Player player);
}
