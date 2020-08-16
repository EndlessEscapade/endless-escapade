using System.IO;

namespace EEMod.Net.Serializers
{
    public class SByteSerializer : NetObjSerializer<sbyte>
    {
        public override sbyte Read(BinaryReader reader) => reader.ReadSByte();
        public override void Write(BinaryWriter writer, sbyte value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
