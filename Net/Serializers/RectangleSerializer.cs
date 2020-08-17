using System.IO;
using Microsoft.Xna.Framework;

namespace EEMod.Net.Serializers
{
    public class RectangleSerializer : NetObjSerializer<Rectangle>
    {
        public override Rectangle Read(BinaryReader reader) => new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        public override void Write(BinaryWriter writer, Rectangle value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Width);
            writer.Write(value.Height);
        }
    }
}
