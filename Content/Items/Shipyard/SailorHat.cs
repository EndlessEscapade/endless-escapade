using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shipyard;

[AutoloadEquip(EquipType.Head)]
public class SailorHat : ModItem
{
    public override void SetStaticDefaults() { ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; }

    public override void SetDefaults() {
        Item.width = 16;
        Item.height = 16;

        Item.vanity = true;
    }
}
