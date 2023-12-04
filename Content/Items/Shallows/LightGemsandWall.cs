using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.Shallows.LightGemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
