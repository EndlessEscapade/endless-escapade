using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.DevSets.Vadim
{
    [AutoloadEquip(EquipType.Head)]
    public class YodaMask : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoda Mask");
            Tooltip.SetDefault("'Great for impersonating mod devs!'\nReminds you of a 2001 Honda Civic");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 21;
        }
    }
}