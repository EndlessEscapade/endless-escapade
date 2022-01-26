using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
#nullable enable

namespace EEMod
{
    public abstract class VertexBufferPool : IDisposable
    {
        private static VertexBufferPool _shared = new DefaultVertexBufferPool();
        public static VertexBufferPool Shared { get => _shared; }

        private bool disposed;
        public bool IsDisposed => disposed;
        public event EventHandler<EventArgs> Disposing = null!;

        public virtual VertexBuffer RentVertexBuffer(Type vertexType, int minVertexCount, BufferUsage usage)
        {
            return RentVertexBuffer(Helpers.VertexDeclarationFromType(vertexType), minVertexCount, usage);
        }

        public virtual DynamicVertexBuffer RentDynamicVertexBuffer(Type vertexType, int minVertexCount, BufferUsage usage)
        {
            return RentDynamicVertexBuffer(Helpers.VertexDeclarationFromType(vertexType), minVertexCount, usage);
        }

        public abstract VertexBuffer RentVertexBuffer(VertexDeclaration vertexDeclaration, int minVertexCount, BufferUsage usage);

        public abstract DynamicVertexBuffer RentDynamicVertexBuffer(VertexDeclaration vertexDeclaration, int minVertexCount, BufferUsage usage);

        public abstract void Return(VertexBuffer buffer);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            Disposing?.Invoke(this, EventArgs.Empty);
            disposed = true;
        }

        ~VertexBufferPool()
        {
            Dispose(false);
        }

        public class DefaultVertexBufferPool : VertexBufferPool
        {
            Dictionary<VertexBufferEntry, List<VertexBuffer>> buffers = new();

            public override DynamicVertexBuffer RentDynamicVertexBuffer(VertexDeclaration vertexDeclaration, int minVertexCount, BufferUsage usage)
            {
                return (DynamicVertexBuffer)Get(vertexDeclaration, minVertexCount, usage, true);
            }

            public override VertexBuffer RentVertexBuffer(VertexDeclaration vertexDeclaration, int minVertexCount, BufferUsage usage)
            {
                return Get(vertexDeclaration, minVertexCount, usage, false);
            }

            private VertexBuffer Get(VertexDeclaration vertexDeclaration, int minVertexCount, BufferUsage usage, bool dynamic)
            {
                CheckDisposed();
                VertexBufferEntry entry = new(vertexDeclaration, usage, dynamic);
                if (!buffers.TryGetValue(entry, out List<VertexBuffer>? bufferList) || bufferList == null)
                    buffers[entry] = bufferList = new();

                VertexBuffer? buffer = null;
                // read backwards so the most recent buffers are checked first
                for (int i = bufferList.Count - 1; i >= 0; i--)
                {
                    VertexBuffer bufferElem = bufferList[i];
                    if (bufferElem == null || bufferElem.IsDisposed)
                    {
                        // remove it quickly without having to shift elements
                        int last = bufferList.Count - 1;
                        bufferList[i] = bufferList[last];
                        bufferList.RemoveAt(last);
                        i++;
                        continue;
                    }

                    if (bufferElem.VertexCount >= minVertexCount)
                    {
                        buffer = bufferElem;
                        break;
                    }
                }

                buffer ??= Create(vertexDeclaration, minVertexCount, usage, dynamic);
                return buffer;
            }

            private static VertexBuffer Create(VertexDeclaration vertexDeclaration, int minVertexCount, BufferUsage bufferUsage, bool dynamic)
            {
                if (dynamic)
                    return new DynamicVertexBuffer(Main.graphics.GraphicsDevice, vertexDeclaration, minVertexCount, bufferUsage);
                return new VertexBuffer(Main.graphics.GraphicsDevice, vertexDeclaration, minVertexCount, bufferUsage);
            }

            public override void Return(VertexBuffer buffer)
            {
                CheckDisposed();
                if (buffer == null)
                    throw new ArgumentNullException(nameof(buffer));
                if (buffer.IsDisposed)
                    throw new ArgumentException("The buffer is disposed", nameof(buffer));

                VertexBufferEntry entry = new(buffer.VertexDeclaration, buffer.BufferUsage, Dynamic: buffer is DynamicVertexBuffer);
                if (!buffers.TryGetValue(entry, out List<VertexBuffer>? bufferList) || bufferList == null)
                    buffers[entry] = bufferList = new();

                bufferList.Add(buffer);
            }

            // the buffers that are left here are supposed to not be referenced anywhere else in order to be used somewhere else
            protected override void Dispose(bool disposing)
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        foreach (List<VertexBuffer> entryBuffers in buffers.Values)
                        {
                            foreach (VertexBuffer buffer in entryBuffers)
                            {
                                buffer?.Dispose();
                            }
                            entryBuffers?.Clear();
                        }
                        buffers.Clear();
                    }
                    buffers = null!;
                }
                base.Dispose(disposing);
            }

            private void CheckDisposed()
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(null);
            }

            private record struct VertexBufferEntry(VertexDeclaration VertexDeclaration, BufferUsage BufferUsage, bool Dynamic);
        }
    }
}
