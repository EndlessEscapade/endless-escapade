using EEMod.Projectiles.CoralReefs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.Coral
{
    public class Floor6x8Coral : ModTile
    {
        private bool isIntersecting;
        private int cooldown = 180;

        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 8;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Big Coral");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = 107;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;
            Player player = Main.LocalPlayer;
            if (new Rectangle((int)player.position.X / 16, (int)player.position.Y / 16, 1, 2).Intersects(new Rectangle(i, j, 1, 1)) && !isIntersecting && (player.velocity.X > 7 || player.velocity.X < -7))
            {
                for (int a = 0; a < 8; a++)
                {
                    Projectile.NewProjectile(new Vector2((i * 16) - frameX, (j * 16) - frameY), Vector2.Zero, ModContent.ProjectileType<CBPetrude>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.03f, 0.1f), Main.rand.Next(100, 180));
                }

                isIntersecting = true;
            }
            if (isIntersecting)
            {
                cooldown--;
                if (cooldown % 15 == 0)
                {
                    for (int a = 0; a < 2; a++)
                    {
                        Projectile.NewProjectile(new Vector2((i * 16) - frameX, (j * 16) - frameY), Vector2.Zero, ModContent.ProjectileType<CBPetrude>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.03f, 0.1f), Main.rand.Next(100, 180));
                    }
                }

                if (cooldown == 0)
                {
                    isIntersecting = false;
                    cooldown = 180;
                }
            }
        }
    }
}