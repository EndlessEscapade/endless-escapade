using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class RedVelvetCape : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Velvet Cape");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.width = 32;
            item.height = 34;
        }
    }
}