using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using InteritosMod.Items.Placeables;
using Microsoft.Xna.Framework;

namespace InteritosMod.Tiles
{
    public class AtlanteanCrystal : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);

            AddMapEntry(new Color(67, 47, 155));

            dustType = 154;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}