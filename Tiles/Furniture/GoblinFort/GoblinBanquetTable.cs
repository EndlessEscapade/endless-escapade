using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Materials;
using EEMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using EEMod.Items.Placeables.Ores;
using System;
using EEMod.Prim;
using EEMod.Projectiles.CoralReefs;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.NPCs.Goblins.Scrapwizard;

namespace EEMod.Tiles.Furniture.GoblinFort
{
    public class GoblinBanquetTable : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileTable[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 10;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;

            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;

            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Goblin Banquet Table");
            AddMapEntry(new Color(44, 193, 139), name);
            DustType = DustID.Clentaminator_Cyan;
            DisableSmartCursor = false;
            MinPick = 0;
            MineResist = 1f;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            bool safeToDraw = false;
            for (int n = 0; n < Main.maxProjectiles; n++)
            {
                if (Main.projectile[n].active)
                {
                    if (Main.projectile[n].type == ModContent.ProjectileType<PhantomTable>())
                    {
                        safeToDraw = true;
                    }
                }
            }

            if(safeToDraw)
            {
                Framing.GetTileSafely(i, j).IsActuated = true;

                return false;
            }
            else
            {
                Framing.GetTileSafely(i, j).IsActuated = false;

                return true;
            }
        }
    }
}