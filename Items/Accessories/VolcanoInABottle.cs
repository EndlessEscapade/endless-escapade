using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Accessories
{

    public class VolcanoInABottle : ModItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volcano in a Bottle");
            //Tooltip.SetDefault("The power of an ancient dragon stirs within you\nReminds you of Lake Floria");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Orange;
            item.width = 32;
            item.height = 34;
        }
    }
}

