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
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EEPlayer>().isWearingCape = true;
        }
    }
}