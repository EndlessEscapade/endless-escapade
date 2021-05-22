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
using EEMod.NPCs.CoralReefs;
using EEMod.Items.Weapons.Melee.Shivs;

namespace EEMod.Prim
{
    class RapierPrimTrail : PrimTrail
    {
        public RapierPrimTrail(Projectile projectile, Vector2 start, Vector2 mid, Vector2 end) : base(projectile)
        {
            _projectile = projectile;
            for (float i = 0; i <= 1; i += 0.01f)
                _points.Add(Helpers.TraverseBezier(end, start, mid, mid, i));
        }
        public override void SetDefaults()
        {
            _alphaValue = 1f;
            _width = 20;
            _cap = 100;
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
                    widthVar = (float)Math.Sqrt(_points.Count) * _width;
                    Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
                    Vector2 normalAhead = CurveNormal(_points, i + 1);
                    Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;
                    //AddVertex(_points[i], c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                    // AddVertex(secondUp, c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                    //AddVertex(secondDown, c1 * _alphaValue, new Vector2((float)Math.Sin(_counter / 20f), (float)Math.Sin(_counter / 20f)));
                }
                else
                {
                    if (i != _points.Count - 1)
                    {
                        float dist = Math.Abs((_points.Count - i) - _counter * 5);
                        widthVar = (float)Math.Sin(i * (3.14f / _points.Count)) * _width * (i) / 100f;
                        float widthVar2 = (float)Math.Sin((i + 1) * (3.14f / _points.Count)) * _width * ((i + 1)) / 100f;
                        Color c = Color.White * ((100 - dist) * 0.01f) * (_counter / 10f);
                        Color CBT = Color.White * ((100 - dist) * 0.01f) * (_counter / 10f);
                        Vector2 normal = CurveNormal(_points, i);
                        Vector2 normalAhead = CurveNormal(_points, i + 1);
                        Vector2 firstUp = _points[i] - normal * widthVar;
                        Vector2 firstDown = _points[i] + normal * widthVar;
                        Vector2 secondUp = _points[i + 1] - normalAhead * widthVar2;
                        Vector2 secondDown = _points[i + 1] + normalAhead * widthVar2;

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
        }
        public override void SetShaders()
        {
            PrepareShader(EEMod.TrailPractice, "Edge", _counter / 20f);
        }
        public override void OnUpdate()
        {
            _counter++;
                EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.7f));
            if (_counter < 20)
            {
                Color chosen = Color.Lerp(Color.Cyan, Color.DarkBlue, Main.rand.NextFloat(1f));
                EEMod.MainParticles.SpawnParticles(_points[(100 -_counter*5) % 100] + new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30)), Vector2.Normalize(_points[50] - Main.LocalPlayer.Center).RotatedBy(Main.rand.NextFloat(-6, 6)) * 5,2, chosen, new SlowDown(0.9f), new RotateTexture(Main.rand.NextFloat(-0.03f, 0.03f)), new SetMask(ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient")), new AfterImageTrail(1f), new RotateVelocity(Main.rand.NextFloat(-0.06f, 0.06f)), new SetLighting(chosen.ToVector3(), 0.1f), new SetTimeLeft(10), new SetShrinkSize(0.95f));
            }
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
            if ((_projectile.modProjectile as Rapier).timeForSwing >= 0.99f)
            {
                OnDestroy();
            }
            else
            {
                //_points.Add((_projectile.modProjectile as Rapier).Center);
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