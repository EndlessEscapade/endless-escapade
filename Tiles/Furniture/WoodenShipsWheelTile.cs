using EEMod.Items.Materials;
using EEMod.Items.Materials.Fruit;
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

namespace EEMod.Tiles.Furniture
{
    public class WoodenShipsWheelTile : ModTile
    {
        public override void SetDefaults()
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
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wooden Ship's Wheel");
            AddMapEntry(new Color(255, 168, 28), name);
            dustType = 11;
            disableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<WoodenShipsWheel>());
        }

        private bool isIntersecting;
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            //int frameX = Main.tile[i, j].frameX;
            //int frameY = Main.tile[i, j].frameY;
            Player player = Main.LocalPlayer;
            if (new Rectangle((int)player.position.X / 16, (int)player.position.Y / 16, 1, 2).Intersects(new Rectangle(i, j, 1, 1)) && !isIntersecting)
                isIntersecting = true;
            else
                isIntersecting = false;

            if (isIntersecting)
            {
                //Main.projectile[player.GetModPlayer<EEPlayer>().Arrow2].ai[1] = 1;
                ArrowsUIState.OceanArrowVisible = true;

                if(EEMod.Inspect.Current && EEWorld.EEWorld.shipComplete)
                {

                    player.GetModPlayer<EEPlayer>().triggerSeaCutscene = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = mod.GetPacket();
                        netMessage.Write(player.GetModPlayer<EEPlayer>().triggerSeaCutscene);
                        netMessage.Send();
                    }
                }
            }
            else
            {
                //Main.projectile[player.GetModPlayer<EEPlayer>().Arrow2].ai[1] = 0;
                ArrowsUIState.OceanArrowVisible = false;
            }
        }
    }
}