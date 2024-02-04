using System.Collections.Generic;
using System.IO;
using System.Linq;
using EndlessEscapade.Common.IO;
using EndlessEscapade.Common.Surroundings;
using Hjson;
using Newtonsoft.Json.Linq;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceManager : ModSystem
{
    private static List<AmbienceSound> sounds = new();
    private static List<AmbienceLoop> loops = new();
    
    public override void PostSetupContent() {
        sounds = PrefabManager.EnumeratePrefabs<AmbienceSound>("AmbienceSound").ToList();
        loops = PrefabManager.EnumeratePrefabs<AmbienceLoop>("AmbienceLoop").ToList();
    }

    public override void Unload() {
        sounds?.Clear();
        sounds = null;
        
        loops?.Clear();
        loops = null;
    }

    public override void PostUpdateWorld() {
        UpdateSounds();
    }

    // TODO: Update loops and sounds based on active flags.
    private static void UpdateSounds() {
        for (var i = 0; i < sounds.Count; i++) {
            var sound = sounds[i];
            
            if (!SoundEngine.TryGetActiveSound(sound.SlotId, out var activeSound) && Main.rand.NextBool(sound.PlaybackChanceDenominator)) {
                sound.SlotId = SoundEngine.PlaySound(sound.Style);
            }
            else {
                activeSound.Volume = sound.Volume;
            }

            sounds[i] = sound;
        }
    }
}
