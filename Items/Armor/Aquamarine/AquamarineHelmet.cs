using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Aquamarine
{
    [AutoloadEquip(EquipType.Head)]
    public class AquamarineHelmet : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Helmet");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AquamarineBreastplate>() && legs.type == ModContent.ItemType<AquamarineLeggings>();
        }

        public override void UpdateEquip(Player player)
        {

        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<AquamarineSetBonusPlayer>().aquamarineSetBonus = true;
        }
    }
}