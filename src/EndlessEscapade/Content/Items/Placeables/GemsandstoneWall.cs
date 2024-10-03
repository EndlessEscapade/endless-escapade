using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class GemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.GemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
