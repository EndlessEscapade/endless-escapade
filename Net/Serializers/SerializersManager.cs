using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EEMod.Autoloading;
using EEMod.Autoloading.AutoloadTypes;
using EEMod.Extensions;

namespace EEMod.Net.Serializers
{
    public class SerializersManager : AutoloadTypeManager<NetObjSerializer>
    {
        [FieldInit]
        private static ConcurrentDictionary<Type, SerializerInfo> serializers = new ConcurrentDictionary<Type, SerializerInfo>();

        public override void CreateInstance(Type type)
        {
            if (type.CouldBeInstantiated() && type.IsSubclassOfGeneric(typeof(NetObjSerializer<>), out Type typ))
            {
                Type serializingTargetType = typ.GenericTypeArguments[0]; // NetObjSerializer<thetype>
                if (type.TryCreateInstance(out NetObjSerializer serializer))
                    AddSerializer(serializingTargetType, serializer, serializer.Priority);
            }
        }

        public static void AddSerializer(Type fortype, NetObjSerializer serializer, SerializerPriority priority = SerializerPriority.Medium)
        {
            if (serializers.TryGetValue(fortype, out var existingSerializer))
                existingSerializer.AddSerializer(priority, serializer);
            else
                serializers[fortype] = new SerializerInfo(serializer, priority);
        }

        public static void AddSerializer<T>(NetObjSerializer<T> serializer, SerializerPriority priority) => AddSerializer(typeof(T), serializer, priority);

        public static NetObjSerializer GetTypeSerializer(Type fortype) => serializers.TryGetValue(fortype, out var serializer) ? serializer.GetHighestPrioritySerializer() : null;
        public static NetObjSerializer<T> GetTypeSerializer<T>() => (NetObjSerializer<T>)GetTypeSerializer(typeof(T));

        class SerializerInfo
        {
            Dictionary<SerializerPriority, NetObjSerializer> serializers;
            private NetObjSerializer _cachedHighest;
            public NetObjSerializer GetHighestPrioritySerializer() => _cachedHighest;
            public void AddSerializer(SerializerPriority priority, NetObjSerializer serializer)
            {
                serializers.Add(priority, serializer);
                foreach (var s in serializers)
                {
                    if (s.Key > priority)
                    {
                        priority = s.Key;
                        serializer = s.Value;
                    }
                }
                _cachedHighest = serializer;
            }
            public SerializerInfo(NetObjSerializer defaultSerializer, SerializerPriority priority = SerializerPriority.Medium)
            {
                serializers = new Dictionary<SerializerPriority, NetObjSerializer>();
                serializers.Add(priority, defaultSerializer);
                _cachedHighest = defaultSerializer;
            }
        }
    }
}
