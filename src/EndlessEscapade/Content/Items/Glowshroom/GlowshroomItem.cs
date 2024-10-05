namespace EndlessEscapade.Content.Items.Glowshroom;

public class GlowshroomItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.maxStack = Item.CommonMaxStack;

        Item.width = 16;
        Item.height = 16;
    }
}
