using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Utilities.Extensions;

public static class TagCompoundExtensions
{
    public static Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(this TagCompound tag, string keysTag, string valuesTag) {
        var keys = tag.GetList<TKey>(keysTag);
        var values = tag.GetList<TValue>(valuesTag);
        
        var zipped = keys.Zip(
            values,
            (x, y) => new {
                Key = x,
                Value = y
            }
        );

        return zipped.ToDictionary(x => x.Key, y => y.Value);
    }
}