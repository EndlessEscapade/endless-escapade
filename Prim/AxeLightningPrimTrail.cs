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
        public AxeLightningPrimTrail(Projectile projectile)
        {
            _projectile = projectile;
            _listOfPoints.Add(_projectile.position);
            Main.NewText("Axe lightning created", 255, 255, 255);
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.7f;
            _width = 5;
            _cap = 170;
            Main.NewText("Defaults set", 255, 255, 255);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.NewText("Trail drawn!", 255, 255, 255);
        }
        public override void Update()
        {
            Main.NewText("Updating!", 255, 255, 255);
            _lerper++;
            _listOfPoints.Add(_projectile.Center);

            if (_cap < _listOfPoints.Count)
            {
                _listOfPoints.RemoveAt(0);
            }
            if ((_projectile.active && _projectile != null))
            {
                Dispose();
            }
            base.Update();
        }
    }
}