using EEMod.Items.Materials;
using EEMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Placeables.Furniture;
using EEMod.EEWorld;
using EEMod.UI.States;
using EEMod.Systems.Subworlds.EESubworlds;

namespace EEMod.Tiles.Furniture
{
    public class WoodenShipsWheelTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wooden Ship's Wheel");
            AddMapEntry(new Color(255, 168, 28), name);
            DustType = DustID.Silver;
            DisableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<WoodenShipsWheel>());
        }

        /*private bool isIntersecting;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //int frameX = Framing.GetTileSafely(i, j).frameX;
            //int frameY = Framing.GetTileSafely(i, j).frameY;

            Player player = Main.LocalPlayer;
            if (new Rectangle((int)player.position.X, (int)player.position.Y, 32, 48).Intersects(new Rectangle(i * 16, j * 16, 32, 48)) && !isIntersecting)
                isIntersecting = true;
            else
                isIntersecting = false;

            if (isIntersecting)
            {
                //Main.projectile[player.GetModPlayer<EEPlayer>().Arrow2].ai[1] = 1;

                player.GetModPlayer<EEPlayer>().triggerSeaCutscene = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = Mod.GetPacket();
                    netMessage.Write(player.GetModPlayer<EEPlayer>().triggerSeaCutscene);
                    netMessage.Send();
                }

                //Main.LocalPlayer.GetModPlayer<EEPlayer>().Initialize();
                //SubworldManager.EnterSubworld<Sea>();
            }
            else
            {
                //Main.projectile[player.GetModPlayer<EEPlayer>().Arrow2].ai[1] = 0;
            }

            return true;
        }*/

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            player.GetModPlayer<EEPlayer>().triggerSeaCutscene = true;
            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = Mod.GetPacket();
                netMessage.Write(player.GetModPlayer<EEPlayer>().triggerSeaCutscene);
                netMessage.Send();
            }

            return true;
        }
    }
}