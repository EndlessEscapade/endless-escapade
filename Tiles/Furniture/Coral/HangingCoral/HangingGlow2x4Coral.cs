using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace EEMod.Tiles.Furniture.Coral.HangingCoral
{
    public class HangingGlow2x4Coral : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16
            };

            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coral Lamp");
            AddMapEntry(new Color(0, 100, 200), name);
            dustType = DustID.Dirt;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.9f;
            b = 0.9f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Texture2D texture = EEMod.instance.GetTexture("Tiles/Furniture/Coral/HangingCoral/HangingGlow2x4CoralGlow");
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);
            Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}