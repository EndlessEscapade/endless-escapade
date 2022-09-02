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
using EEMod.NPCs.Goblins.Scrapwizard;

namespace EEMod.Prim
{
    public class ScrapwizardTendrilPrimTrail : Primitive
    {
        public ScrapwizardTendrilPrimTrail(Projectile projectile, NPC _targetEntity, float width = 1, float alpha = 1f) : base(projectile)
        {
            BindableEntity = projectile;
            _width = width;
            Alpha = alpha;

            targetEntity = _targetEntity;
        }

        public override void SetDefaults()
        {
            _cap = 300;

            behindTiles = false;
            pixelated = true;
            manualDraw = true;
        }

        public NPC targetEntity;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;
            float widthVar;

            float colorSin = (float)Math.Sin(_counter / 3f);
            {
                widthVar = 0;
                Color c1 = Color.Lerp(Color.White, Color.White, colorSin);

                Vector2 normalAhead = CurveNormal(_points, 1);
                Vector2 secondUp = _points[1] - normalAhead * widthVar;
                Vector2 secondDown = _points[1] + normalAhead * widthVar;
                Vector2 v = new Vector2((float)Math.Sin(_counter / 20f));

                AddVertex(_points[0], c1 * Alpha, v);
                AddVertex(secondUp, c1 * Alpha, v);
                AddVertex(secondDown, c1 * Alpha, v);
            }

