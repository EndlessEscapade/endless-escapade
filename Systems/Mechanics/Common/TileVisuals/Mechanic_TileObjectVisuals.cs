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
    public class TileObjectVisuals : Mechanic
    {
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            ModContent.GetInstance<EEMod>().TVH.Draw(spriteBatch);
        }

        public override void OnUpdate()
        {
            ModContent.GetInstance<EEMod>().TVH.Update();
        }

        public override void OnLoad()
        {
           
        }
        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}