﻿using EndlessEscapade.Content.Walls.Reefs.DarkestTrenches;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.DarkestTrenches;

public class DarkGemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<DarkGemsandWall>());
    }
}