using System.IO;
using Microsoft.Xna.Framework;

namespace EEMod.Net.Serializers
{
    public class Vector2Serializer : NetObjSerializer<Vector2>
    {
        public override Vector2 Read(BinaryReader reader) => new Vector2(reader.ReadSingle(), reader.ReadSingle());

        public override void Write(BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }
    }
}
