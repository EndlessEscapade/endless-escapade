using System.IO;
using Microsoft.Xna.Framework;

namespace EEMod.Net.Serializers
{
    public class ByteSerializer : NetObjSerializer<byte>
    {
        public override byte Read(BinaryReader reader) => reader.ReadByte();
        public override void Write(BinaryWriter writer, byte value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class SByteSerializer : NetObjSerializer<sbyte>
    {
        public override sbyte Read(BinaryReader reader) => reader.ReadSByte();
        public override void Write(BinaryWriter writer, sbyte value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class Int16Serializer : NetObjSerializer<short>
    {
        public override short Read(BinaryReader reader) => reader.ReadInt16();
        public override void Write(BinaryWriter writer, short value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class UInt16Serializer : NetObjSerializer<ushort>
    {
        public override ushort Read(BinaryReader reader) => reader.ReadUInt16();
        public override void Write(BinaryWriter writer, ushort value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class Int32Serializer : NetObjSerializer<int>
    {
        public override int Read(BinaryReader reader) => reader.ReadInt32();
        public override void Write(BinaryWriter writer, int value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class UInt32Serializer : NetObjSerializer<uint>
    {
        public override uint Read(BinaryReader reader) => reader.ReadUInt32();
        public override void Write(BinaryWriter writer, uint value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class Int64Serializer : NetObjSerializer<long>
    {
        public override long Read(BinaryReader reader) => reader.ReadInt64();
        public override void Write(BinaryWriter writer, long value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class UInt64Serializer : NetObjSerializer<ulong>
    {
        public override ulong Read(BinaryReader reader) => reader.ReadUInt64();
        public override void Write(BinaryWriter writer, ulong value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class SingleSerializer : NetObjSerializer<float>
    {
        public override float Read(BinaryReader reader) => reader.ReadSingle();
        public override void Write(BinaryWriter writer, float value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class DoubleSerializer : NetObjSerializer<double>
    {
        public override double Read(BinaryReader reader) => reader.ReadDouble();
        public override void Write(BinaryWriter writer, double value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class CharSerializer : NetObjSerializer<char>
    {
        public override char Read(BinaryReader reader) => reader.ReadChar();
        public override void Write(BinaryWriter writer, char value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class DecimalSerializer : NetObjSerializer<decimal>
    {
        public override decimal Read(BinaryReader reader) => reader.ReadDecimal();
        public override void Write(BinaryWriter writer, decimal value) => writer.Write(value);
        public override SerializerPriority Priority => SerializerPriority.Low;
    }

    public class Vector2Serializer : NetObjSerializer<Vector2>
    {
        public override Vector2 Read(BinaryReader reader) => new Vector2(reader.ReadSingle(), reader.ReadSingle());

        public override void Write(BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }
    }

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
