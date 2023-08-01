using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Tropical;

public class TropicalWoodWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.Tropical.TropicalWoodWall>());
    }
}