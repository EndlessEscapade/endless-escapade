using System.Collections.Generic;
using System.IO;
using EndlessEscapade.Common.Boids;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Boids;

public sealed class BoidSystem : ModSystem
{
    public static List<BoidData> Data { get; private set; } = new();
    public static List<Boid> Boids { get; private set; } = new();

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
                if (token is not JProperty {
                        Value: var entityJson
                    } ||
                    entityJson["Boid"] is not JObject boidJson) {
                    continue;
                }

                Data.Add(boidJson.ToObject<BoidData>());
            }
        }
    }

    public override void Unload() {
        Data?.Clear();
        Data = null;

        Boids?.Clear();
        Boids = null;
    }

    public override void PreUpdateWorld() {
        foreach (var boid in Boids) {
            boid.Update();
        }
    }
}
