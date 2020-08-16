using System.IO;

namespace EEMod.Net.Serializers
{
    public class CharSerializer : NetObjSerializer<char>
    {
        public override char Read(BinaryReader reader) => reader.ReadChar();
        public override void Write(BinaryWriter writer, char value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
