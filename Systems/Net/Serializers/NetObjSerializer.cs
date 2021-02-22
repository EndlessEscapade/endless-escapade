using EEMod.Autoloading.AutoloadTypes;
using System;
using System.IO;

namespace EEMod.Net.Serializers
{
    public abstract class NetObjSerializer : IAutoloadType
    {
        public abstract void WriteObj(BinaryWriter writer, object obj);

        public abstract object ReadObj(BinaryReader reader);

        public virtual SerializerPriority Priority => SerializerPriority.Medium;

        public static NetObjSerializer<T> Get<T>() => SerializersManager.GetTypeSerializer<T>();
    }

    public abstract class NetObjSerializer<T> : NetObjSerializer
    {
        public abstract void Write(BinaryWriter writer, T value);

        public abstract T Read(BinaryReader reader);

        public sealed override void WriteObj(BinaryWriter writer, object obj) => Write(writer, obj is T v ? v : throw new ArgumentException($"The given object type does not match this serializer's target type", nameof(obj)));

        public sealed override object ReadObj(BinaryReader reader) => Read(reader);
    }
}