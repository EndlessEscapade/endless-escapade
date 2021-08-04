using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Aquamarine
{
    [AutoloadEquip(EquipType.Legs)]
    public class AquamarineLeggings : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Greaves");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {

        }
    }
}