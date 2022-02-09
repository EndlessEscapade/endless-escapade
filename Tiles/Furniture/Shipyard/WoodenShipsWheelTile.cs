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

namespace EEMod.Tiles.Furniture.Shipyard
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
            DisableSmartCursor = false;
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<WoodenShipsWheel>());
        }

        public override string HighlightTexture => "EEMod/Tiles/Furniture/WoodenShipsWheelTile_Highlight";

        public override bool HasSmartInteract()
        {
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            player.GetModPlayer<EEPlayer>().triggerSeaCutscene = true;

            //player.GetModPlayer<EEPlayer>().ScanBoat();

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            player.cursorItemIconText = "Wooden Ship's Wheel";
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                // player.showItemIcon = false;
                player.cursorItemIconID = 0;
            }
        }
    }
}