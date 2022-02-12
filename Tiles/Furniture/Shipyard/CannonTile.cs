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
using EEMod.Items.Shipyard.Cannons;

namespace EEMod.Tiles.Furniture.Shipyard
{
    public class CannonTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;

            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Steel Cannon");
            AddMapEntry(new Color(255, 168, 28), name);
            DustType = DustID.Silver;
            DisableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, ItemID.DirtBlock);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero;

            Texture2D texture = EEMod.Instance.Assets.Request<Texture2D>("Tiles/Furniture/Shipyard/CannonTile").Value;



            int type = 0;

            if (Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().cannonType == ModContent.ItemType<Cannon>()) type = 0;
            if (Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().cannonType == ModContent.ItemType<LythenCannon>()) type = 1;

            Rectangle rect = new Rectangle(Framing.GetTileSafely(i, j).TileFrameX, Framing.GetTileSafely(i, j).TileFrameY + (type * 36), 16, 16);

            Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);

            return false;
        }
    }
}