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
using EEMod.NPCs.CoralReefs;

namespace EEMod.Prim
{
    class SpirePrimTrail : Primitive
    {
        public SpirePrimTrail(Projectile projectile, Color _color, int width = 40) : base(projectile)
        {
            BindableEntity = projectile;
            _width = width;
            color = _color;
        }
        private Color color;
        public override void SetDefaults()
        {
            Alpha = 0.8f;
            _cap = 60;
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
            {
                /*widthVar = _width;
                Color c1 = color;
                Vector2 normalAhead = CurveNormal(_points, 1);
                Vector2 secondUp = _points[1] - normalAhead * widthVar;
                Vector2 secondDown = _points[1] + normalAhead * widthVar;*/
                //   AddVertex(_points[i], c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                //  AddVertex(secondUp, c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                // AddVertex(secondDown, c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
            }
            for (int i = 1; i < _points.Count - 1; i++)
            {
                widthVar = _width * (float)Math.Sin(i / (float)_points.Count * 3.14f);

                Color c = color;
                Color CBT = color;
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);
                Vector2 firstUp = _points[i] - normal * widthVar;
                Vector2 firstDown = _points[i] + normal * widthVar;
                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                AddVertex(firstDown, c * Alpha, new Vector2((i / (float)_cap), 1));
                AddVertex(firstUp, c * Alpha, new Vector2((i / (float)_cap), 0));
                AddVertex(secondDown, CBT * Alpha, new Vector2((i + 1) / (float)_cap, 1));

                AddVertex(secondUp, CBT * Alpha, new Vector2((i + 1) / (float)_cap, 0));
                AddVertex(secondDown, CBT * Alpha, new Vector2((i + 1) / (float)_cap, 1));
                AddVertex(firstUp, c * Alpha, new Vector2((i / (float)_cap), 0));
            }
        }
        public override void SetShaders()
        {
            PrepareShader(EEMod.NonBasicEffectShader, "Lazor", _counter);
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
            _width *= 0.4f;
            if (_width < 0.05f)
            {
                Dispose();
            }
        }

    }
}