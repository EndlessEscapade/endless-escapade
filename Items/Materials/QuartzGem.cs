using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Materials
{
    public class QuartzGem : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Quartz");
        }
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.maxStack = 999;
            item.value = 100;
            item.rare = ItemRarityID.Blue;
        }
    }
}
