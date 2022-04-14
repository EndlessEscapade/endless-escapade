using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Dalantinium
{
    public class DalantiniumSetPlayer : ModPlayer
    {
        public bool dalantiniumHood;
        public bool dalantiniumSet;

        public override void ResetEffects()
        {
            base.ResetEffects();
            dalantiniumSet = false;
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            base.ModifyManaCost(item, ref reduce, ref mult);
            if (dalantiniumHood)
            {
                reduce -= 0.05f;
            }
        }
    }
}
