using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.DevSets.Vadim
{
    [AutoloadEquip(EquipType.Head)]
    public class YodaMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoda Mask");
            Tooltip.SetDefault("'Great for impersonating mod devs!'\nReminds you of a 2001 Honda Civic");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Cyan;
            item.defense = 21;
        }
    }
}