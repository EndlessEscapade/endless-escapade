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
    public class ParticleSystemRU : Mechanic
    {
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            EEMod.Particles.Draw(spriteBatch);
        }

        public override void OnUpdate()
        {
            EEMod.Particles.Update();
        }

        public override void OnLoad()
        {
           
        }
        protected override Layer DrawLayering => Layer.AboveTiles;
    }
}