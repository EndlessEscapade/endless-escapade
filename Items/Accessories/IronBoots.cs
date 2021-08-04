using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class IronBoots : EEItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron Boots");
            Tooltip.SetDefault("Extremely heavy");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.width = 32;
            Item.height = 34;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.gravity = 1.5f;
        }
    }
}