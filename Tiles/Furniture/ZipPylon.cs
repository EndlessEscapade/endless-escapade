using EEMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles.Furniture
{
    public class ZipPylon : EETile
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
                        if (EEWorld.EEWorld.PylonBegin[k] == default)
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
                        if (EEWorld.EEWorld.PylonEnd[k] == default && new Vector2(i, j) * 16 + new Vector2(8, -8) != EEWorld.EEWorld.PylonBegin[k])
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
            Texture2D zipline = ModContent.GetInstance<EEMod>().GetTexture("Items/Zipline");
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
                        Main.spriteBatch.Draw(zipline, begin + (end - begin) * k - Main.screenPosition + new Vector2(11.5f * 16 + 8, 13.5f * 16 - 8), new Rectangle(0, 0, 2, 2), Color.White, (end - begin).ToRotation(), Vector2.One, 1, SpriteEffects.None, 0);
                        //Main.spriteBatch.Draw(texture, begin + endbegindistance * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White, ebdistrot, Vector2.One, 1, SpriteEffects.None, 0);
                    }
                }
            }
            EEPlayer eeplayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (eeplayer.holdingPylon && eeplayer.PylonBegin == new Vector2(i, j) * 16 + new Vector2(8, -8))
            {
                Main.spriteBatch.Draw(zipline, Main.LocalPlayer.position + Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White, (Main.LocalPlayer.position - eeplayer.PylonBegin).ToRotation(), Vector2.One, 1, SpriteEffects.None, 0);
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