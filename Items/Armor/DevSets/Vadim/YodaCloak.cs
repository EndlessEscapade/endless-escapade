using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.DevSets.Vadim
{
    [AutoloadEquip(EquipType.Body)]
    public class YodaCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoda Cloak");
            Tooltip.SetDefault("'Great for impersonating mod devs!'");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Cyan;
            item.defense = 12;
        }
    }
}