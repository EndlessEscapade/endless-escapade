using System.IO;
using Microsoft.Xna.Framework;

namespace EEMod.Net.Serializers
{
    public class Vector4Serializer : NetObjSerializer<Vector4>
    {
        public override Vector4 Read(BinaryReader reader) => new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        public override void Write(BinaryWriter writer, Vector4 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }
    }
}
