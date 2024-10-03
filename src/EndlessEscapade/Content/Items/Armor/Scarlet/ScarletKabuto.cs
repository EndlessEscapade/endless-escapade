using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Scarlet;

[AutoloadEquip(EquipType.Head)]
public class ScarletKabuto : ModItem
{
    public override void SetStaticDefaults() {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 22;

        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }
}
