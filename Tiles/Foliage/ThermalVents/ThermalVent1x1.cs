using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;

namespace EEMod.Tiles.Foliage.ThermalVents
{
    public class ThermalVent1x1 : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16
            };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Thermal Vent");
            AddMapEntry(new Color(255, 100, 0), name);
            DustType = DustID.Dirt;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.40f;
            g = 0.08f;
            b = 0.13f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Helpers.DrawTileGlowmask(Mod.Assets.Request<Texture2D>("Tiles/Foliage/ThermalVents/ThermalVent1x1Glow").Value, i, j);
        }
    }
}