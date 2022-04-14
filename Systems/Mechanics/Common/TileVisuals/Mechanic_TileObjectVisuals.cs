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
    public class TileObjectVisuals : ModSystem
    {
        public override void PostDrawTiles()
        {
            ModContent.GetInstance<EEMod>().TVH.Draw(Main.spriteBatch);
        }

        public override void PostUpdateEverything()
        {
            ModContent.GetInstance<EEMod>().TVH.Update();
        }
    }
}