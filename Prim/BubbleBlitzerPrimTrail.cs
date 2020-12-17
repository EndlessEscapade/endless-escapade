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
    class BubbleBlitzerPrimTrail : PrimTrail
    {
        public BubbleBlitzerPrimTrail(Projectile projectile) : base(projectile)
        {
            _projectile = projectile;
        }
        private Color _color;
        public override void SetDefaults()
        {
            _alphaValue = 0.1f;
            _width = 4;
            _cap = 20;
            _color = new Color(0.75f, 0.38f, 0.9f, 1f);
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(_counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(_points.Count) * _width;
            DrawBasicTrail(c1, widthVar);*/

            if (_noOfPoints <= 6) return;
            float widthVar;
            float colorSin = (float)Math.Sin(_counter / 3f);
            for (int i = 0; i < _points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = _width;
                    Vector2 normalAhead = CurveNormal(_points, i + 1);
                    Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;
                    AddVertex(_points[i], _color * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                    AddVertex(secondUp, _color * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                    AddVertex(secondDown, _color * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                }
                else
                {
                    if (i != _points.Count - 1)
                    {
                        widthVar = _width;
                        Vector2 normal = CurveNormal(_points, i);
                        Vector2 normalAhead = CurveNormal(_points, i + 1);
                        Vector2 firstUp = _points[i] - normal * widthVar;
                        Vector2 firstDown = _points[i] + normal * widthVar;
                        Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, _color * _alphaValue, new Vector2((i / _cap), 1));
                        AddVertex(firstUp, _color * _alphaValue, new Vector2((i / _cap), 0));
                        AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / _cap, 1));

                        AddVertex(secondUp, _color * _alphaValue, new Vector2((i + 1) / _cap, 0));
                        AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstUp, _color * _alphaValue, new Vector2((i / _cap), 0));
                    }
                    else
                    {

                    }
                }
            }
        }
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
            if ((!_projectile.active && _projectile != null) || _destroyed)
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
            _destroyed = true;
            _width *= 0.8f;
            if (_width < 0.05f)
            {
                Dispose();
            }
        }
    }
}