using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FlowerJellyHat : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flower Jelly Hat");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
        }
    }
}