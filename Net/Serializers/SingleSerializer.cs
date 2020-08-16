using System.IO;

namespace EEMod.Net.Serializers
{
    public class SingleSerializer : NetObjSerializer<float>
    {
        public override float Read(BinaryReader reader) => reader.ReadSingle();
        public override void Write(BinaryWriter writer, float value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }
}
