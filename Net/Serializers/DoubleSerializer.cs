using System.IO;

namespace EEMod.Net.Serializers
{
    public class DoubleSerializer : NetObjSerializer<double>
    {
        public override double Read(BinaryReader reader) => reader.ReadDouble();
        public override void Write(BinaryWriter writer, double value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
