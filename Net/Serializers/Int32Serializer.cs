using System.IO;

namespace EEMod.Net.Serializers
{
    public class Int32Serializer : NetObjSerializer<int>
    {
        public override int Read(BinaryReader reader) => reader.ReadInt32();
        public override void Write(BinaryWriter writer, int value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
