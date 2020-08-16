using System.IO;

namespace EEMod.Net.Serializers
{
    public class Int64Serializer : NetObjSerializer<long>
    {
        public override long Read(BinaryReader reader) => reader.ReadInt64();
        public override void Write(BinaryWriter writer, long value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
