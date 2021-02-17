using EEMod.Extensions;
using EEMod.Tiles.Furniture;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class CoralReefVines : Mechanic
    {
        protected override Layer DrawLayering => Layer.BehindTiles;
     
        public override void OnDraw()
        {
        }
    }
}