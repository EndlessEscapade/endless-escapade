using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Buffs.Buffs;

namespace EEMod.Tiles.Furniture
{
    public class ThermalVent : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            //TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Thermal Vent");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.tile[i, j].frameY == 0 && Main.rand.Next(3) == 0)
            {
                int num = Dust.NewDust(new Vector2(i * 16 + 4, j * 16), 1, 1, DustID.Smoke, 0, 1);
                Main.dust[num].velocity = new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), -1f);
                Main.dust[num].scale = 2;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<ThermalHealing>(), 130);
            }
        }
    }
}
