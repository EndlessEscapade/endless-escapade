using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Materials
{
    public class QuartzicLifeFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 5));
            base.SetStaticDefaults();
            DisplayName.SetDefault("Quartzic Life Fragment");
            Tooltip.SetDefault("The Essence of a Powerful Quartz Creature");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 28;
            item.maxStack = 999;
            item.value = 100;
            item.rare = 5;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.alpha = 90;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Pink.ToVector3() * 0.55f * Main.essScale);
        }

    }
}
