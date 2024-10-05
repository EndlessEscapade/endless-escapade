using System;
using System.Runtime.CompilerServices;

namespace EndlessEscapade.Generators
{
    [InterpolatedStringHandler]
    internal readonly ref struct IndentedStringWriterInterpolatedStringHandler
    {
        private readonly IndentedStringWriter writer;

        public IndentedStringWriterInterpolatedStringHandler(int literalLength, int formattedCount, IndentedStringWriter writer)
        {
            this.writer = writer;
            writer.Builder.EnsureCapacity(writer.Builder.Capacity + literalLength + formattedCount * 2);
        }
        public readonly void AppendFormatted(byte value) => writer.Write(value);
        public readonly void AppendFormatted(sbyte value) => writer.Write(value);
        public readonly void AppendFormatted(short value) => writer.Write(value);
        public readonly void AppendFormatted(ushort value) => writer.Write(value);
        public readonly void AppendFormatted(int value) => writer.Write(value);
        public readonly void AppendFormatted(uint value) => writer.Write(value);
        public readonly void AppendFormatted(long value) => writer.Write(value);
        public readonly void AppendFormatted(ulong value) => writer.Write(value);
        public readonly void AppendFormatted(string value) => writer.Write(value);
        public readonly void AppendFormatted(float value) => writer.Write(value);
        public readonly void AppendFormatted(double value) => writer.Write(value);
        public readonly void AppendFormatted(char value) => writer.Write(value);
        public readonly void AppendFormatted(char[] values) => writer.Write(values);
        public readonly void AppendFormatted(ReadOnlySpan<char> value) => writer.Write(value);
        public readonly void AppendFormatted<T>(T value)
        {
            if (value != null)
                writer.Write(value.ToString());
        }
        public readonly void AppendLiteral(string s) => writer.Write(s);
    }

}
