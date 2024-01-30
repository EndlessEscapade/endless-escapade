using System.IO;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

// TODO: I hate this. Switch to a data oriented approach later.
[Autoload(Side = ModSide.Client)]
public sealed class AmbienceSystem : ModSystem
{
    public override void Load() {
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

                Mod.Logger.Debug(property);
            }
        }
    }

    public override void PostUpdateEverything() {
        foreach (var track in ModContent.GetContent<AmbienceTrack>()) {
            track.Update();
        }
    }
}
