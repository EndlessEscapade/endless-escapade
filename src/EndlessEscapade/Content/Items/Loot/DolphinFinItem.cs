namespace EndlessEscapade.Content.Items.Loot;

public class DolphinFinItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.accessory = true;

        Item.width = 22;
        Item.height = 26;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        base.UpdateAccessory(player, hideVisual);

        player.ignoreWater = true;
    }
}
