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
using EEMod.Projectiles.Ranged;
using EEMod.Projectiles.Melee;
using EEMod.NPCs.CoralReefs;

namespace EEMod.Prim
{
    class AxeLightningPrimTrail : PrimTrail
    {
        public AxeLightningPrimTrail(Projectile projectile) : base(projectile)
        {
            _projectile = projectile;
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.7f;
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
            for (int i = 0; i < _points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = (float)Math.Sqrt(_points.Count) * _width;
                    Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
                    Vector2 normalAhead = CurveNormal(_points, i + 1);
                    Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;
                    AddVertex(_points[i], c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                    AddVertex(secondUp, c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                    AddVertex(secondDown, c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                }
                else
                {
                    if (i != _points.Count - 1)
                    {
                        widthVar = (float)Math.Sqrt(_points.Count - i) * _width;
                        Color base1 = new Color(7, 86, 122);
                        Color base2 = new Color(255, 244, 173);
                        Color c = Color.Lerp(Color.White, Color.Cyan, colorSin);
                        Color CBT = Color.Lerp(Color.White, Color.Cyan, colorSin);
                        Vector2 normal = CurveNormal(_points, i);
                        Vector2 normalAhead = CurveNormal(_points, i + 1);
                        float j = (_cap + ((float)(Math.Sin(_counter / 10f)) * 1) - i * 0.1f) / _cap;
                        widthVar *= j;
                        Vector2 firstUp = _points[i] - normal * widthVar;
                        Vector2 firstDown = _points[i] + normal * widthVar;
                        Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, c * _alphaValue, new Vector2((i / _cap), 1));
                        AddVertex(firstUp, c * _alphaValue, new Vector2((i / _cap), 0));
                        AddVertex(secondDown, CBT * _alphaValue, new Vector2((i + 1) / _cap, 1));

                        AddVertex(secondUp, CBT * _alphaValue, new Vector2((i + 1) / _cap, 0));
                        AddVertex(secondDown, CBT * _alphaValue, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstUp, c * _alphaValue, new Vector2((i / _cap), 0));
                    }
                    else
                    {

                    }
                }
            }
            PrepareBasicShader();
            _device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, _noOfPoints / 3);
        }
        public override void OnUpdate()
        {
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
            if ((!_projectile.active && _projectile != null))
            {
                OnDestroy();
            }
            else
            {
                _points.Add(_projectile.Center);
            }
        }
        public override void OnDestroy()
        {
            _width *= 0.9f;
            if (_width < 0.05f)
            {
                Dispose();
            }
        }
    }
}