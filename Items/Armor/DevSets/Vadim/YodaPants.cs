using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.DevSets.Vadim
{
    [AutoloadEquip(EquipType.Legs)]
    public class YodaPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoda Pants");
            Tooltip.SetDefault("'Great for impersonating mod devs!'");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Cyan;
            item.defense = 11;
        }

    }
}