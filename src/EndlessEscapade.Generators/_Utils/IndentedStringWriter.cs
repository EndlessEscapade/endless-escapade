using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace EndlessEscapade.Generators
{
    internal sealed class IndentedStringWriter
    {
        public const string DefaultIndentString = "    ";
        public readonly StringBuilder Builder;
        public readonly string IndentString;
        public int Indent;

        private bool tabsPending;

        public IndentedStringWriter()
        {
            Builder = new StringBuilder();
            Indent = 0;
            IndentString = DefaultIndentString;
            tabsPending = false;
        }
        public IndentedStringWriter(int capacity)
        {
            Builder = new StringBuilder(capacity);
            Indent = 0;
            IndentString = DefaultIndentString;
            tabsPending = false;
        }
        public IndentedStringWriter(StringBuilder builder)
        {
            Builder = builder;
            Indent = 0;
            IndentString = DefaultIndentString;
            tabsPending = false;
        }

        #region Write methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(byte value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(sbyte value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(short value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(ushort value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(int value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(uint value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(long value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(ulong value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(float value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(double value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(char value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(string value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(ReadOnlySpan<char> values)
        {
            if (tabsPending)
                WriteTabs();
            for (int i = 0; i < values.Length; i++)
                Builder.Append(values[i]);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(char[] values)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(values);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(char[] values, int start, int count)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(values, start, count);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write([InterpolatedStringHandlerArgument("")] ref IndentedStringWriterInterpolatedStringHandler handler)
        {
            tabsPending = false;
            return this;
        }
        #endregion // Write methods

        private void WriteTabs()
        {
            if (tabsPending)
            {
                for (int i = 0; i < Indent; i++)
                    Builder.Append(IndentString);
                tabsPending = false;
            }
        }

        public override string ToString()
        {
            return Builder.ToString();
        }
    }

}
