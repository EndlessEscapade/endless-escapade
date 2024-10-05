namespace EndlessEscapade.Content.Items.Loot;

public class FishGillsItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.accessory = true;

        Item.width = 26;
        Item.height = 28;
    }
}
