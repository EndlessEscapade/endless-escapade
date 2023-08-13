using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Shallows;

public class LightGemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.Shallows.LightGemsandstoneWall>());
    }
}