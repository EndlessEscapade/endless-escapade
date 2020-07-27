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
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
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
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin == default)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin = new Vector2(i + 12, j + 13) * 16 + new Vector2(8,-8);
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd = new Vector2(i + 12, j + 13) * 16 + new Vector2(8, -8);
                }
            }
            return true;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            if(Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd != default &&
                Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin != default)
            {
                Vector2 begin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
                Vector2 end = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
                for (float k = 0; k < 1; k += 0.01f)
                {
                    Main.spriteBatch.Draw(mod.GetTexture("Items/Zipline"), begin + (end - begin) *k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White, (end - begin).ToRotation(), new Rectangle(0, 0, 2, 2).Size() / 2, 1, SpriteEffects.None, 0);
                }
            }
        }
    }
}
