using System.IO;

namespace EEMod.Net.Serializers
{
    public class DecimalSerializer : NetObjSerializer<decimal>
    {
        public override decimal Read(BinaryReader reader) => reader.ReadDecimal();
        public override void Write(BinaryWriter writer, decimal value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
