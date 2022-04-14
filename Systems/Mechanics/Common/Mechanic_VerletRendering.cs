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
    public class VerletRendering : ModSystem
    {
        public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            ModContent.GetInstance<EEMod>().verlet.GlobalRenderPoints();

            Main.spriteBatch.End();
        }
    }
}