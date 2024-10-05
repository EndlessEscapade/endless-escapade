namespace EndlessEscapade.Content.Items.Loot;

[AutoloadEquip(EquipType.Shield)]
public class GreatShellItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.accessory = true;

        Item.width = 34;
        Item.height = 38;

        Item.defense = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        base.UpdateAccessory(player, hideVisual);

        // TODO: Find a way to decrease knockback instead of completely negating it.
        player.noKnockback = true;
    }
}
