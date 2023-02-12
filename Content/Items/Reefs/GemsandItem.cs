﻿using EndlessEscapade.Content.Tiles.Reefs;
using EndlessEscapade.Content.Tiles.Reefs.Kelp;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs;

public class GemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<GemsandTile>());
    }
}