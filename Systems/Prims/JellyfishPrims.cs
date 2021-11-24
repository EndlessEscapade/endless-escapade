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


namespace EEMod.Prim
{
    class JellyfishPrims : Primitive
    {
        public JellyfishPrims(NPC projectile) : base(projectile)
        {
            BindableEntity = projectile;
        }
        public override void SetDefaults()
        {
            Alpha = 0.7f;
            _width = 1;
            _cap = 80;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(_counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(_points.Count) * _width;
            DrawBasicTrail(c1, widthVar);*/

            if (_noOfPoints <= 1) return;
            float widthVar;
            float colorSin = (float)Math.Sin(_counter / 3f);
            {
                widthVar = (float)Math.Sqrt(_points.Count) * _width;
                Color c1 = Color.Lerp(Color.White, Color.Gold, colorSin);
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
                widthVar = (float)Math.Sqrt(_points.Count - i) * _width;
                Color base1 = new Color(7, 86, 122);
                Color base2 = new Color(255, 244, 173);
                Color c = Color.Lerp(Color.White, Color.Gold, colorSin);
                Color CBT = Color.Lerp(Color.White, Color.Gold, colorSin);
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);
                float j = (_cap + ((float)(Math.Sin(_counter / 10f)) * 1) - i * 0.1f) / _cap;
                widthVar *= j;
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

        /*Jellyfish ja = (npc.modNPC as Jellyfish);
        Cap = ja.cap;
                    List<List<List<Vector2>>> tentacle = new List<List<List<Vector2>>>();

                    for (int b = 0; b< 2; b++)
                    {
                        List<List<Vector2>> tempTentA = new List<List<Vector2>>();
                        for (int a = 0; a<ja.noOfTentacles / 2; a++)
                        {
                            List<Vector2> tempTent = new List<Vector2>();
                            for (int i = 0; i<Cap; i++)
                            {
                                tempTent.Add(ja.lol1[a, (int)Cap - i - 1, b]);
                            }
    tempTentA.Add(tempTent);
                        }
tentacle.Add(tempTentA);
                    }

                    List<VertexPositionColorTexture[]> vertices2 = new List<VertexPositionColorTexture[]>();
vertices = new VertexPositionColorTexture[noOfPoints];

float width = 2;
float alphaValue = 0.8f;
for (int b = 0; b < tentacle.Count; b++)
{
    for (int a = 0; a < tentacle[b].Count; a++)
    {
        for (int i = 1; i < tentacle[b][a].Count - 1; i++)
        {

            Color base1 = new Color(7, 86, 122);
            Color base2 = new Color(255, 244, 173);

            Color drawColour = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
            Color c = Color.Lerp(Color.DarkCyan, base2, i / Cap).MultiplyRGB(drawColour);
            Color c1 = Color.Lerp(Color.DarkCyan, base2, (i + 1) / Cap).MultiplyRGB(drawColour);

            Vector2 normal = CurveNormal(tentacle[b][a], i);
            Vector2 normalAhead = CurveNormal(tentacle[b][a], i + 1);

            float j = (Cap - (i * 0.9f)) / Cap;
            width = (i / Cap) * 3;

            Vector2 firstUp = tentacle[b][a][i] - normal * width;
            Vector2 firstDown = tentacle[b][a][i] + normal * width;
            Vector2 secondUp = tentacle[b][a][i + 1] - normalAhead * width;
            Vector2 secondDown = tentacle[b][a][i + 1] + normalAhead * width;

            AddVertex(firstDown, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
            AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));
            AddVertex(secondDown, c1 * alphaValue, new Vector2((float)Math.Sin(lerper / 20f), (float)Math.Sin(lerper / 20f)));

            AddVertex(secondUp, c1 * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
            AddVertex(secondDown, c1 * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
            AddVertex(firstUp, c * alphaValue, new Vector2((float)Math.Sin(lerper / 20f) * j, (float)Math.Sin(lerper / 20f) * j));
        }

        vertices2.Add(vertices);
        PrepareBasicShader();
        device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices2[a], 0, noOfPoints / 3);
    }
}
                }*/
        public override void SetShaders()
        {
            PrepareBasicShader();
        }
        public override void OnUpdate()
        {
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
            if ((!BindableEntity.active && BindableEntity != null) || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                _points.Add(BindableEntity.Center);
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
    }
}