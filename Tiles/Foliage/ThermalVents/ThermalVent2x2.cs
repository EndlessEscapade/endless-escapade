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
    public class ThermalVent2x2 : EETile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Thermal Vent");
            AddMapEntry(new Color(255, 100, 0), name);
            dustType = DustID.Dirt;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.40f;
            g = 0.08f;
            b = 0.13f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Helpers.DrawTileGlowmask(Mod.Assets.Request<Texture2D>("Tiles/Foliage/ThermalVents/ThermalVent2x2Glow").Value, i, j);
        }
    }
}