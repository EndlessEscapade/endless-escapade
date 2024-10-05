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
        public static readonly Dictionary<string, T> Content = [];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name">The name of the content to retrieve.</param>
    /// <typeparam name="T">The type of the content to retrieve.</typeparam>
    /// <returns></returns>
    public static T Get<T>(string name) {
        return ContentData<T>.Content[name];
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">The type of the content collection to retrieve.</typeparam>
    /// <returns></returns>
    public static IEnumerable<T> Enumerate<T>() {
        return ContentData<T>.Content.Values;
    }

    internal static void LoadContent<T>(Mod mod, string path) {
        using var stream = mod.GetFileStream(path);
        using var reader = new StreamReader(stream);

        var hjson = reader.ReadToEnd();
        var json = HjsonValue.Parse(hjson).ToString(Stringify.Plain);

        var name = Path.GetFileNameWithoutExtension(path);
        var content = JsonConvert.DeserializeObject<T>(json);

        ContentData<T>.Content[name] = content;
    }
}
