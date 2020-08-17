using System.IO;
using Microsoft.Xna.Framework;

namespace EEMod.Net.Serializers
{
    public class Vector3Serializer : NetObjSerializer<Vector3>
    {
        public override Vector3 Read(BinaryReader reader) => new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        public override void Write(BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
        }
    }
}
