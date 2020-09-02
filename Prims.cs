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
using EEMod.Projectiles.Mage;

namespace EEMod
{
    public class Prims
    {
        //Global.graphics.GraphicsDevice for future reference

        public interface ITrailShader
        {
            string ShaderPass { get; }
            void ApplyShader<T>(Effect effect, T trail, List<Vector2> positions);
        }
        public void DrawTrails(SpriteBatch spriteBatch)
        {
            foreach (Trail trail in _trails)
            {
                trail.Draw(_effect, _basicEffect, Main.graphics.GraphicsDevice);
            }
            foreach (VerletBuffer verlet in _Verlets)
            {
                verlet.DrawCape(_basicEffect, Main.graphics.GraphicsDevice);
            }
        }
        public class DefaultShader : ITrailShader
        {
            public string ShaderPass => "DefaultPass";
            public void ApplyShader<T>(Effect effect, T trail, List<Vector2> positions)
            {
                foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                }
                effect.CurrentTechnique.Passes[ShaderPass].Apply();
            }
        }
        private Effect _effect;
        private List<Trail> _trails = new List<Trail>();
        private List<VerletBuffer> _Verlets = new List<VerletBuffer>();
        private static BasicEffect _basicEffect;
        public void UpdateTrails()
        {
            for (int i = 0; i < _trails.Count; i++)
            {
                Trail trail = _trails[i];
                trail.Update();
            }
            for (int i = 0; i < _Verlets.Count; i++)
            {
                VerletBuffer trail = _Verlets[i];
                trail.Update();
            }
        }
        public Prims(Mod mod)
        {
            _trails = new List<Trail>();
            _effect = mod.GetEffect("Effects/trailShaders");
            _basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
            _basicEffect.VertexColorEnabled = true;
        }
        public void CreateTrail(List<Vector2> gaming = null, ITrailShader shader = null, Projectile projectile = null)
        {
            Trail newTrail = new Trail(gaming, new RoundCap(), new DefaultShader(), projectile);
            _trails.Add(newTrail);
        }
        public void CreateVerlet()
        {
            VerletBuffer newTrail = new VerletBuffer();
            _Verlets.Add(newTrail);
        }
        public class VerletBuffer
        {
            bool active = false;
            public void Update()
            {
                if (lerpage >= 1)
                {
                    lerpage = 0;
                }
                lerpage += 0.01f;
            }
            float lerpage;
            public void DrawCape(BasicEffect effect2, GraphicsDevice device)
            {

                Vector2[] pointsArray = Main.LocalPlayer.GetModPlayer<EEPlayer>().arrayPoints;
                if (pointsArray.Length <= 1) return;
                if (!active) return;
                int currentIndex = 0;
                VertexPositionColor[] vertices = new VertexPositionColor[pointsArray.Length * 6 - 9];
                void AddVertex(Vector2 position, Color color)
                {
                    vertices[currentIndex++] = new VertexPositionColor(new Vector3(position.ForDraw(), 0f), color);
                }
                for (int i = 0; i < pointsArray.Length; i++)
                {
                    float j = (pointsArray.Length - i) / (float)pointsArray.Length;
                    float increment = i / (float)pointsArray.Length;
                    if (i == 0)
                    {
                        AddVertex(pointsArray[i], Color.Red);
                        AddVertex(pointsArray[i + 1] + CurveNormal(pointsArray.ToList(), i + 1) * -5 * (j - increment), Color.DarkRed);
                        AddVertex(pointsArray[i + 1] + CurveNormal(pointsArray.ToList(), i + 1) * 5 * (j - increment), Color.DarkRed);
                    }
                    if (i > 0 && i < pointsArray.Length - 1)
                    {
                        Vector2 normal = CurveNormal(pointsArray.ToList(), i);
                        Vector2 normalAhead = CurveNormal(pointsArray.ToList(), i + 1);

                        Vector2 firstUp = pointsArray[i] - normal * 5 * j;
                        Vector2 firstDown = pointsArray[i] + normal * 5 * j;
                        Vector2 secondUp = pointsArray[i + 1] - (normalAhead * 5 * ((pointsArray.Length) - (i + 1)) / pointsArray.Length);
                        Vector2 secondDown = pointsArray[i + 1] + (normalAhead * 5 * ((pointsArray.Length) - (i + 1)) / pointsArray.Length);
                        float varLerp = Math.Abs(lerpage - increment);

                        float varLerpAhead = Math.Abs(lerpage - ((i + 1) / (float)pointsArray.Length));
                        float addon = 0f;
                        Color Base = Color.Red;
                        Color Base2 = Color.DarkRed;
                        Color varColor = new Color(Base.R + (Base2.R - Base.R) * varLerp,
                                                   Base.G + (Base2.G - Base.G) * varLerp,
                                                   Base.B + (Base2.B - Base.B) * varLerp);
                        Color varColorAhead = new Color(Base.R + (Base2.R - Base.R) * varLerpAhead,
                                                        Base.G + (Base2.G - Base.G) * varLerpAhead,
                                                        Base.B + (Base2.B - Base.B) * varLerpAhead);
                        if (pointsArray[i].Y - pointsArray[i - 1].Y > 3)
                        {
                            if (pointsArray[i].Y > pointsArray[i - 1].Y && pointsArray[i].Y > pointsArray[i + 1].Y)
                            {
                                AddVertex(firstUp, varColorAhead);
                                AddVertex(secondUp, varColorAhead);
                                AddVertex(firstDown, varColorAhead);

                                AddVertex(secondUp, varColorAhead);
                                AddVertex(secondDown, varColorAhead);
                                AddVertex(firstDown, varColorAhead);
                                continue;
                            }
                            if (pointsArray[i].Y > pointsArray[i - 1].Y)
                            {
                                AddVertex(firstUp, Color.DarkRed);
                                AddVertex(secondUp, Color.DarkRed);
                                AddVertex(firstDown, Color.DarkRed);

                                AddVertex(secondUp, Color.DarkRed);
                                AddVertex(secondDown, Color.DarkRed);
                                AddVertex(firstDown, Color.DarkRed);
                            }
                            if (pointsArray[i].Y < pointsArray[i - 1].Y & pointsArray[i].Y < pointsArray[i + 1].Y)
                            {
                                AddVertex(firstUp, varColorAhead);
                                AddVertex(secondUp, varColorAhead);
                                AddVertex(firstDown, varColor);

                                AddVertex(secondUp, varColorAhead);
                                AddVertex(secondDown, varColorAhead);
                                AddVertex(firstDown, varColor);
                                continue;
                            }
                            if (pointsArray[i].Y <= pointsArray[i - 1].Y)
                            {
                                AddVertex(firstUp, Color.DarkRed);
                                AddVertex(secondUp, Color.DarkRed);
                                AddVertex(firstDown, Color.DarkRed);

                                AddVertex(secondUp, Color.DarkRed);
                                AddVertex(secondDown, Color.DarkRed);
                                AddVertex(firstDown, Color.DarkRed);
                            }
                        }
                        else
                        {
                            AddVertex(firstUp, Color.DarkRed);
                            AddVertex(secondUp, Color.DarkRed);
                            AddVertex(firstDown, Color.DarkRed);

                            AddVertex(secondUp, Color.DarkRed);
                            AddVertex(secondDown, Color.DarkRed);
                            AddVertex(firstDown, Color.DarkRed);
                        }
                    }
                }
                int width = device.Viewport.Width;
                int height = device.Viewport.Height;
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
                effect2.View = view;
                effect2.Projection = projection;
                foreach (EffectPass pass in effect2.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, pointsArray.Length * 2 - 3);
                }
            }
            //Helper methods
            private Vector2 CurveNormal(List<Vector2> points, int index)
            {
                if (points.Count == 1) return points[0];

                if (index == 0)
                {
                    return Clockwise90(Vector2.Normalize(points[1] - points[0]));
                }
                return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
            }

