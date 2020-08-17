using System.IO;

namespace EEMod.Net.Serializers
{
    public class UInt64Serializer : NetObjSerializer<ulong>
    {
        public override ulong Read(BinaryReader reader) => reader.ReadUInt64();
        public override void Write(BinaryWriter writer, ulong value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
