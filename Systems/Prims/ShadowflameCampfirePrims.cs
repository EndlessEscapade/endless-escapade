using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System;
using EEMod.Effects;
using EEMod.Items.Weapons.Mage;
using static Terraria.ModLoader.ModContent;
using System.Reflection;
using EEMod.Items.Weapons.Melee;

namespace EEMod.Prim
{
    class ShadowflameCampfirePrims : Primitive
    {
        public bool _additive;

        public Vector2 startPoint;
        public Vector2 controlPoint;
        public Vector2 endPoint;

        public float rot;

        public ShadowflameCampfirePrims(Color _color, Vector2 start, Vector2 cp1, Vector2 end, int width = 40, int cap = 10, bool additive = false, float sineValue = 3f) : base(null)
        {
            startPoint = start;
            controlPoint = cp1;
            endPoint = end;

            _width = width;
            color = _color;
            _cap = cap;

            sinValue = sineValue;

            _additive = additive;

            if (_additive) Alpha = 0.2f;
            else Alpha = 0.8f;
        }

        public float sinValue;

        private Color color;
        public override void SetDefaults()
        {
            behindTiles = true;
            ManualDraw = false;
            pixelated = true;
            manualDraw = true;
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            //if (_noOfPoints <= 1) return; 

            //float colorSin = (float)Math.Sin(_counter / 3f);
            //Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            //float widthVar = (float)Math.Sqrt(_points.Count) * _width;
            //DrawBasicTrail(c1, widthVar);*/

            if (_noOfPoints <= 1 || _points.Count() <= 1) return;
            float widthVar;

            float colorSin = (float)Math.Sin(_counter / 3f);
            {
                widthVar = 0;
                Color c1 = Color.Lerp(Color.White, color, colorSin);

                Vector2 normalAhead = CurveNormal(_points, 1);
                Vector2 secondUp = _points[1] - normalAhead * widthVar;
                Vector2 secondDown = _points[1] + normalAhead * widthVar;
                Vector2 v = new Vector2((float)Math.Sin(_counter / 20f));

                AddVertex(_points[0], c1 * Alpha, v);
                AddVertex(secondUp, c1 * Alpha, v);
                AddVertex(secondDown, c1 * Alpha, v);
            }

            for (int i = 1; i < _points.Count - 1; i++)
            {
                widthVar = ((i) / (float)_points.Count) * _width;

                Vector2 normal = -Vector2.UnitX.RotatedBy(rot / 1.5f);
                Vector2 normalAhead = -Vector2.UnitX.RotatedBy(rot / 1.5f);

                float j = (_cap + ((float)(Math.Sin(_counter / 10f)) * 1) - i * 0.1f) / _cap;
                widthVar *= j;

                Vector2 firstUp = _points[i] - normal * widthVar;
                Vector2 firstDown = _points[i] + normal * widthVar;
                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                AddVertex(firstDown, color * Alpha, new Vector2(((i + (_counter / 4f)) / (float)_cap) % 1, 1));
                AddVertex(firstUp, color * Alpha, new Vector2(((i + (_counter / 4f)) / (float)_cap) % 1, 0));
                AddVertex(secondDown, color * Alpha, new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 1));

                AddVertex(secondUp, color * Alpha, new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 0));
                AddVertex(secondDown, color * Alpha, new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 1));
                AddVertex(firstUp, color * Alpha, new Vector2((((i + (_counter / 4f)) / (float)_cap)) % 1, 0));
            }
        }

        public override void SetShaders()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, EEMod.TornSailShader, Main.GameViewMatrix.ZoomMatrix);

            if (_additive)
                EEMod.LightningShader.Parameters["maskTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/GlowingWeb").Value);
            else
                EEMod.LightningShader.Parameters["maskTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/FlameTrailBoosted").Value);

            EEMod.LightningShader.Parameters["newColor"].SetValue(new Vector4(color.R, color.G, color.B, color.A) / 255f);

            EEMod.LightningShader.Parameters["transformMatrix"].SetValue(view * projection);

            if (vertices.Length == 0) return;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.LightningShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            if (_noOfPoints >= 1)
            {
                _device.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints / 3);
            }

            VertexBufferPool.Shared.Return(buffer);
        }

        public override void OnUpdate()
        {
            _counter++;

            _noOfPoints = _points.Count() * 6;

            _points.Clear();

            for (int i = 0; i < _cap; i++)
            {
                Vector2 lerp1 = Vector2.Lerp(startPoint, controlPoint, 1 - (i / (float)_cap));
                Vector2 lerp2 = Vector2.Lerp(controlPoint, endPoint, 1 - (i / (float)_cap));

                Vector2 lerpFinal = Vector2.Lerp(lerp1, lerp2, 1 - (i / (float)_cap));

                _points.Add(lerpFinal + (Vector2.UnitX.RotatedBy(rot) * sinValue * (float)Math.Sin((_counter / 60f) + (i / 4f))));
            }
        }

        public override void OnDestroy()
        {
            _destroyed = true;
            _width *= 0.9f;
            if (_width < 0.05f)
            {
                Dispose();
            }
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
}