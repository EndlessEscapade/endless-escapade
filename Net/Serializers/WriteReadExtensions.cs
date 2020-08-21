using System;
using System.IO;

namespace EEMod.Net.Serializers
{
    public static class WriteReadExtensions
    {
        public static void Write<T>(this BinaryWriter writer, T value)
        {
            var serializer = SerializersManager.GetTypeSerializer<T>();
            if (serializer is null)
                throw new ArgumentException($"Serializer for the type '{typeof(T).Name}' not found");
            serializer.Write(writer, value);
        }
        public static T Read<T>(this BinaryReader reader)
        {
            var serializer = SerializersManager.GetTypeSerializer<T>();
            if (serializer is null)
                throw new ArgumentException($"Serializer for the type '{typeof(T).Name}' not found");
            return serializer.Read(reader);
        }
    }
}
