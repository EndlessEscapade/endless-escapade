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
using static Terraria.ModLoader.ModContent;
using System.Reflection;

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
        public static List<Trail> _trails = new List<Trail>();
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
            Dispose();
        }
        public Prims(Mod mod)
        {
            _trails = new List<Trail>();
            _effect = mod.GetEffect("Effects/trailShaders");
            _basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
            _basicEffect.VertexColorEnabled = true;
        }
        public void CreateTrail(Projectile projectile = null)
        {
            Trail newTrail = new Trail(new RoundCap(), new DefaultShader(), projectile);
            _trails.Add(newTrail);
        }
        public void Dispose()
        {
            for (int i = 0; i < _trails.Count; i++)
            {
                if (!_trails[i]._projectile.active)
                {
                    if (_trails[i]._projectile.type != ProjectileType<DalantiniumFan>() &&
                    _trails[i]._projectile.type != ProjectileType<DalantiniumFanAlt>())
                    {
                        _trails.RemoveAt(i);
                    }
                    if (_trails[i].lerper > 20 && _trails[i]._projectile.type == ProjectileType<DalantiniumFan>())
                    {
                        _trails.RemoveAt(i);
                    }
                    if (i >= 0 && i < _trails.Count)
                    {
                        if (_trails[i]._projectile.type == ProjectileType<DalantiniumFanAlt>())
                        {
                            if (_trails[i].lerper > 1000)
                            {
                                _trails.RemoveAt(i);
                            }
                        }
                    }
                }
            }
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
        public delegate void DrawPrimDelegate(int noOfPoints);
        public delegate void UpdatePrimDelegate();
        public static Type[] types => Assembly.GetExecutingAssembly().GetTypes();
        public class Trail
        {
            
            private ITrailShader _trailShader;
            public Projectile _projectile;
            public List<Vector2> _points = new List<Vector2>();
            public bool active;
            public int lerper;
            float DalCap;
            List<UpdatePrimDelegate> UpdateMethods = new List<UpdatePrimDelegate>();
            void LythenPrimUpdates()
            {
                if (_projectile.type == ProjectileType<LythenStaffProjectile>())
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
            void DalantiniumPrimUpdates()
            {
                if (_projectile.type == ProjectileType<DalantiniumFan>())
                {
                    DalCap = 10;
                    DalantiniumFan DF = (_projectile.modProjectile as DalantiniumFan);
                    lerper++;
                    _points.Add(DF.DrawPos);
                    active = true;
                    if (_points.Count > 10)
                    {
                        _points.RemoveAt(0);
                    }
                }
            }
            void DalantiniumAltPrimUpdates()
            {
                if (_projectile.type == ProjectileType<DalantiniumFanAlt>())
                {
                    DalCap = 10;
                    DalantiniumFanAlt DF = (_projectile.modProjectile as DalantiniumFanAlt);
                    lerper++;
                    _points.Add(_projectile.Center);
                    active = true;
                    if (_points.Count > DalCap)
                    {
                        _points.RemoveAt(0);
                    }
                }
            }
            public Trail(ITrailCap cap, ITrailShader shader, Projectile projectile)
            {
                _trailShader = shader;
                _projectile = projectile;
                active = true;
                UpdateMethods.Add(LythenPrimUpdates);
                UpdateMethods.Add(DalantiniumPrimUpdates);
                UpdateMethods.Add(DalantiniumAltPrimUpdates);
            }
            public void Update()
            {
                if (_projectile != null)
                {
                  foreach(UpdatePrimDelegate UPD in UpdateMethods)
                  {
                        UPD.Invoke();
                  }
                }
            }
            public void Draw(Effect effect, BasicEffect effect2, GraphicsDevice device)
            {
                //PREPARATION
                if (_points.Count <= 1) return;
                if (!active) return;
                int currentIndex = 0;
                VertexPositionColorTexture[] vertices;
                void AddVertex(Vector2 position, Color color, Vector2 uv)
                {
                    vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position.ForDraw(), 0f), color, uv);
                }
                void PrepareShader()
                {
                    int width = device.Viewport.Width;
                    int height = device.Viewport.Height;
                    Vector2 zoom = Main.GameViewMatrix.Zoom;
                    Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                    Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
                    effect.Parameters["WorldViewProjection"].SetValue(view * projection);
                    _trailShader.ApplyShader(effect, this, _points);
                }
                void PrepareBasicShader()
                {
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
                    }
                }
                //PRIM DELEGATES
                DrawPrimDelegate LythenPrims = (int noOfPoints) =>
                {
                    vertices = new VertexPositionColorTexture[noOfPoints];
                    AddVertex(_points[0], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    AddVertex(_points[1], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    AddVertex(_points[2], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    PrepareShader();
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints/3);
                };
                DrawPrimDelegate DalantiniumPrims = (int noOfPoints) =>
                {
                    vertices = new VertexPositionColorTexture[noOfPoints];
                    float width = 5;
                    float alphaValue = 0.2f;
                    for (int i = 0; i < _points.Count; i++)
                        {
                        if (i == 0)
                            {
                            Color c = Color.DarkRed;
                            Vector2 normalAhead = CurveNormal(_points, i + 1);
                                Vector2 secondUp = _points[i + 1] - normalAhead * width;
                                Vector2 secondDown = _points[i + 1] + normalAhead * width;
                                AddVertex(_points[i], c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                            }
                            else
                            {
                                
                                if (i != _points.Count - 1)
                                {
                                    Color c = Color.Red;
                                    Vector2 normal = CurveNormal(_points, i);
                                    Vector2 normalAhead = CurveNormal(_points, i + 1);
                                    float j = (_points.Count - (i * 0.9f)) / 10f;
                                    width *= (_points.Count - (i * 0.4f)) / 10f;
                                    Vector2 firstUp = _points[i] - normal * width;
                                    Vector2 firstDown = _points[i] + normal * width;
                                    Vector2 secondUp = _points[i + 1] - normalAhead * width;
                                    Vector2 secondDown = _points[i + 1] + normalAhead * width;

                                    AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    AddVertex(firstDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    

                                    AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                                    AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                                    AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                                }
                                else
                                {

                                }
                            }
                        }


                    PrepareBasicShader();
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                };
                DrawPrimDelegate DalantiniumAltPrims = (int noOfPoints) =>
                {
                    vertices = new VertexPositionColorTexture[noOfPoints];
                    float width = 6;
                    float alphaValue = 0.1f;
                    for (int i = 0; i < _points.Count; i++)
                    {
                        if (i == 0)
                        {
                            Color c = Color.DarkRed;
                            Vector2 normalAhead = CurveNormal(_points, i + 1);
                            Vector2 secondUp = _points[i + 1] - normalAhead * width;
                            Vector2 secondDown = _points[i + 1] + normalAhead * width;
                            AddVertex(_points[i], c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                            AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                            AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                        }
                        else
                        {
                            if (i != _points.Count - 1)
                            {
                                Color c = Color.Red;
                                Vector2 normal = CurveNormal(_points, i);
                                Vector2 normalAhead = CurveNormal(_points, i + 1);
                                float j = (DalCap + ((float)(Math.Sin(lerper/10f))*1) - i*0.05f) / DalCap;
                                width *= j;
                                Vector2 firstUp = _points[i] - normal * width;
                                Vector2 firstDown = _points[i] + normal * width;
                                Vector2 secondUp = _points[i + 1] - normalAhead * width;
                                Vector2 secondDown = _points[i + 1] + normalAhead * width;

                                AddVertex(firstUp, c * alphaValue, new Vector2(1));
                                AddVertex(secondDown, c * alphaValue, new Vector2(0));
                                AddVertex(firstDown, c * alphaValue, new Vector2(0));


                                AddVertex(secondUp, c * alphaValue, new Vector2(1));
                                AddVertex(secondDown, c * alphaValue, new Vector2(0));
                                AddVertex(firstUp, c * alphaValue, new Vector2(0));
                            }
                            else
                            {

                            }
                        }
                    }
                    PrepareBasicShader();
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                };
                if (_projectile != null)
                {
                    if(_projectile.type == ProjectileType<LythenStaffProjectile>())
                    {
                        LythenPrims.Invoke(3);
                    }
                    if (_projectile.type == ProjectileType<DalantiniumFan>())
                    {
                        DalantiniumPrims.Invoke(51);
                    }
                    if (_projectile.type == ProjectileType<DalantiniumFanAlt>())
                    {
                        DalantiniumAltPrims.Invoke((int)DalCap*6 - 9);
                    }
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


