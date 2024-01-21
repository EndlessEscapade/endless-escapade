using Terraria.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Scarlet;

[AutoloadEquip(EquipType.Head)]
public class ScarletKasa : ModItem
{
    public override void SetStaticDefaults() {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }
    
    public override void SetDefaults() {
        Item.width = 34;
        Item.height = 16;
        
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }
}
