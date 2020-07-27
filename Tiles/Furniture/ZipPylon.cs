using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Items;
using System.Windows;
using System;

namespace EEMod.Tiles.Furniture
{
    public class ZipPylon : ModTile
    {
        public Vector2 link = new Vector2();
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            TileObjectData.newSubTile.LavaDeath = false;
            TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Zip-Pylon");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
        }

        public override bool NewRightClick(int i, int j)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ZipWire>())
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().currentZipPylon != default)
                {
                    Main.NewText(Main.LocalPlayer.GetModPlayer<EEPlayer>().currentZipPylon);
                    link = Main.LocalPlayer.GetModPlayer<EEPlayer>().currentZipPylon;
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().currentZipPylon = default;
                    Main.NewText(link);
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().currentZipPylon = new Vector2(i, j);
                    Main.NewText(link);
                }
            }
            return true;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            if(link != default)
            {
                Vector2 pos = new Vector2(i , j) * 16;
                Vector2 newLink = link * 16;
                Main.spriteBatch.Draw(mod.GetTexture("Items/Zipline"), pos - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White, MathHelper.ToDegrees((float)Math.Acos((pos.X * newLink.X + pos.Y * newLink.Y) / (pos.Length() * newLink.Length()))), new Rectangle(0, 0, 2, 2).Size() / 2, new Vector2(Vector2.Distance(pos, newLink), 1), SpriteEffects.None, 0);
            }
        }
    }
}