            private Vector2 Clockwise90(Vector2 vector)
            {
                return new Vector2(-vector.Y, vector.X);
            }
        }
        public class Trail
        {
            private ITrailShader _trailShader;
            private ITrailCap _trailCap;
            private Projectile _projectile;
            private List<Vector2> _points;
            private bool active;
            private int lerper;
            public Trail(List<Vector2> gaming, ITrailCap cap, ITrailShader shader, Projectile projectile)
            {
                _trailCap = cap;
                _points = gaming;
                _trailShader = shader;
                _projectile = projectile;
                active = true;
            }
            public void Update()
            {
                if (_projectile != null)
                {
                    if (_projectile.type == ModContent.ProjectileType<LythenStaffProjectile>())
                    {
                        lerper++;
                        LythenStaffProjectile LR = (_projectile.modProjectile as LythenStaffProjectile);
                        if (LR.positionOfOthers[0] != Vector2.Zero && LR.positionOfOthers[1] != Vector2.Zero)
                        {
                            _points = new List<Vector2>
                          {
                          _projectile.Center,
                          LR.positionOfOthers[0],
                          LR.positionOfOthers[1]
                          };
                        }
                        else
                        {
                            active = false;
                        }
                    }
                }
            }
            public void Draw(Effect effect, BasicEffect effect2, GraphicsDevice device)
            {
                if (_points.Count <= 1) return;
                if (!active) return;
                int currentIndex = 0;
                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[_points.Count];
                void AddVertex(Vector2 position, Color color, Vector2 uv)
                {
                    vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position.ForDraw(), 0f), color, uv);
                }
                if (_projectile != null)
                {
                    if (_projectile.type == ModContent.ProjectileType<LythenStaffProjectile>())
                    {
                        AddVertex(_points[0], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                        AddVertex(_points[1], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                        AddVertex(_points[2], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    }
                }
                int width = device.Viewport.Width;
                int height = device.Viewport.Height;
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
                effect.Parameters["WorldViewProjection"].SetValue(view * projection);
                _trailShader.ApplyShader(effect, this, _points);
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);
            }
            //Helper methods
            private Vector2 CurveNormal(List<Vector2> points, int index)
            {
                if (points.Count == 1) return points[0];

                if (index == 0)
                {
                    return Clockwise90(Vector2.Normalize(points[1] - points[0]));
                }
                if (index == points.Count - 1)
                {
                    return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
                }
                return Clockwise90(Vector2.Normalize(points[index + 1] - points[index - 1]));
            }

            private Vector2 Clockwise90(Vector2 vector)
            {
                return new Vector2(-vector.Y, vector.X);
            }
        }

    }
}


