using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class MirrorShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mirror Shield");
            //Tooltip.SetDefault("The power of an ancient dragon stirs within you\nReminds you of Lake Floria");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Pink;
            item.width = 32;
            item.height = 34;
        }
    }
}