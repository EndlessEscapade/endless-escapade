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
            void ApplyShader(Effect effect, Trail trail, List<Vector2> positions);
        }
        public void DrawTrails(SpriteBatch spriteBatch)
        {
            foreach (Trail trail in _trails)
            {
                trail.Draw(_effect, _basicEffect, Main.graphics.GraphicsDevice);
            }
        }
        public class DefaultShader : ITrailShader
        {
            public string ShaderPass => "DefaultPass";
            public void ApplyShader(Effect effect, Trail trail, List<Vector2> positions)
            {
                effect.CurrentTechnique.Passes[ShaderPass].Apply();
            }
        }
        private List<Trail> _trails = new List<Trail>();
        private Effect _effect;
        private BasicEffect _basicEffect;
        public void UpdateTrails()
        {
            for (int i = 0; i < _trails.Count; i++)
            {
                Trail trail = _trails[i];

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
        public void CreateTrail(List<Vector2> gaming, ITrailShader shader = null, Projectile projectile = null)
        {
            Trail newTrail = new Trail(gaming, new RoundCap(), new DefaultShader(), projectile);
            _trails.Add(newTrail);
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
                void AddVertex(Vector2 position, Color color,Vector2 uv)
                {
                    vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position.ForDraw(), 0f), color,uv);
                }
                if (_projectile != null)
                {
                    if (_projectile.type == ModContent.ProjectileType<LythenStaffProjectile>())
                    {
                       AddVertex(_points[0], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 10f)));
                       AddVertex(_points[1], Color.LightBlue * (float)Math.Asin(lerper / 20f), new Vector2((float)Math.Cos(lerper / 10f), (float)Math.Cos(lerper / 20f)));
                       AddVertex(_points[2], Color.LightBlue * (float)Math.Sin(lerper / 20f), new Vector2((float)Math.Asin(lerper / 20f), (float)Math.Cos(lerper / 10f)));
                    }
                }

               int width = device.Viewport.Width;
                int height = device.Viewport.Height;
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
                effect.Parameters["WorldViewProjection"].SetValue(view * projection);
                _trailShader.ApplyShader(effect, this,_points);
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0,1);
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


