using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria.ModLoader;
using System.IO;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static EEMod.EEWorld.EEWorld;

namespace EEMod.Tiles.EmptyTileArrays
{
    public static class ETAHelpers
    {
        public enum ETAAnchor
        {
            Bottom,
            Top,
            Left,
            Right,
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }
        public static void PlaceCrystal(ETAAnchor Origin, Vector2 position, byte[,,] array, string CrystalTexture, string CrystalGlow)
        {
            int i = (int)position.X;
            int j = (int)position.Y;
            int width = array.GetLength(1);
            int height = array.GetLength(0);
            switch (Origin)
            {
                case ETAAnchor.Bottom:
                    if (CheckRangeRight(i, j, width))
                    {
                        if (!Framing.GetTileSafely(i, j - height).active() && !Framing.GetTileSafely(i + width - 1, j - 1).active())
                        {
                            EmptyTileEntities.Instance.AddPair(new Crystal(new Vector2(i, j - height), CrystalTexture, CrystalGlow), new Vector2(i, j - height), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i, j - height));
                        }
                    }
                    break;
                case ETAAnchor.Top:
                    if (CheckRangeRight(i, j, width))
                    {
                        if (!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i + width, j + 1 + height).active())
                        {
                            EmptyTileEntities.Instance.AddPair(new Crystal(new Vector2(i, j + 1), CrystalTexture, CrystalGlow), new Vector2(i, j + 1), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i, j + 1));
                        }
                    }
                    break;
                case ETAAnchor.Left:
                    if (CheckRangeDown(i, j, height))
                    {
                        if (!Framing.GetTileSafely(i - 1, j).active() && !Framing.GetTileSafely(i + 1 + width, j + height).active())
                        {
                            EmptyTileEntities.Instance.AddPair(new Crystal(new Vector2(i + 1, j), CrystalTexture, CrystalGlow), new Vector2(i + 1, j), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i + 1, j));
                        }
                    }
                    break;
                case ETAAnchor.Right:
                    if (CheckRangeDown(i, j, height))
                    {
                        if (!Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - width - 1, j + height).active())
                        {
                            EmptyTileEntities.Instance.AddPair(new Crystal(new Vector2(i - width, j), CrystalTexture, CrystalGlow), new Vector2(i - width, j), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i - width, j));
                        }
                    }
                    break;
                case ETAAnchor.BottomLeft:
                    if (CheckRangeDown(i - height, j, height) && CheckRangeRight(i, j, width))
                    {
                        if (!Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i, j - 1).active())
                        {
                            EmptyTileEntities.Instance.AddPair(new Crystal(new Vector2(i, j - height), CrystalTexture, CrystalGlow), new Vector2(i, j - height), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i, j - height));
                        }
                    }
                    break;
            }
        }

        /*public static void PlaceComplexCrystal(int width, int heihgt, int widthOfLedge, int heightOfLedge, int vert, int hori, bool dir, byte[,,] array)
        {
            int width = 18;
            int height = 18;
            int widthOfLedge = 5;
            int heightOfLedge = 6;
            int Vert = -5;
            int Hori = -6;
            Vector2 TopLeft = new Vector2(i - Hori - width, j - height - Vert);
            byte[,,] array = EmptyTileArrays.LuminantCoralCrystalBigTopLeft;
            if (CheckRangeRight(i, j, widthOfLedge) && CheckRangeDown(i, j, heightOfLedge))
            {
                for (int a = 0; a < array.GetLength(1); a++)
                {
                    for (int b = 0; b < array.GetLength(0); b++)
                    {
                        if (array[b, a, 0] == 1)
                        {
                            if (Main.tileSolid[Framing.GetTileSafely((int)TopLeft.X + a, (int)TopLeft.Y + b).type] && Framing.GetTileSafely((int)TopLeft.X + a, (int)TopLeft.Y + b).active())
                            {
                                check++;
                            }
                        }
                    }
                }
                if (check <= 11)
                {
                    if (!Framing.GetTileSafely((int)TopLeft.X, (int)TopLeft.Y).active() && !Framing.GetTileSafely((int)TopLeft.X + width + Vert, (int)TopLeft.Y + height + Hori).active())
                    {
                        EmptyTileEntityCache.AddPair(new BigCrystal(TopLeft, "Tiles/EmptyTileArrays/LuminantCoralCrystalBigTopLeft", "ShaderAssets/LuminantCoralCrystalBigTopLeftLightMap"), TopLeft, EmptyTileArrays.LuminantCoralCrystalBigTopLeft);
                    }
                    EESubWorlds.CoralCrystalPosition.Add(TopLeft);
                }
            }
        }*/
    }
}
