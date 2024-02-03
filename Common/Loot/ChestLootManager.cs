using System.Collections.Generic;
using System.IO;
using System.Linq;
using EndlessEscapade.Common.Ambience;
using EndlessEscapade.Utilities.Extensions;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Loot;

public sealed class ChestLootManager : ModSystem
{
    private static readonly List<ChestLoot> ChestLoot = new();

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

                if (value["ChestLoot"] is JObject chestLootJson) {
                    ChestLoot.Add(chestLootJson.ToObject<ChestLoot>());
                }
            }
        }
    }

    public override void PostWorldGen() {
        // Initial generation, ensures each item generates at least once.
        foreach (var loot in ChestLoot) { }

        // Extra generation, generates extra items based on their loot spawn rate.
        foreach (var loot in ChestLoot) { }
    }
}
