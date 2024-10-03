using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class DarkGemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.DarkGemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
