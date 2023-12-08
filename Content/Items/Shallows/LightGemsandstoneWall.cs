using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.Shallows.LightGemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
