﻿using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class GemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.GemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
