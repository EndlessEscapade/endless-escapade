using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Tropical.TropicalWoodWall>());

        Item.width = 24;
        Item.height = 24;
    }
}
