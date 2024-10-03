﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Materials;

public class LythenOre : ModItem
{
    public override void SetStaticDefaults() {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
    }

    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LythenOre>());

        Item.width = 32;
        Item.height = 28;
    }
}
