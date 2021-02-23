using EEMod.Extensions;
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
    public class VerletRendering : Mechanic
    {
        public override void OnDraw()
        {
            Singleton.verlet.GlobalRenderPoints();
        }

        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}