﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using EEMod.Projectiles.CoralReefs;
using Terraria.ID;
using EEMod.Items.Materials.Fruit;
using EEMod.NPCs;
using EEMod.Items.Materials;

namespace EEMod.Tiles.Furniture
{
    public class ThinTropicalTree : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 10;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 20 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Tropical Tree");
            //drop = ModContent.ItemType<Moyai>();
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
        }


        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (Main.rand.Next(5) == 0)
            {
                NPC.NewNPC(i, j, ModContent.NPCType<Cococritter>());
            }
            Item.NewItem(new Vector2(i, j), ModContent.ItemType<TropicalWood>(), Main.rand.Next(12, 24));
            Item.NewItem(new Vector2(i, j), ModContent.ItemType<Coconut>(), Main.rand.Next(3, 5));
        }
    }
}
