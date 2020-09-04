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
                if (!Main.LocalPlayer.GetModPlayer<EEPlayer>().holdingPylon)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        if (EEWorld.EEWorld.PylonBegin[k] == default(Vector2))
                        {
                            EEWorld.EEWorld.PylonBegin[k] = new Vector2(i, j) * 16 + new Vector2(8, -8);
                            Main.LocalPlayer.GetModPlayer<EEPlayer>().holdingPylon = true;
                            break;
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < 100; k++)
                    {
                        if (EEWorld.EEWorld.PylonEnd[k] == default(Vector2) && new Vector2(i, j) * 16 + new Vector2(8, -8) != EEWorld.EEWorld.PylonBegin[k])
                        {
                            EEWorld.EEWorld.PylonEnd[k] = new Vector2(i, j) * 16 + new Vector2(8, -8);
                            Main.LocalPlayer.GetModPlayer<EEPlayer>().holdingPylon = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                Main.LocalPlayer.position = new Vector2(i, j) * 16;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline = true;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin = new Vector2(i, j) * 16 + new Vector2(8, -8);
                for (int k = 0; k < 100; k++)
                {
                    if (EEWorld.EEWorld.PylonBegin[k] == new Vector2(i, j) * 16 + new Vector2(8, -8))
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd = EEWorld.EEWorld.PylonEnd[k];
                    }
                }
            }
            return true;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            for (int l = 0; l < 100; l++)
            {
                if (EEWorld.EEWorld.PylonBegin[l] != default && EEWorld.EEWorld.PylonEnd[l] != default)
                {
                    Vector2 begin = EEWorld.EEWorld.PylonBegin[l];
                    Vector2 end = EEWorld.EEWorld.PylonEnd[l];
                    Vector2 endbegindistance = end - begin;
                    float n = 1 / endbegindistance.Length();
                    // float ebdistrot = endbegindistance.ToRotation();
                    //Texture2D texture = TextureCache.Zipline;
                    for (float k = 0; k < 1; k += n)
                    {
                        Main.spriteBatch.Draw(TextureCache.Zipline, begin + (end - begin) * k - Main.screenPosition + (new Vector2(11.5f, 13.5f) * 16) + new Vector2(8, -8), new Rectangle(0, 0, 2, 2), Color.White, (end - begin).ToRotation(), new Vector2(2, 2) / 2, 1, SpriteEffects.None, 0);
                        //Main.spriteBatch.Draw(texture, begin + endbegindistance * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White, ebdistrot, Vector2.One, 1, SpriteEffects.None, 0);
                    }
                }
            }
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().holdingPylon && Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin == new Vector2(i, j) * 16 + new Vector2(8, -8))
            {
                Main.spriteBatch.Draw(TextureCache.Zipline, Main.LocalPlayer.position + Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White, (Main.LocalPlayer.position - Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin).ToRotation(), new Vector2(2, 2) / 2, 1, SpriteEffects.None, 0);
            }
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //for (int k = 0; k < 100; k++)
            //{
            //    if (EEWorld.EEWorld.PylonBegin[k] == new Vector2(i, j) * 16 + new Vector2(8, -8))
            //    {

            //    }
            //}
        }
    }
}
