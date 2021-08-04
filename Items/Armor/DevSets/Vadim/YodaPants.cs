using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.DevSets.Vadim
{
    [AutoloadEquip(EquipType.Legs)]
    public class YodaPants : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoda Pants");
            Tooltip.SetDefault("'Great for impersonating mod devs!'");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 11;
        }
    }
}