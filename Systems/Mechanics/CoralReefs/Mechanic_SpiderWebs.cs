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
    public class SpiderWebs : Mechanic
    {
        public override void OnDraw()
        {
           
        }

        public override void OnUpdate()
        {

        }

        public override void OnLoad()
        {
           
        }
        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}