using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Accessories
{
    public class HydrodynamicalDivingGear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrodynamical Diving Gear");
            Tooltip.SetDefault("Grants freedom underwater");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(0, 0, 32);
            item.scale = 0.2f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EEPlayer>().hydroGear = true;
        }
    }
}
