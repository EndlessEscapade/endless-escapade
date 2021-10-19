using EEMod.Extensions;
using EEMod.Systems;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EEMod.Prim;

namespace EEMod
{
    public class MeshBehindTiles : ModSystem
    {
        public override void PostDrawTiles()
        {
            PrimitiveSystem.primitives.DrawTrailsBehindTiles();
        }

        public override void PostUpdateEverything()
        {
            PrimitiveSystem.primitives.UpdateTrailsBehindTiles();
        }
    }
}