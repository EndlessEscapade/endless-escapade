using System.IO;

namespace EEMod.Net.Serializers
{
    public class ByteSerializer : NetObjSerializer<byte>
    {
        public override byte Read(BinaryReader reader) => reader.ReadByte();
        public override void Write(BinaryWriter writer, byte value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
