using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Scarlet;

[AutoloadEquip(EquipType.Head)]
public class ScarletHachimaki : ModItem
{
    public override void SetStaticDefaults() {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 12;

        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }
}
