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
using EEMod.Projectiles;

namespace EEMod.Prim
{
    class WormMesh : Primitive
    {
        ExampleWorm EW;
        public WormMesh(Projectile projectile, ExampleWorm Mesh) : base(projectile)
        {
            EW = Mesh;
            for (int i = 0; i < EW.Segments.Length; i++)
                _points.Add(EW.Segments[i].position);
        }
        public override void SetDefaults()
        {
            Alpha = 1f;
            _width = 5;
            _cap = 50;
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
            for (int i = 1; i < _points.Count - 1; i++)
            {
                widthVar = _width * (1 - i / (float)_points.Count);
                float widthVar2 = _width * (1 - (i + 1) / (float)_points.Count);
                Color c = Color.Lerp(EW.color, Color.Black, (i / (float)_points.Count));
                Color CBT = Color.Lerp(EW.color, Color.Black, ((i + 1) / (float)_points.Count));
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);
                Vector2 firstUp = _points[i] - normal * widthVar;
                Vector2 firstDown = _points[i] + normal * widthVar;
                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar2;
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar2;

                AddVertex(firstDown, c * Alpha, new Vector2((i / _cap), 1));
                AddVertex(firstUp, c * Alpha, new Vector2((i / _cap), 0));
                AddVertex(secondDown, CBT * Alpha, new Vector2((i + 1) / _cap, 1));

                AddVertex(secondUp, CBT * Alpha, new Vector2((i + 1) / _cap, 0));
                AddVertex(secondDown, CBT * Alpha, new Vector2((i + 1) / _cap, 1));
                AddVertex(firstUp, c * Alpha, new Vector2((i / _cap), 0));

            }
        }
        public override void SetShaders()
        {
            PrepareBasicShader();
        }
        public override void OnUpdate()
        {
            _counter++;
            for (int i = 0; i < _points.Count; i++)
                _points[i] = EW.Segments[i].position;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
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