            for (int i = 0; i < _points.Count - 1; i++)
            {
                widthVar = _width * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp((i / 10f) * (i / 10f), 0f, 1f) * MathHelper.Clamp(((_points.Count - i) / 10f) * ((_points.Count - i) / 10f), 0f, 1f);

                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                Vector2 firstUp = _points[i] - normal * widthVar;
                Vector2 firstDown = _points[i] + normal * widthVar;
                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                AddVertex(firstDown, Color.White * Alpha * (0.65f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.35f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2(((i + (_counter / 4f)) / (float)_cap) % 1, 1));
                AddVertex(firstUp, Color.White * Alpha * (0.65f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.35f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2(((i + (_counter / 4f)) / (float)_cap) % 1, 0));
                AddVertex(secondDown, Color.White * Alpha * (0.65f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.35f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 1));

                AddVertex(secondUp, Color.White * Alpha * (0.65f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.35f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 0));
                AddVertex(secondDown, Color.White * Alpha * (0.65f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.35f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 1));
                AddVertex(firstUp, Color.White * Alpha * (0.65f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.35f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) / (float)_cap)) % 1, 0));
            }
        }

        private float xOffset;

        public override void SetShaders()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default);

            EEMod.ShadowMagic.Parameters["maskTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/EnergyTrailAlt").Value);

            EEMod.ShadowMagic.Parameters["transformMatrix"].SetValue(view * projection);

            xOffset += (1 / -35f) * Alpha;

            EEMod.ShadowMagic.Parameters["xOffset"].SetValue(xOffset);

            EEMod.ShadowMagic.Parameters["myDist"].SetValue(3f);

            EEMod.ShadowMagic.Parameters["alpha"].SetValue(Alpha);

            //EEMod.ShadowMagic.Parameters["color1"].SetValue(new Vector4(94, 20, 174, 10) / 255f);
            //EEMod.ShadowMagic.Parameters["color2"].SetValue(new Vector4(178, 54, 212, 70) / 255f);
            //EEMod.ShadowMagic.Parameters["color3"].SetValue(new Vector4(255, 0, 133, 150) / 255f);
            //EEMod.ShadowMagic.Parameters["color4"].SetValue(new Vector4(250, 128, 16, 230) / 255f);
            //EEMod.ShadowMagic.Parameters["color5"].SetValue(new Vector4(225, 228, 159, 230) / 255f);

            EEMod.ShadowMagic.Parameters["color1"].SetValue(new Vector4(94, 20, 174, 10) / 255f);
            EEMod.ShadowMagic.Parameters["color2"].SetValue(new Vector4(178, 54, 212, 70) / 255f);
            EEMod.ShadowMagic.Parameters["color3"].SetValue(new Vector4(255, 100, 133, 150) / 255f);
            EEMod.ShadowMagic.Parameters["color4"].SetValue(new Vector4(250, 170, 70, 230) / 255f);
            EEMod.ShadowMagic.Parameters["color5"].SetValue(new Vector4(240, 235, 170, 230) / 255f);

            if (vertices.Length == 0) return;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.ShadowMagic.CurrentTechnique.Passes)
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

            Vector2 dir = Vector2.Normalize(BindableEntity.Center - (targetEntity.ModNPC as Scrapwizard).staffCastPos).RotatedBy(MathHelper.PiOver2);

            for (int i = 0; i < _cap; i++)
            {
                Vector2 lerp1 = (targetEntity.ModNPC as Scrapwizard).staffCastPos;
                Vector2 lerp2 = BindableEntity.Center;

                Vector2 lerpFinal = Vector2.Lerp(lerp1, lerp2, (i / (float)_cap)) +
                   dir *
                   (0.5f - 0.5f * (float)Math.Cos(((float)i / (float)_cap) * MathHelper.TwoPi)) *
                   ((float)Math.Sin((i / 24f) - 2f * (Main.GameUpdateCount / 15f)) / 2f + (float)Math.Sin((i / 24f) - 3f * (Main.GameUpdateCount / 16f)) / 2f)
                    * MathHelper.Clamp(0f, 1f, Vector2.Distance(BindableEntity.Center, (targetEntity.ModNPC as Scrapwizard).staffCastPos)) * 20f;

                _points.Add(lerpFinal);
            }

            if(_destroyed)
            {
                Alpha *= 0.92f;

                if (Alpha < 0.1f) Dispose();
            }
        }

        public override void OnDestroy()
        {
            _destroyed = true;
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
    
    public class ScrapwizardTendrilPrimTrail2 : Primitive
    {
        public ScrapwizardTendrilPrimTrail2(Projectile projectile, NPC _targetEntity, float width = 1) : base(projectile)
        {
            BindableEntity = projectile;
            _width = width;

            targetEntity = _targetEntity;
        }

        public override void SetDefaults()
        {
            Alpha = 1f;
            _cap = 300;

            behindTiles = true;
            pixelated = true;
            manualDraw = true;
        }

        public NPC targetEntity;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;
            float widthVar;

            float colorSin = (float)Math.Sin(_counter / 3f);
            {
                widthVar = 0;
                Color c1 = Color.Lerp(Color.White, Color.White, colorSin);

                Vector2 normalAhead = CurveNormal(_points, 1);
                Vector2 secondUp = _points[1] - normalAhead * widthVar;
                Vector2 secondDown = _points[1] + normalAhead * widthVar;
                Vector2 v = new Vector2((float)Math.Sin(_counter / 20f));

                AddVertex(_points[0], c1 * Alpha, v);
                AddVertex(secondUp, c1 * Alpha, v);
                AddVertex(secondDown, c1 * Alpha, v);
            }

            for (int i = 0; i < _points.Count - 1; i++)
            {
                widthVar = _width * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp((i / 10f) * (i / 10f), 0f, 1f) * MathHelper.Clamp(((_points.Count - i) / 10f) * ((_points.Count - i) / 10f), 0f, 1f);

                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                Vector2 firstUp = _points[i] - normal * widthVar;
                Vector2 firstDown = _points[i] + normal * widthVar;
                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                AddVertex(firstDown, Color.White * Alpha * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2(((i + (_counter / 4f)) / (float)_cap) % 1, 1));
                AddVertex(firstUp, Color.White * Alpha * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2(((i + (_counter / 4f)) / (float)_cap) % 1, 0));
                AddVertex(secondDown, Color.White * Alpha * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 1));

                AddVertex(secondUp, Color.White * Alpha * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 0));
                AddVertex(secondDown, Color.White * Alpha * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) + 1) / (float)_cap) % 1, 1));
                AddVertex(firstUp, Color.White * Alpha * (0.95f + (float)Math.Cos(((float)i / (float)_points.Count) * MathHelper.TwoPi) * 0.05f) * MathHelper.Clamp(_counter / 10f, 0f, 1f), new Vector2((((i + (_counter / 4f)) / (float)_cap)) % 1, 0));
            }
        }


        public override void SetShaders()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default);

            EEMod.ContinuousPrimTexShader.Parameters["maskTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/GlowingWeb").Value);

            EEMod.ContinuousPrimTexShader.Parameters["WorldViewProjection"].SetValue(view * projection);

            EEMod.ContinuousPrimTexShader.Parameters["darkColor"].SetValue(new Vector4(0, 0, 0, 0) / 255f);
            EEMod.ContinuousPrimTexShader.Parameters["lightColor"].SetValue(new Vector4(42, 11, 68, 128) / 255f);

            if (vertices.Length == 0) return;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.ContinuousPrimTexShader.CurrentTechnique.Passes)
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

            Vector2 dir = Vector2.Normalize(BindableEntity.Center - (targetEntity.ModNPC as Scrapwizard).staffCastPos).RotatedBy(MathHelper.PiOver2);

            for (int i = 0; i < _cap; i++)
            {
                Vector2 lerp1 = (targetEntity.ModNPC as Scrapwizard).staffCastPos;
                Vector2 lerp2 = BindableEntity.Center;

                Vector2 lerpFinal = Vector2.Lerp(lerp1, lerp2, (i / (float)_cap)) +
                   dir *
                   (0.5f - 0.5f * (float)Math.Cos(((float)i / (float)_cap) * MathHelper.TwoPi)) *
                   ((float)Math.Sin((i / 24f) - 2f * (Main.GameUpdateCount / 15f)) / 2f + (float)Math.Sin((i / 24f) - 3f * (Main.GameUpdateCount / 16f)) / 2f)
                    * MathHelper.Clamp(0f, 1f, Vector2.Distance(BindableEntity.Center, (targetEntity.ModNPC as Scrapwizard).staffCastPos)) * 20f;

                _points.Add(lerpFinal);
            }

            if (_destroyed)
            {
                Alpha *= 0.92f;

                if (Alpha < 0.1f) Dispose();
            }
        }

        public override void OnDestroy()
        {
            _destroyed = true;
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
}