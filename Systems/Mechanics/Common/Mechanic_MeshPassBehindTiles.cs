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

namespace EEMod
{
    public class MeshBehindTiles : Mechanic
    {
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            EEMod.primitives.DrawTrailsBehindTiles();
        }

        public override void OnUpdate()
        {
            EEMod.primitives.UpdateTrailsBehindTiles();
        }

        public override void OnLoad()
        {
           
        }
        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}