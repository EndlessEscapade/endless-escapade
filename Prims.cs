using EEMod.Effects;
using EEMod.Extensions;
using EEMod.Projectiles.Mage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod
{
    public class Prims
    {
        private readonly Effect _effect;
        private readonly List<VerletBuffer> _Verlets = new List<VerletBuffer>();

        private static BasicEffect _basicEffect;

        public static List<Trail> trails = new List<Trail>();

        //Global.graphics.GraphicsDevice for future reference
        public interface ITrailShader
        {
            string ShaderPass { get; }

            void ApplyShader<T>(Effect effect, T trail, List<Vector2> positions);
        }

        public void DrawTrails(SpriteBatch spriteBatch)
        {
            foreach (Trail trail in trails)
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

        public void UpdateTrails()
        {
            for (int i = 0; i < trails.Count; i++)
            {
                Trail trail = trails[i];

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
            trails = new List<Trail>();
            _effect = mod.GetEffect("Effects/trailShaders");
            _basicEffect = new BasicEffect(Main.graphics.GraphicsDevice)
            {
                VertexColorEnabled = true
            };
        }

        public void CreateTrail(Projectile projectile = null) => trails.Add(new Trail(new RoundCap(), new DefaultShader(), projectile));

        public void Dispose()
        {
            for (int i = 0; i < trails.Count; i++)
            {
                if (!trails[i].projectile.active)
                {
                    if (trails[i].projectile.type != ProjectileType<DalantiniumFan>() && trails[i].projectile.type != ProjectileType<DalantiniumFanAlt>())
                    {
                        trails.RemoveAt(i);
                    }

                    if (trails[i].lerper > 20 && trails[i].projectile.type == ProjectileType<DalantiniumFan>())
                    {
                        trails.RemoveAt(i);
                    }

                    if (i >= 0 && i < trails.Count)
                    {
                        if (trails[i].projectile.type == ProjectileType<DalantiniumFanAlt>())
                        {
                            if (trails[i].lerper > 165)
                            {
                                trails[i].points.Clear();
                                trails.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }

        public void CreateVerlet() => _Verlets.Add(new VerletBuffer());

        public class VerletBuffer
        {
            private readonly bool _active = false; // Marked as readonly because you guys did nothing with it :KEKW:
            private float _lerpage;

            public void Update()
            {
                if (_lerpage >= 1)
                {
                    _lerpage = 0;
                }

                _lerpage += 0.01f;
            }

            public void DrawCape(BasicEffect effect2, GraphicsDevice device)
            {
                Vector2[] pointsArray = Main.LocalPlayer.GetModPlayer<EEPlayer>().arrayPoints;

                if (pointsArray.Length > 1 && _active)
                {
                    int currentIndex = 0;
                    VertexPositionColor[] vertices = new VertexPositionColor[pointsArray.Length * 6 - 9];

                    void AddVertex(Vector2 position, Color color) => vertices[currentIndex++] = new VertexPositionColor(new Vector3(position.ForDraw(), 0f), color);

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
                            Vector2 secondUp = pointsArray[i + 1] - (normalAhead * 5 * (pointsArray.Length - (i + 1)) / pointsArray.Length);
                            Vector2 secondDown = pointsArray[i + 1] + (normalAhead * 5 * (pointsArray.Length - (i + 1)) / pointsArray.Length);
                            float varLerp = Math.Abs(_lerpage - increment);

                            float varLerpAhead = Math.Abs(_lerpage - ((i + 1) / (float)pointsArray.Length));
                            Color Base = Color.Red;
                            Color Base2 = Color.DarkRed;
                            Color varColor = new Color(Base.R + (Base2.R - Base.R) * varLerp, Base.G + (Base2.G - Base.G) * varLerp, Base.B + (Base2.B - Base.B) * varLerp);
                            Color varColorAhead = new Color(Base.R + (Base2.R - Base.R) * varLerpAhead, Base.G + (Base2.G - Base.G) * varLerpAhead, Base.B + (Base2.B - Base.B) * varLerpAhead);

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
            }

            //Helper methods
            private Vector2 CurveNormal(List<Vector2> points, int index)
            {
                if (points.Count == 1)
                {
                    return points[0];
                }

                if (index == 0)
                {
                    return Clockwise90(Vector2.Normalize(points[1] - points[0]));
                }

                return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
            }

            private Vector2 Clockwise90(Vector2 vector) => new Vector2(-vector.Y, vector.X);
        }

        public delegate void DrawPrimDelegate(int noOfPoints);

        public delegate void UpdatePrimDelegate();

        public static Type[] types => Assembly.GetExecutingAssembly().GetTypes();

        public class Trail
        {
            private readonly ITrailShader _trailShader;
            private float _dalCap;
            private readonly List<UpdatePrimDelegate> _updateMethods = new List<UpdatePrimDelegate>();

            public Projectile projectile;
            public List<Vector2> points = new List<Vector2>();
            public bool active;
            public int lerper;

            private void LythenPrimUpdates()
            {
                if (projectile.type == ProjectileType<LythenStaffProjectile>())
                {
                    lerper++;

                    LythenStaffProjectile LR = projectile.modProjectile as LythenStaffProjectile;
                    if (LR.positionOfOthers[0] != Vector2.Zero && LR.positionOfOthers[1] != Vector2.Zero)
                    {
                        points = new List<Vector2>
                        {
                            projectile.Center,
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

            private void DalantiniumPrimUpdates()
            {
                if (projectile.type == ProjectileType<DalantiniumFan>())
                {
                    DalantiniumFan DF = (DalantiniumFan)projectile.modProjectile;

                    _dalCap = 10;
                    lerper++;
                    active = true;

                    points.Add(DF.DrawPos);

                    if (points.Count > 10)
                    {
                        points.RemoveAt(0);
                    }
                }
            }

            private void DalantiniumAltPrimUpdates()
            {
                if (projectile.type == ProjectileType<DalantiniumFanAlt>())
                {
                    _dalCap = 20;
                    lerper++;
                    points.Add(projectile.Center);
                    active = true;

                    if (points.Count > _dalCap)
                    {
                        points.RemoveAt(0);
                    }
                }
            }

            public Trail(ITrailCap cap, ITrailShader shader, Projectile projectile)
            {
                _trailShader = shader;
                this.projectile = projectile;
                active = true;

                _updateMethods.Add(LythenPrimUpdates);
                _updateMethods.Add(DalantiniumPrimUpdates);
                _updateMethods.Add(DalantiniumAltPrimUpdates);
            }

            public void Update()
            {
                if (projectile != null)
                {
                    foreach (UpdatePrimDelegate UPD in _updateMethods)
                    {
                        UPD.Invoke();
                    }
                }
            }

            public void Draw(Effect effect, BasicEffect effect2, GraphicsDevice device)
            {
                //PREPARATION
                if (points.Count > 1 && active)
                {
                    int currentIndex = 0;
                    VertexPositionColorTexture[] vertices;

                    void AddVertex(Vector2 position, Color color, Vector2 uv)
                    {
                        if (currentIndex < vertices.Length)
                        {
                            vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position.ForDraw(), 0f), color, uv);
                        }
                    }

                    void PrepareShader()
                    {
                        int width = device.Viewport.Width;
                        int height = device.Viewport.Height;
                        Vector2 zoom = Main.GameViewMatrix.Zoom;
                        Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                        Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

                        effect.Parameters["WorldViewProjection"].SetValue(view * projection);

                        _trailShader.ApplyShader(effect, this, points);
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
                    #region delegates
                    //DrawPrimDelegate LythenPrims = (int noOfPoints) =>
                    //{
                    //    vertices = new VertexPositionColorTexture[noOfPoints];

                    //    AddVertex(points[0], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //    AddVertex(points[1], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //    AddVertex(points[2], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //    PrepareShader();
                    //    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                    //};
                    //DrawPrimDelegate DalantiniumPrims = (int noOfPoints) =>
                    //{
                    //    vertices = new VertexPositionColorTexture[noOfPoints];

                    //    float width = 5;
                    //    float alphaValue = 0.2f;

                    //    for (int i = 0; i < points.Count; i++)
                    //    {
                    //        if (i == 0)
                    //        {
                    //            Color c = Color.DarkRed;
                    //            Vector2 normalAhead = CurveNormal(points, i + 1);
                    //            Vector2 secondUp = points[i + 1] - normalAhead * width;
                    //            Vector2 secondDown = points[i + 1] + normalAhead * width;

                    //            AddVertex(points[i], c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //            AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //            AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //        }
                    //        else
                    //        {
                    //            if (i != points.Count - 1)
                    //            {
                    //                Color c = Color.Red;
                    //                Vector2 normal = CurveNormal(points, i);
                    //                Vector2 normalAhead = CurveNormal(points, i + 1);
                    //                float j = (_dalCap - (i * 0.9f)) / _dalCap;

                    //                width *= (_dalCap - (i * 0.4f)) / _dalCap;

                    //                Vector2 firstUp = points[i] - normal * width;
                    //                Vector2 firstDown = points[i] + normal * width;
                    //                Vector2 secondUp = points[i + 1] - normalAhead * width;
                    //                Vector2 secondDown = points[i + 1] + normalAhead * width;

                    //                AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //                AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //                AddVertex(firstDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));

                    //                AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                    //                AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                    //                AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                    //            }
                    //        }
                    //    }

                    //    PrepareBasicShader();
                    //    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                    //};

                    //DrawPrimDelegate DalantiniumAltPrims = (int noOfPoints) =>
                    //{
                    //    vertices = new VertexPositionColorTexture[noOfPoints];

                    //    float width = 6;
                    //    float alphaValue = 0.1f;

                    //    for (int i = 0; i < points.Count; i++)
                    //    {
                    //        if (i == 0)
                    //        {
                    //            Color c = Color.DarkRed;
                    //            Vector2 normalAhead = CurveNormal(points, i + 1);
                    //            Vector2 secondUp = points[i + 1] - normalAhead * width;
                    //            Vector2 secondDown = points[i + 1] + normalAhead * width;

                    //            AddVertex(points[i], c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //            AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //            AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                    //        }
                    //        else
                    //        {
                    //            if (i != points.Count - 1)
                    //            {
                    //                Color c = Color.Red;
                    //                Vector2 normal = CurveNormal(points, i);
                    //                Vector2 normalAhead = CurveNormal(points, i + 1);
                    //                float j = (_dalCap + ((float)Math.Sin(lerper / 10f) * 1) - i * 0.05f) / _dalCap;

                    //                width *= j;

                    //                Vector2 firstUp = points[i] - normal * width;
                    //                Vector2 firstDown = points[i] + normal * width;
                    //                Vector2 secondUp = points[i + 1] - normalAhead * width;
                    //                Vector2 secondDown = points[i + 1] + normalAhead * width;

                    //                AddVertex(firstUp, c * alphaValue, new Vector2(1));
                    //                AddVertex(secondDown, c * alphaValue, Vector2.Zero);
                    //                AddVertex(firstDown, c * alphaValue, Vector2.Zero);

                    //                AddVertex(secondUp, c * alphaValue, new Vector2(1));
                    //                AddVertex(secondDown, c * alphaValue, Vector2.Zero);
                    //                AddVertex(firstUp, c * alphaValue, Vector2.Zero);
                    //            }
                    //        }
                    //    }

                    //    PrepareBasicShader();

                    //    device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                    //};
                    #endregion
                    // the delegates aren't used outside here, so there's no need to create them

                    if (projectile != null)
                    {
                        if (projectile.type == ProjectileType<LythenStaffProjectile>())
                        {
                            #region LythenPrims
                            const int noOfPoints = 3;
                            vertices = new VertexPositionColorTexture[noOfPoints];

                            AddVertex(points[0], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                            AddVertex(points[1], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                            AddVertex(points[2], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                            PrepareShader();
                            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                            #endregion LythenPrims
                        }

                        else if (projectile.type == ProjectileType<DalantiniumFan>())
                        {
                            #region DalantiniumPrims
                            int noOfPoints = (int)_dalCap * 6 - 9;

                            vertices = new VertexPositionColorTexture[noOfPoints];

                            float width = 5;
                            float alphaValue = 0.2f;

                            for (int i = 0; i < points.Count; i++)
                            {
                                if (i == 0)
                                {
                                    Color c = Color.DarkRed;
                                    Vector2 normalAhead = CurveNormal(points, i + 1);
                                    Vector2 secondUp = points[i + 1] - normalAhead * width;
                                    Vector2 secondDown = points[i + 1] + normalAhead * width;

                                    AddVertex(points[i], c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                }
                                else
                                {
                                    if (i != points.Count - 1)
                                    {
                                        Color c = Color.Red;
                                        Vector2 normal = CurveNormal(points, i);
                                        Vector2 normalAhead = CurveNormal(points, i + 1);
                                        float j = (_dalCap - (i * 0.9f)) / _dalCap;

                                        width *= (_dalCap - (i * 0.4f)) / _dalCap;

                                        Vector2 firstUp = points[i] - normal * width;
                                        Vector2 firstDown = points[i] + normal * width;
                                        Vector2 secondUp = points[i + 1] - normalAhead * width;
                                        Vector2 secondDown = points[i + 1] + normalAhead * width;

                                        AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                        AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                        AddVertex(firstDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));

                                        AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                                        AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                                        AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
                                    }
                                }
                            }

                            PrepareBasicShader();
                            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                            #endregion DalantiniumPrims
                        }

                        else if (projectile.type == ProjectileType<DalantiniumFanAlt>())
                        {
                            #region DalantiniumAltPrims
                            int noOfPoints = (int)_dalCap * 6 - 9;

                            vertices = new VertexPositionColorTexture[noOfPoints];

                            float width = 6;
                            float alphaValue = 0.1f;

                            for (int i = 0; i < points.Count; i++)
                            {
                                if (i == 0)
                                {
                                    Color c = Color.DarkRed;
                                    Vector2 normalAhead = CurveNormal(points, i + 1);
                                    Vector2 secondUp = points[i + 1] - normalAhead * width;
                                    Vector2 secondDown = points[i + 1] + normalAhead * width;

                                    AddVertex(points[i], c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    AddVertex(secondUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                    AddVertex(secondDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
                                }
                                else
                                {
                                    if (i != points.Count - 1)
                                    {
                                        Color c = Color.Red;
                                        Vector2 normal = CurveNormal(points, i);
                                        Vector2 normalAhead = CurveNormal(points, i + 1);
                                        float j = (_dalCap + ((float)Math.Sin(lerper / 10f) * 1) - i * 0.05f) / _dalCap;

                                        width *= j;

                                        Vector2 firstUp = points[i] - normal * width;
                                        Vector2 firstDown = points[i] + normal * width;
                                        Vector2 secondUp = points[i + 1] - normalAhead * width;
                                        Vector2 secondDown = points[i + 1] + normalAhead * width;

                                        AddVertex(firstUp, c * alphaValue, new Vector2(1));
                                        AddVertex(secondDown, c * alphaValue, Vector2.Zero);
                                        AddVertex(firstDown, c * alphaValue, Vector2.Zero);

                                        AddVertex(secondUp, c * alphaValue, new Vector2(1));
                                        AddVertex(secondDown, c * alphaValue, Vector2.Zero);
                                        AddVertex(firstUp, c * alphaValue, Vector2.Zero);
                                    }
                                }
                            }

                            PrepareBasicShader();

                            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, noOfPoints / 3);
                            #endregion 
                        }
                    }
                }
            }

            //Helper methods
            private Vector2 CurveNormal(List<Vector2> points, int index)
            {
                if (points.Count == 1)
                {
                    return points[0];
                }

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

            private Vector2 Clockwise90(Vector2 vector) => new Vector2(-vector.Y, vector.X);
        }
    }
}