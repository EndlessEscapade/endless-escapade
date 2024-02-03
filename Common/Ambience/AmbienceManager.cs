using System.Collections.Generic;
using System.IO;
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
    private static readonly List<AmbienceSound> Sounds = new();
    private static readonly List<AmbienceLoop> Loops = new();
    
    public override void PostSetupContent() {
        foreach (var fullFilePath in Mod.GetFileNames()) {
            if (!fullFilePath.EndsWith(".prefab")) {
                continue;
            }

            using var stream = Mod.GetFileStream(fullFilePath);
            using var reader = new StreamReader(stream);

            var hjson = reader.ReadToEnd();
            var json = HjsonValue.Parse(hjson).ToString(Stringify.Plain);

            foreach (var token in JToken.Parse(json)) {
                if (token is not JProperty property) {
                    continue;
                }

                var value = property.Value;

                if (value["AmbienceSound"] is JObject ambienceSoundJson) {
                    Sounds.Add(ambienceSoundJson.ToObject<AmbienceSound>());
                }

                if (value["AmbienceLoop"] is JObject ambienceLoopJson) {
                    Loops.Add(ambienceLoopJson.ToObject<AmbienceLoop>());
                }
            }
        }
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
