using System.Collections.Generic;
using System.IO;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.IO;

public sealed class PrefabManager : ModSystem
{
    private static List<JToken> tokens = new();
    
    public override void Load() {
        LoadPrefabsFromMod(Mod);
    }

    public override void Unload() {
        tokens?.Clear();
        tokens = null;
    }

    public static void LoadPrefabsFromMod(Mod mod) {
        foreach (var fullFilePath in mod.GetFileNames()) {
            if (!fullFilePath.EndsWith(".prefab")) {
                continue;
            }

            using var stream = mod.GetFileStream(fullFilePath);
            using var reader = new StreamReader(stream);

            var hjson = reader.ReadToEnd();
            var json = HjsonValue.Parse(hjson).ToString(Stringify.Plain);
            
            foreach (var token in JToken.Parse(json)) {
                if (token is not JProperty property) {
                    continue;
                }

                tokens.Add(property.Value);
            }
        }
    }

    public static IEnumerable<T> EnumeratePrefabs<T>(string propertyName) {
        foreach (var token in tokens) {
            if (token[propertyName] is JObject jsonObject) {
                yield return jsonObject.ToObject<T>();
            }
        }
    }
}
