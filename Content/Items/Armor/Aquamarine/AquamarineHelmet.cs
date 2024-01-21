using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Aquamarine;

[AutoloadEquip(EquipType.Head)]
public class AquamarineHelmet : ModItem
{
    public override void SetStaticDefaults() {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void SetDefaults() {
        Item.width = 16;
        Item.height = 20;
    }
}
