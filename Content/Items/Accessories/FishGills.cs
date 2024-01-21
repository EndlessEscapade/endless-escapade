using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Accessories;

public class FishGills : ModItem
{
    public override void SetDefaults() {
        Item.accessory = true;

        Item.width = 26;
        Item.height = 28;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) { }
}
