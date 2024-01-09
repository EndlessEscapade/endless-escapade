using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.ShellPiles;

[AutoloadEquip(EquipType.Shield)]
public class GreatShell : ModItem
{
    public override void SetDefaults() {
        Item.accessory = true;
        
        Item.width = 34;
        Item.height = 38;
        
        Item.defense = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        // TODO: Find a way to decrease knockback instead of completely negating it.
        player.noKnockback = true;
    }
}
