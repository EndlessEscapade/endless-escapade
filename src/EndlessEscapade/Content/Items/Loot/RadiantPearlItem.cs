namespace EndlessEscapade.Content.Items.Loot;

public class RadiantPearlItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.accessory = true;

        Item.width = 36;
        Item.height = 36;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        base.UpdateAccessory(player, hideVisual);

        Main.instance.SpelunkerProjectileHelper.AddSpotToCheck(player.Center);
    }
}
