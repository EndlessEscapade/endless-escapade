using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Aquamarine
{
    [AutoloadEquip(EquipType.Body)]
    public class AquamarineBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Breastplate");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            
        }
    }
}