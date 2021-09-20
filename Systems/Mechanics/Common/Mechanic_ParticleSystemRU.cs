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
    public class ParticleSystemRU : ModSystem
    {
        public override void PostDrawTiles()
        {
            EEMod.Particles.Draw(Main.spriteBatch);
        }

        public override void PostUpdatePlayers()
        {
            EEMod.Particles.Update();
        }
    }
}