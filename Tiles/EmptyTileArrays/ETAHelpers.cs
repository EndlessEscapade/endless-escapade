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
                            EmptyTileEntityCache.AddPair(new Crystal(new Vector2(i, j - height), CrystalTexture, CrystalGlow), new Vector2(i, j - height), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i, j - height));
                        }
                    }
                    break;
                case ETAAnchor.Top:
                    if (CheckRangeRight(i, j, width))
                    {
                        if (!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i + width, j + 1 + height).active())
                        {
                            EmptyTileEntityCache.AddPair(new Crystal(new Vector2(i, j + 1), CrystalTexture, CrystalGlow), new Vector2(i, j + 1), array);
                            EESubWorlds.CoralCrystalPosition.Add(new Vector2(i, j + 1));
                        }
                    }
                    break;
            }
        }
    }
}
