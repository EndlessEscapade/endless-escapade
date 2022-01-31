using EEMod.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Seamap
{
    internal class SeamapUpdater : ModSystem
    {
        public static bool IsOnSeamap => Main.worldName == KeyID.Sea;
        // runs in DoUpdate
        public override void PreUpdateEntities()
        {
            if (IsOnSeamap)
            {
                Seamap.SeamapContent.Seamap.UpdateSeamap();
            }
            base.PreUpdateEntities();
        }
    }
}
