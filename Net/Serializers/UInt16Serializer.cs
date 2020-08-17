using System.IO;

namespace EEMod.Net.Serializers
{
    public class UInt16Serializer : NetObjSerializer<ushort>
    {
        public override ushort Read(BinaryReader reader) => reader.ReadUInt16();
        public override void Write(BinaryWriter writer, ushort value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
