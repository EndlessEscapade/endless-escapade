using System.IO;

namespace EEMod.Net.Serializers
{
    public class UInt32Serializer : NetObjSerializer<uint>
    {
        public override uint Read(BinaryReader reader) => reader.ReadUInt32();
        public override void Write(BinaryWriter writer, uint value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
