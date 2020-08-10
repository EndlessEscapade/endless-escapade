using Terraria;
using Terraria.ObjectData;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using System;
using Terraria.ModLoader.IO;
using System.IO;

namespace EEMod.Tiles.Furniture.Coral
{
    public class GlowHangCoral1TE : ModTileEntity
    {
        public float kayLerp;
        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active();
        }
        public override void Update()
        {
            kayLerp += Main.rand.NextFloat(0, 0.001f);
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
            kayLerp = reader.ReadInt32();
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
            kayLerp = tag.GetInt(nameof(kayLerp));
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
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.newTile.Height = 11;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorBottom = default;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<GlowHangCoral1TE>().Hook_AfterPlacement, 1, 0, true);
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
                Color color = Color.White;
                Main.tile[i, j].frameX = 17;
                int index = ModContent.GetInstance<GlowHangCoral1TE>().Find(i, j);
                Main.NewText(TileEntity.ByID.Count);
                if (index == -1)
                {
                    return;
                }
                GlowHangCoral1TE TE = (GlowHangCoral1TE)TileEntity.ByID[index];
                int frameX = Main.tile[i, j].frameX;
                int frameY = Main.tile[i, j].frameY;
                int width = 20;
                int offsetY = 2;
                int height = 20;
                int offsetX = 2;

                Main.NewText(TE.kayLerp);
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                for (int k = 0; k < 7; k++)
                {
                    Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/Coral/GlowHangCoral1Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X + offsetX - (width - 16f) / 2f, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero, new Rectangle(frameX, frameY, width, height), color * ((float)Math.Sin(TE.kayLerp) * 0.5f + Main.rand.NextFloat(0, 0.5f)), 0f, default, 1f, SpriteEffects.None, 0f);
                }
            }
        }


    }
}