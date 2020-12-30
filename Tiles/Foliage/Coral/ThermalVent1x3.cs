using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.Coral
{
    public class ThermalVent1x3 : ModTile
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
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Thermal Vent");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
        }

        private int lmnop = 0;

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.tile[i, j].frameY == 0 && Main.rand.Next(3) == 0)
            {
                int num = Dust.NewDust(new Vector2(i * 16 + 4, j * 16), 1, 1, DustID.Smoke, 0, 1);
                Main.dust[num].velocity = new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), -1f);
                Main.dust[num].scale = 2;
            }
            lmnop++;
            if (Math.Abs(Main.LocalPlayer.Center.X - (i * 16)) <= 16 && lmnop >= 60 && Main.LocalPlayer.Center.Y - (j * 16) <= -640)
            {
                Projectile.NewProjectile(new Vector2((i * 16) + 8, (j * 16) + 0), new Vector2(0, -5), ProjectileID.GeyserTrap, 20, 2f);
                lmnop = 0;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<ThermalHealing>(), 130);
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Helpers.DrawTileGlowmask(mod.GetTexture("Tiles/Foliage/Coral/ThermalVent1x3Glow"), i, j);
        }
    }
}