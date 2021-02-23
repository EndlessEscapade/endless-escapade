using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class IronBoots : ModItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron Boots");
            Tooltip.SetDefault("You fall faster in water");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
        }
    }
}