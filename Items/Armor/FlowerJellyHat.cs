using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FlowerJellyHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flower Jelly Hat");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
        }
    }
}