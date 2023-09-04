using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Ambience;

public abstract class AmbienceTrack : ModType
{
    private SlotId soundSlot;
    private SlotId loopSlot;
    
    protected readonly List<SoundData> Sounds = new List<SoundData>();

    protected sealed override void Register() {
        Initialize();
    
        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    protected abstract void Initialize();

    protected void RegisterLoop(string path) {
        
    }

    protected void RegisterSound(string path, int rate, int variants = 1) {
        Sounds.Add(new SoundData(path, rate, variants));
    } 
    
    public void Update() {
        if (SoundEngine.TryGetActiveSound(soundSlot, out _)) {
            return;
        }
        else {
            soundSlot = SlotId.Invalid;
        }
        
        foreach (var data in Sounds) {
            if (Main.rand.NextBool(data.Rate)) {
                soundSlot = SoundEngine.PlaySound(in data.Style);
                break;
            }
        }
    }

    protected record struct SoundData(string Path, int Rate, int Variants = 1)
    {
        public readonly SoundStyle Style = new SoundStyle($"{nameof(EndlessEscapade)}/{Path}", Variants, SoundType.Ambient);
    }
}
