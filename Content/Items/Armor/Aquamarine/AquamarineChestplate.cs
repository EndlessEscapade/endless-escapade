using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Aquamarine;

[AutoloadEquip(EquipType.Body)]
public class AquamarineChestplate : ModItem
{
    public override void SetDefaults() {
        Item.width = 34;
        Item.height = 24;
    }
}
