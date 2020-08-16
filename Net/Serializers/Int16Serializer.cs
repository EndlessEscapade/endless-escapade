using System.IO;

namespace EEMod.Net.Serializers
{
    public class Int16Serializer : NetObjSerializer<short>
    {
        public override short Read(BinaryReader reader) => reader.ReadInt16();
        public override void Write(BinaryWriter writer, short value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
