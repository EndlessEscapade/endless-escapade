using System.Collections.Generic;
using System.IO;
using Hjson;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace EndlessEscapade.Core.IO;

public sealed class ContentSystem : ModSystem
{
    private static class ContentData<T>
    {
        private static T Read<T>(Mod mod, string path) {
            using var stream = mod.GetFileStream(path);
            using var reader = new StreamReader(stream);

            var hjson = reader.ReadToEnd();
            var json = HjsonValue.Parse(hjson).ToString(Stringify.Plain);

            var track = JsonConvert.DeserializeObject<T>(json);

            return track;
        }
    }
}
