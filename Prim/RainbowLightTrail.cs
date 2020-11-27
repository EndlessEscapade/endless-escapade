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
    class RainbowLightTrail : PrimTrail
    {
        public RainbowLightTrail(Projectile projectile) : base(projectile)
        {
            _projectile = projectile;
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.7f;
            _width = 1;
            _cap = 100;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (i == 0)
                {

                }
                else
                {
                    if (i != _points.Count - 1)
                    {
                        MakePrimHelix(i, 20, 0.8f,default,1,2);
                    }
                }
            }
        }
        public override void SetShaders()
        {
            PrepareShader(EEMod.TrailPractice, _counter / 40f);
        }
        public override void OnUpdate()
        {
            _points.Add(Main.LocalPlayer.Center);
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
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