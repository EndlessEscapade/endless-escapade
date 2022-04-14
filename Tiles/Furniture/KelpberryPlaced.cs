using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Items.Materials;

namespace EEMod.Tiles.Furniture
{
    public class KelpberryPlaced : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16
            };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName("Kelpberries");
            AddMapEntry(new Color(200, 180, 0), name);

            DustType = DustID.AmberBolt;
            ItemDrop = ModContent.ItemType<Kelpberries>();
        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.8f;
            b = 0.1f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero;
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Tiles/Furniture/KelpberryPlacedGlow").Value;
            Rectangle rect = new Rectangle(0, 0, 16, 16);
            Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}