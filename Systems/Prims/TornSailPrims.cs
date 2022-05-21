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
    class TornSailPrims : Primitive
    {
        public TornSailPrims(Projectile projectile, Vector2 p1, Vector2 p2, float _offset, float _flowSpeed, bool _outline = false, float width = 1, float turnoff = 0.9f) : base(projectile)
        {
            BindableEntity = projectile;
            _width = width;
            _decayRate = turnoff;

            myCenterPoint = p1;
            distantCenterPoint = p2;

            outline = _outline;

            offset = _offset;

            flowSpeed = _flowSpeed;
        }

        float _decayRate;

        public override void SetDefaults()
        {
            Alpha = 1f;
            _cap = 300;

            behindTiles = false;
            ManualDraw = false;
            manualDraw = true;
            pixelated = true;
        }

        public Vector2 myCenterPoint;
        public Vector2 distantCenterPoint;

        bool outline = false;

        float speed = 20f;
        float count = 40;

        float flowSpeed = 1f;

        float offset;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            _points.Clear();

            if(!Main.gameInactive && !Main.gameMenu && !Main.gamePaused) distantCenterPoint += new Vector2(0, 0.25f * (float)Math.Sin((Main.GameUpdateCount / (speed))));

            for (int i = 1; i < count; i++)
            {
                if (i == 1)
                {
                    _points.Add(myCenterPoint);

                    continue;
                }

                Vector2 vec = Vector2.Lerp(myCenterPoint, distantCenterPoint, i / count) + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / speed) - (i / (4f * flowSpeed)) + offset) * 4f);

                //_points.Add(new Vector2(vec.X, vec.Y + (float)Math.Sin((Main.GameUpdateCount - i * 10) / 10f)));

                _points.Add(vec);
            }

            float widthVar;

            for (int i = 0; i < 40 - 2; i++)
            {
                widthVar = _width * (1 - (i / 60f));

                Color c = Color.Lerp(Color.Black, Color.White, (float)Math.Sin((Main.GameUpdateCount / speed) - ((i + 2) / (4f * flowSpeed)) + offset).PositiveSin());
                Color CBT = Color.Lerp(Color.Black, Color.White, (float)Math.Sin((Main.GameUpdateCount / speed) - ((i + 2) / (4f * flowSpeed)) + offset).PositiveSin());

                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                normal = new Vector2(0, -1);
                normalAhead = new Vector2(0, -1);

                Vector2 firstUp = _points[i] - normal * widthVar;
                Vector2 firstDown = _points[i] + normal * widthVar;

                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;


                AddVertex(firstDown, c * Alpha, new Vector2((i / _cap), 1));
                AddVertex(firstUp, c * Alpha, new Vector2((i / _cap), 0));
                AddVertex(secondDown, CBT * Alpha, new Vector2((i + 1) / _cap, 1));

                AddVertex(secondUp, CBT * Alpha, new Vector2((i + 1) / _cap, 0));
                AddVertex(secondDown, CBT * Alpha, new Vector2((i + 1) / _cap, 1));
                AddVertex(firstUp, c * Alpha, new Vector2((i / _cap), 0));
            }
        }


        public override void SetShaders()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, EEMod.SeafoamShader);

            if (outline)
            {
                EEMod.TornSailShader.Parameters["newColor5"].SetValue(new Vector4(88, 75, 65, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor4"].SetValue(new Vector4(88, 75, 65, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor3"].SetValue(new Vector4(114, 100, 73, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor2"].SetValue(new Vector4(140, 116, 84, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor1"].SetValue(new Vector4(171, 155, 120, 255) / 255f);
            }
            else
            {
                EEMod.TornSailShader.Parameters["newColor5"].SetValue(new Vector4(88, 75, 65, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor4"].SetValue(new Vector4(114, 100, 73, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor3"].SetValue(new Vector4(140, 116, 84, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor2"].SetValue(new Vector4(171, 155, 120, 255) / 255f);
                EEMod.TornSailShader.Parameters["newColor1"].SetValue(new Vector4(176, 168, 150, 255) / 255f);
            }

            EEMod.TornSailShader.Parameters["transformMatrix"].SetValue(view * projection);

            EEMod.TornSailShader.Parameters["lightColor"].SetValue(Lighting.GetColor((int)(myCenterPoint.X / 16f), (int)(myCenterPoint.Y / 16f)).ToVector4());

            EEMod.TornSailShader.Parameters["maskTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/ClothNoiseBoosted2").Value);

            //EEMod.TornSailShader.Parameters["transformMatrix"].SetValue(world * view * projection);

            if (vertices.Length == 0) return;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly); //new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.TornSailShader.CurrentTechnique.Passes)
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
            if ((!BindableEntity.active && BindableEntity != null) || BindableEntity == null || _destroyed)
            {
                OnDestroy();
            }
            else
            {

            }
        }

        public override void OnDestroy()
        {
            Dispose();
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
}