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

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class GoblinBanner : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banner");
        }

        public DarksaberPrimTrail trail;
        public DarksaberPrimTrail trail2;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.damage = 1;
            Projectile.hide = false;
            Projectile.damage = 5;
            Projectile.knockBack = 0f;
        }

        public float rot;

        public override void AI()
        {
            
        }
    }

    /*public class DarksaberNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int darksaberIframes;

        public override bool PreAI(NPC npc)
        {
            darksaberIframes--;

            return base.PreAI(npc);
        }
    }*/

    public class GoblinBannerPrims : Primitive
    {
        public GoblinBannerPrims(Entity projectile, Color _color, int _bladeVal1, int _bladeVal2, int _interval, bool _additive, int cap = 300, int _width = 5) : base(projectile)
        {
            BindableEntity = projectile;
            width = _width;
            color = _color;
            _cap = cap;

            interval = _interval;

            bladeVal1 = _bladeVal1;
            bladeVal2 = _bladeVal2;

            interval = _interval;

            additive = _additive;

            orig = Main.player[(BindableEntity as Projectile).owner].Center;

            _points.Clear();

            myLength = 0;
        }

        public int myLength;
        private Color color;
        public bool additive;
        public bool flipped;
        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = false;
            ManualDraw = false;
            manualDraw = true;
            pixelated = true;
        }

        public float width;

        public int bladeVal1;
        public int bladeVal2;

        public int interval;

        public Vector2 orig;

        public float rot;

        public int ticks;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;

            for (int i = 0; i < (myLength / interval) - 1; i++)
            {
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                //normal = new Vector2(0, -1);
                //normalAhead = new Vector2(0, -1);

                Vector2 firstUp = _points[i] - normal * width;
                Vector2 firstDown = _points[i] + normal * width;

                Vector2 firstSpine = _points[i];
                Vector2 secondSpine = _points[i + 1];

                Vector2 secondUp = _points[i + 1] - normalAhead * width;
                Vector2 secondDown = _points[i + 1] + normalAhead * width;

                if (i == (myLength / interval) - 3)
                {
                    if (flipped)
                    {
                        AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));

                        AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / _cap, 0));
                        AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));


                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 1));
                        AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                        AddVertex(secondSpine, Color.White, new Vector2((i + 1) / _cap, 1));
                    }
                    else
                    {
                        AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));

                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / _cap, 0));


                        AddVertex(secondSpine, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstDown, Color.White, new Vector2((i / _cap), 0));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 1));
                    }
                }
                else if (i == (myLength / interval) - 2)
                {
                    if (flipped)
                    {
                        if (!additive)
                        {
                            AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                            AddVertex(firstSpine, Color.White, new Vector2((i / _cap), 0));
                            AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        }
                        else
                        {
                            AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                            AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                            AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        }
                    }
                    else
                    {
                        if (!additive)
                        {
                            AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                            AddVertex(firstSpine, Color.White, new Vector2((i / _cap), 0));
                            AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));
                        }
                        else
                        {
                            AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                            AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                            AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));
                        }
                    }
                }
                else
                {
                    AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                    AddVertex(firstSpine, color, new Vector2((i / _cap), 0));
                    AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));

                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 0));
                    AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                    AddVertex(firstSpine, color, new Vector2((i / _cap), 0));


                    AddVertex(firstSpine, color, new Vector2((i / _cap), 1));
                    AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 1));

                    AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 0));
                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 1));
                    AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                }
            }
        }

        public override void SetShaders()
        {
            if (vertices.Length == 0) return;

            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            if (additive)
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(204f, 107f, 183f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(204f, 107f, 183f, 0f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.4f);

                EEMod.DarksaberShader.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/LightningNoisePixelatedBloom").Value);

                EEMod.DarksaberShader.Parameters["ticks"].SetValue(Main.GameUpdateCount / 20f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }
            else
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(120f, 44f, 166f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(204f, 107f, 183f, 64f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.3f);

                EEMod.DarksaberShader.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/LightningNoisePixelatedBloom").Value);

                EEMod.DarksaberShader.Parameters["ticks"].SetValue(Main.GameUpdateCount / 20f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.DarksaberShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            _device.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints);

            VertexBufferPool.Shared.Return(buffer);
        }

        public override void OnUpdate()
        {
            if (additive) width = 10f + ((float)Math.Sin(Main.GameUpdateCount / 10f) * 1.5f);

            if (myLength < bladeVal2 && ticks < 30 && ticks > 5) myLength += (interval * 2);

            ticks++;
            if (ticks > 35) myLength -= (interval * 2);

            _counter++;
            _noOfPoints = _points.Count() * 12;
            if (_cap < _noOfPoints / 12)
            {
                _points.RemoveAt(0);
            }

            if ((!BindableEntity.active && BindableEntity != null) || BindableEntity == null || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                _points.Clear();

                for (int i = 0; i < myLength / interval; i++)
                {
                    Vector2 vec = Vector2.Lerp(orig, orig + (Vector2.UnitX.RotatedBy(rot) * myLength), (float)i / (float)(myLength / interval));

                    _points.Add(vec);
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