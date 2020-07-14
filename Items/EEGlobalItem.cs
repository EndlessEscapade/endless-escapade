using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles;
using Terraria.ID;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        //private bool randomAssVanillaAdaptedFlag = false;
        //private int debug = 0;
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            EEPlayer eeplayer = player.GetModPlayer<EEPlayer>();
            if (eeplayer.dalantiniumHood)
            {
                reduce -= 0.05f;
            }
            if (eeplayer.hydriteVisage)
            {
                reduce -= 0.1f;
            }
        }


    }
}