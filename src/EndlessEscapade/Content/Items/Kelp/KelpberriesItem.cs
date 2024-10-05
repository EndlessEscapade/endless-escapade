namespace EndlessEscapade.Content.Items.Kelp;

public class KelpberriesItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.maxStack = Item.CommonMaxStack;

        Item.width = 22;
        Item.height = 26;
    }
}
