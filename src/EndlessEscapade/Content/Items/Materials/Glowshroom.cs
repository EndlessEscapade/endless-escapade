using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Materials;

public class Glowshroom : ModItem
{
    public override void SetDefaults() {
        Item.maxStack = Item.CommonMaxStack;

        Item.width = 16;
        Item.height = 16;
    }
}
