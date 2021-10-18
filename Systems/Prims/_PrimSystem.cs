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
using Terraria.ID;

namespace EEMod.Prim
{
    public class PrimtiveSystem : ModSystem
    {
        public static TrailManager trailManager;
        public static PrimTrailManager primitives;

        public override void PreUpdateProjectiles()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                trailManager.UpdateTrails();
                //prims.UpdateTrails();
                primitives.UpdateTrailsAboveTiles();
            }
        }
    }
}