using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture.Coral.HangingCoral
{
    public class GlowHangCoral1TE : ModTileEntity
    {
        public float kayLerp;

        public override bool ValidTile(int i, int j)
        {
            if (WorldGen.InWorld(i, j))
            {
                Tile tile = Main.tile[i, j];
                return tile.active();
            }
            else
                return false;
        }

        public override void Update()
        {
            kayLerp += Main.rand.NextFloat(0, 0.04f);
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }

            return Place(i, j);
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            writer.Write(kayLerp);
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            kayLerp = reader.ReadSingle();
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                [nameof(kayLerp)] = kayLerp
            };
        }

        public override void Load(TagCompound tag)
        {
            kayLerp = tag.GetFloat(nameof(kayLerp));
        }
    }

    public class GlowHangCoral1 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Height = 11;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coral Lamp");
            AddMapEntry(new Color(0, 100, 200), name);
            dustType = DustID.Dirt;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.frameX < 18)
            {
                r = 0.05f;
                g = 0.05f;
                b = 0.05f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (tile != null && tile.active() && tile.type == Type)
            {
                int frameX = tile.frameX;
                int frameY = tile.frameY;
                const int width = 20;
                const int offsetY = 0;
                const int height = 16;
                int offsetX = 0;
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                tile.frameX = 17;
                Color color = Color.White;
                int index = ModContent.GetInstance<GlowHangCoral1TE>().Find(i, j - tile.frameY / 16);
                if (index == -1)
                {
                    return;
                }
                GlowHangCoral1TE TE = (GlowHangCoral1TE)TileEntity.ByID[index];
                Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X + offsetX - (width - 16f) / 2f, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero;
                Rectangle rect = new Rectangle(frameX, frameY, width, height);
                color *= (float)Math.Sin(TE.kayLerp) * 0.5f + 0.5f;
                for (int k = 0; k < 7; k++)
                {
                    Main.spriteBatch.Draw(EEMod.instance.GetTexture("Tiles/Furniture/Coral/HangingCoral/GlowHangCoral1Glow"), position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}