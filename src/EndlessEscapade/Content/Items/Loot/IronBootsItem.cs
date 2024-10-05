namespace EndlessEscapade.Content.Items.Loot;

public class IronBootsItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.accessory = true;

        Item.width = 36;
        Item.height = 32;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        base.UpdateAccessory(player, hideVisual);

        if (!player.controlDown) {
            return;
        }

        player.gravity += 1.5f;
    }
}
