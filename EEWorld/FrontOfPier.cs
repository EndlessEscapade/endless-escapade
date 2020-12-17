using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEMod.Tiles;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria.ID;
using static EEMod.EEWorld.EEWorld.FrontOfPierArrayVals;

namespace EEMod.EEWorld
{
    public partial class EEWorld
    {
        internal static class FrontOfPierArrayVals
        {
            internal const ushort A = 0;
            internal const ushort B = TileID.PalmWood;
            internal const ushort C = TileID.WoodBlock;
            internal const ushort D = TileID.HangingLanterns;
            internal const ushort E = TileID.BeachPiles;
            internal const ushort F = TileID.Banners;
            internal const ushort G = TileID.Ebonwood;
            internal const ushort H = TileID.LivingWood;
            internal const ushort I = TileID.Rope;
            internal const ushort J = TileID.Sand;
            internal const ushort A0 = 0;
            internal const ushort B1 = WallID.PalmWoodFence;
            internal const ushort C2 = WallID.RichMaogany;
            internal const ushort D3 = WallID.BorealWoodFence;
            internal const ushort E4 = WallID.EbonwoodFence;
            internal const ushort F5 = WallID.RichMahoganyFence;
            internal const ushort G6 = WallID.ShadewoodFence;
        }
        public static int[,,] FrontOfPier = new int[,,]
        {
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{C,0,0,0,0,0,0,0,144,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{C,0,0,2,0,0,0,0,162,18},{B,D3,28,0,0,0,0,0,36,36},{C,D3,0,0,0,0,0,0,216,36},{I,D3,0,0,0,0,0,0,144,72},{I,D3,0,0,0,0,0,0,126,72}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{D,E4,0,0,28,0,0,0,0,936},{F,B1,0,0,28,0,0,0,108,0},{0,B1,0,0,28,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{D,E4,0,0,28,0,0,0,0,954},{F,B1,0,0,28,0,0,0,108,18},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,E4,0,0,28,0,0,0,0,0},{F,B1,0,0,28,0,0,0,108,36},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,E4,0,0,28,0,0,0,0,0},{0,B1,0,0,28,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,E4,0,0,28,0,0,0,0,0},{0,B1,0,0,28,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,E4,0,0,28,0,0,0,0,0},{0,B1,0,0,28,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,E4,0,0,28,0,0,0,0,0},{0,B1,0,0,28,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{0,B1,0,0,0,0,0,0,0,0},{E,B1,0,0,0,0,0,0,44,0},{0,B1,0,0,28,0,0,0,0,0},{J,B1,0,0,28,0,0,0,36,54},{J,F5,0,0,28,0,0,0,36,0}},
{{B,0,0,4,0,0,0,0,162,36},{B,0,0,0,0,0,0,0,144,72},{B,0,0,0,0,0,0,0,144,72},{B,0,0,0,0,0,0,0,54,0},{B,0,0,0,0,0,0,0,36,36},{B,0,0,0,0,0,0,0,18,36}},
{{0,0,0,0,0,0,0,0,0,0},{0,C2,0,0,28,0,0,0,0,0},{0,C2,0,0,28,0,0,0,0,0},{G,C2,28,0,28,0,0,0,90,36},{0,C2,0,0,28,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0},{C,0,0,4,0,0,0,0,162,18},{C,0,0,0,0,0,0,0,144,72},{C,0,0,0,0,0,0,0,72,18},{0,F5,0,0,28,0,0,0,0,0},{C,G6,0,0,28,0,0,0,72,54}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{H,0,0,4,0,0,0,0,36,72},{H,F5,0,1,28,0,0,0,54,54},{H,F5,0,0,28,0,0,0,0,18}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{H,G6,0,4,28,0,0,0,36,72},{H,0,0,0,0,0,0,0,36,18}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,0,0,0,0,0,0,0,0,36}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,C2,0,0,28,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,C2,0,0,28,0,0,0,0,36}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,0,0,0,0,0,0,0,0,18}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,0,0,0,0,0,0,0,0,36}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,0,0,0,0,0,0,0,0,18}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,C2,0,0,28,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,C2,0,0,28,0,0,0,0,36}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,0,0,0,0,0,0,0,0,36}},
{{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0},{H,0,0,0,0,0,0,0,0,0}},
        };
    }
}