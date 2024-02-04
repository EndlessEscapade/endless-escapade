using System.Collections.Generic;
using EndlessEscapade.Common.IO;
using EndlessEscapade.Common.Surroundings;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceManager : ModSystem
{
    private static List<IAmbienceTrack> tracks = new();
    
    private static List<AmbienceSound> sounds = new();
    private static List<AmbienceLoop> loops = new();

    public override void PostSetupContent() {
        sounds = new List<AmbienceSound>(PrefabManager.EnumeratePrefabs<AmbienceSound>("AmbienceSound"));
        loops = new List<AmbienceLoop>(PrefabManager.EnumeratePrefabs<AmbienceLoop>("AmbienceLoop"));
    }

    public override void Unload() {
        sounds?.Clear();
        sounds = null;

        loops?.Clear();
        loops = null;
    }

    public override void PostUpdateWorld() {
        UpdateSounds();
        UpdateLoops();
    }

    private static void UpdateSounds() {
        for (var i = 0; i < sounds.Count; i++) {
            var sound = sounds[i];

            var active = false;

            foreach (var flag in sound.Flags) {
                if (SurroundingsManager.TryGetSurrounding(flag, out var surrounding) && surrounding) {
                    active = true;
                    break;
                }
            }

            if (active) {
                sound.Volume += 0.01f;
            }
            else {
                sound.Volume -= 0.01f;
            }
            
            var playing = SoundEngine.TryGetActiveSound(sound.SlotId, out var activeSound);

            if (active && (!playing || activeSound == null) && Main.rand.NextBool(sound.PlaybackChanceDenominator)) {
                sound.SlotId = SoundEngine.PlaySound(sound.Style);
                sound.Volume = 0f;
            }
            else if (playing && activeSound != null) {
                if (!active && sound.Volume <= 0f) {
                    activeSound.Stop();
                    sound.SlotId = SlotId.Invalid;
                }
                else {
                    activeSound.Volume = sound.Volume;
                }
            }

            sounds[i] = sound;
        }
    }

    private static void UpdateLoops() {
        for (var i = 0; i < loops.Count; i++) {
            var loop = loops[i];

            var active = false;

            foreach (var flag in loop.Flags) {
                if (SurroundingsManager.TryGetSurrounding(flag, out var surrounding) && surrounding) {
                    active = true;
                    break;
                }
            }

            if (active) {
                loop.Volume += 0.01f;
            }
            else {
                loop.Volume -= 0.01f;
            }

            var playing = SoundEngine.TryGetActiveSound(loop.SlotId, out var activeSound);

            if (active && (!playing || activeSound == null)) {
                loop.SlotId = SoundEngine.PlaySound(loop.Style);
                loop.Volume = 0f;
            }
            else if (playing && activeSound != null) {
                if (!active && loop.Volume <= 0f) {
                    activeSound.Stop();
                    loop.SlotId = SlotId.Invalid;
                }
                else {
                    activeSound.Volume = loop.Volume;
                }
            }
            
            loops[i] = loop;
        }
    }
}
