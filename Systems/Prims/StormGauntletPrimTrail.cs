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
    class StormGauntletPrimTrail : Primitive
    {
        public StormGauntletPrimTrail(Projectile projectile, float width = 1, float turnoff = 0.9f) : base(projectile)
        {
            BindableEntity = projectile;
            _width = width;
            _decayRate = turnoff;
        }

        float _decayRate;

        public override void SetDefaults()
        {
            Alpha = 1f;
            _cap = 200;

            behindTiles = false;
            ManualDraw = false;
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            _points.Clear();

            for (int i = 0; i <= 10; i++)
            {
                Vector2 oldCenter = BindableEntity.oldPosition + new Vector2(BindableEntity.width / 2f, BindableEntity.height / 2f);

                if (i >= 1 && i < 10)
                {
                    Vector2 bezierPoint = Vector2.Lerp(Vector2.Lerp(BindableEntity.Center + new Vector2(Main.rand.NextFloat(-7f, 7f),
                        Main.rand.NextFloat(-7f, 7f)), oldCenter + new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f)), (2 * i) / 10f),
                    Vector2.Lerp(oldCenter + new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f)),
                        Main.LocalPlayer.Center + new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f)), i / 10f), i / 10f);

                    _points.Add(bezierPoint);
                }
                else if (i == 0) _points.Add(BindableEntity.Center);
                else _points.Add(Main.LocalPlayer.Center);
            }

            float widthVar;

            float colorSin = (float)Math.Sin(_counter / 3f);

            for (int i = 0; i < _points.Count - 1; i++)
            {
                widthVar = _width + (float)(Math.Sin((i / 3f) + (Main.GameUpdateCount / 3f)) * 6f);

                Color c = Color.Lerp(Color.Gold, Color.Gold, colorSin);
                Color CBT = Color.Lerp(Color.Gold, Color.Gold, colorSin);

                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                float j = (_cap + ((float)(Math.Sin(_counter / 10f)) * 1) - i * 0.1f) / _cap;
                //widthVar *= j;

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
            int width = _device.Viewport.Width;
            int height = _device.Viewport.Height;

            Vector2 zoom = Main.GameViewMatrix.Zoom;
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            EEMod.LightningShader.Parameters["maskTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/LightningTexBoosted2").Value);
            EEMod.LightningShader.Parameters["newColor"].SetValue(new Vector4(Color.Gold.R, Color.Gold.G, Color.Gold.B, Color.Gold.A) / 255f);
            EEMod.LightningShader.CurrentTechnique.Passes[0].Apply();
        }

        public override void OnUpdate()
        {
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if ((!BindableEntity.active && BindableEntity != null) || _destroyed)
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