using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;

namespace EEMod
{
    public static class Prims
    {
        public static void Draw(Effect effect, BasicEffect effect2, GraphicsDevice device, List<Vector2> _points)
        {
            if (_points.Count <= 1) return;

            float trailLength = 0f;
            for (int i = 1; i < _points.Count; i++)
            {
                trailLength += Vector2.Distance(_points[i - 1], _points[i]);
            }

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[(_points.Count - 1) * 6];

            for (int i = 1; i < _points.Count; i++)
            {
                
            }

            int width = device.Viewport.Width;
            int height = device.Viewport.Height;
            Vector2 zoom = Main.GameViewMatrix.Zoom;
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            effect.Parameters["WorldViewProjection"].SetValue(view * projection);
            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, (_points.Count - 1) * 2);
        }
    }
}
