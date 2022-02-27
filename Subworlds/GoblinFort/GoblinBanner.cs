using EEMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using EEMod.Prim;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using EEMod.Extensions;

namespace EEMod.Subworlds.GoblinFort
{
    public class GoblinBanner : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banner");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 10000000;
            Projectile.hide = false;
        }

        public override void AI()
        {
            if(Projectile.ai[0] == 0)
            {
                PrimitiveSystem.primitives.CreateTrail(new GoblinBannerPrims(Projectile, true));
                PrimitiveSystem.primitives.CreateTrail(new GoblinBannerPrims(Projectile, false));

                Projectile.ai[0]++;
            }
        }
    }

    public class GoblinBannerPrims : Primitive
    {
        public GoblinBannerPrims(Entity projectile, bool outline) : base(projectile)
        {
            BindableEntity = projectile;

            _cap = 200;
            _width = 23;

            this.outline = outline;

            if (this.outline) color = new Color(54, 38, 52);
            else color = Color.White;
        }

        public bool outline;

        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = false;
            ManualDraw = false;
            manualDraw = true;
            pixelated = true;

            myShader = new BasicEffect(Main.graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
            };

            myShader.Projection = Matrix.CreateOrthographic(_device.Viewport.Width / 2, _device.Viewport.Height / 2, 0, 1000);
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;

            for (int i = 0; i < _points.Count() - 1; i++)
            {
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);


                Vector2 firstUp = _points[i] - normal * 23;
                Vector2 firstDown = _points[i] + normal * 23;

                Vector2 secondUp = _points[i + 1] - normalAhead * 23;
                Vector2 secondDown = _points[i + 1] + normalAhead * 23;

                Color color2 = new Color((color * colorVal).ToVector3());

                AddVertex(firstDown, color2, new Vector2(1, i / (float)(_points.Count() - 1)));
                AddVertex(firstUp, color2, new Vector2(0, i / (float)(_points.Count() - 1)));
                AddVertex(secondDown, color2, new Vector2(1, (i + 1) / (float)(_points.Count() - 1)));

                AddVertex(secondUp, color2, new Vector2(0, (i + 1) / (float)(_points.Count() - 1)));
                AddVertex(secondDown, color2, new Vector2(1, (i + 1) / (float)(_points.Count() - 1)));
                AddVertex(firstUp, color2, new Vector2(0, i / (float)(_points.Count() - 1)));
            }
        }

        public Color color;
        float colorVal;

        BasicEffect myShader;

        public override void SetShaders()
        {
            if (vertices.Length == 0) return;

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, default, SamplerState.PointClamp, default, default, myShader, Main.GameViewMatrix.ZoomMatrix);

            myShader.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            myShader.TextureEnabled = true;

            if (!outline)
                myShader.Texture = ModContent.Request<Texture2D>("EEMod/Subworlds/GoblinFort/GoblinBannerBanner").Value;
            else
                myShader.Texture = ModContent.Request<Texture2D>("EEMod/Subworlds/GoblinFort/GoblinBannerOutline").Value;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in myShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints / 3);

            VertexBufferPool.Shared.Return(buffer);
        }

        public int ticks;

        public override void OnUpdate()
        {
            Color lightColor = Lighting.GetColor((BindableEntity.Center / 16f).ToPoint());
            colorVal = ((lightColor.R + lightColor.G + lightColor.B) / 3f) / 255f;

            ticks++;

            _counter++;
            _noOfPoints = _points.Count() * 6;

            if ((!BindableEntity.active && BindableEntity != null) || BindableEntity == null || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                _points.Clear();

                for (int i = 0; i < 11; i++)
                {
                    Vector2 offset = Vector2.Zero;

                    if (outline) offset = new Vector2(0, 2);

                    Vector2 endpoint = Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 110).RotatedBy(Math.Sin(ticks / 45f).PositiveSin() * 0.15f), i / 11f);

                    if (i <= 1) _points.Add(offset + BindableEntity.Center + Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 110), i / 11f));
                    else _points.Add(offset + BindableEntity.Center + endpoint + (Vector2.UnitX * 3f * (float)Math.Sin((ticks / 30f) + (i / 1.5f))).RotatedBy((endpoint).ToRotation() + 1.57f));
                }
            }
        }

        public override void OnDestroy()
        {
            _points.Clear();

            Dispose();
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
}