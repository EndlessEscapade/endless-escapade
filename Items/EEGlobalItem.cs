using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        //public override bool InstancePerEntity => true; // unneeded atm

        //public override bool CloneNewInstances => true;

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