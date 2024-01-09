using System;
using EndlessEscapade.Common.EC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EndlessEscapade.Common.IO;

public sealed class EntityJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Entity);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        throw new NotImplementedException();
    }
}