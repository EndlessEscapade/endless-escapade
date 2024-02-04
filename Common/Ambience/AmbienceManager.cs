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
    private static List<AmbienceSound> Sounds = new();
    private static List<AmbienceLoop> Loops = new();
    
    public override void PostSetupContent() {
        Sounds = PrefabManager.EnumeratePrefabs<AmbienceSound>("AmbienceSound").ToList();
        Loops = PrefabManager.EnumeratePrefabs<AmbienceLoop>("AmbienceLoop").ToList();
    }

    public override void PostUpdateWorld() {
        UpdateSounds();
    }

    // TODO: Update loops and sounds based on active flags.
    private static void UpdateSounds() {
        for (var i = 0; i < Sounds.Count; i++) {
            var sound = Sounds[i];
            
            if (!SoundEngine.TryGetActiveSound(sound.SlotId, out var activeSound) && Main.rand.NextBool(sound.PlaybackChanceDenominator)) {
                sound.SlotId = SoundEngine.PlaySound(sound.Style);
            }
            else {
                activeSound.Volume = sound.Volume;
            }

            Sounds[i] = sound;
        }
    }
}
