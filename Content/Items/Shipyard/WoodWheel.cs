using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shipyard;

public class WoodWheel : ModItem
{
    public override void SetDefaults() { Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Shipyard.WoodWheel>()); }
}
