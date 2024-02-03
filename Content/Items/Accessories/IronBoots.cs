using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Accessories;

public class IronBoots : ModItem
{
    public override void SetDefaults() {
        Item.accessory = true;

        Item.width = 36;
        Item.height = 32;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (!player.controlDown) {
            return;
        }

        player.gravity += 1.5f;
    }
}
