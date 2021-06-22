using EEMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Kelpweaver
{
    [AutoloadEquip(EquipType.Body)]
    public class KelpweaverBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpweaver Body");
            Tooltip.SetDefault("Creepy and crawly");
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
            player.allDamage += 0.09f;
        }
    }
}