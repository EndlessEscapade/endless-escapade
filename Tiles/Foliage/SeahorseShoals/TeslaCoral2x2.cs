using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using Terraria.ID;
using EEMod.Prim;
using EEMod.Projectiles.CoralReefs;

namespace EEMod.Tiles.Foliage.SeahorseShoals
{
    public class TeslaCoral2x2 : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorTop = default;
            //TileObjectData.newTile.RandomStyleRange = 6;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 0)
            {
                if (Main.rand.NextBool(20))
                {
                    for (int k = -20; k < 20; k++)
                    {
                        for (int l = -20; l < 20; l++)
                        {
                            if (WorldGen.InWorld(i + k, j + l) && Main.tile[i + k, j + l].IsActive && Main.tile[i + k, j + l].type == ModContent.TileType<TeslaCoral2x2>() && Main.tile[i + k, j + l].frameX == 0 && Main.tile[i + k, j + l].frameY == 0)
                            {
                                int lightningproj = Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j), new Vector2((i * 16) + 16, (j * 16) + 16), Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), 20, 2.5f);

                                if (Main.netMode != NetmodeID.Server)
                                {
                                    PrimSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj]));
                                }

                                TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                                zappy.target = new Vector2(((i + k) * 16) + 16, ((j + l) * 16) + 16);

                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}