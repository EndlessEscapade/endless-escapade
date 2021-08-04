using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture
{
    public class OrbHolder : EETile
    {
        public class OrbHolderTE : EETileEntity
        {
            public bool hasOrb = false;

            public override bool ValidTile(int i, int j)
            {
                Tile tile = Framing.GetTileSafely(i, j);
                return tile.active();
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
                writer.Write(hasOrb);
            }

            public override void NetReceive(BinaryReader reader, bool lightReceive)
            {
                hasOrb = reader.ReadBoolean();
            }

            public override TagCompound Save()
            {
                return new TagCompound
                {
                    [nameof(hasOrb)] = hasOrb
                };
            }

            public override void Load(TagCompound tag)
            {
                hasOrb = tag.GetBool(nameof(hasOrb));
            }
        }

        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 9;
            TileObjectData.newTile.Height = 10;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<OrbHolderTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Orb Holder");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
            animationFrameHeight = 180;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            int x = i - tile.frameX / 18 % 9;
            int y = j - tile.frameY / 18 % 10;

            int targetTe = ModContent.GetInstance<OrbHolderTE>().Find(x, y);
            if (targetTe > -1 && TileEntity.ByID[targetTe] is OrbHolderTE TE)
            {
                if (TE.hasOrb)
                {
                    frameYOffset = Main.tileFrameCounter[Type] / 3 % 7 * animationFrameHeight;
                }
                else
                {
                    frameYOffset = 8 * animationFrameHeight;
                }
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<OrbHolderTE>().Kill(i, j);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
        }
    }
}