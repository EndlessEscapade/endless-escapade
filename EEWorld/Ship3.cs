using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEMod.Tiles;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria.ID;
using EEMod.Tiles.Furniture;
using EEMod.Tiles.Walls;

namespace EEMod.EEWorld
{
    public class Ship3
    {
        static readonly int A = 0;
        static readonly int B = ModContent.TileType<CoralSandTile>();
        static readonly int C = TileID.LivingMahoganyLeaves;
        static readonly int D = ModContent.TileType<CoralSandTile>();
        static readonly int E = TileID.PalmWood;
        static readonly int F = TileID.Ebonwood;
        static readonly int G = TileID.RichMahogany;
        static readonly int H = TileID.WoodBlock;
        static readonly int I = TileID.Chain;
        static readonly int J = TileID.Rope;
        static readonly int K = TileID.DyePlants;
        static readonly int L = TileID.Platforms;
        static readonly int M = ModContent.TileType<CoralChestTile>();
        static readonly int A0 = 0;
        static readonly int B1 = ModContent.WallType<CoralSandWallTile>();
        static readonly int C2 = WallID.LivingLeaf;
        static readonly int D3 = WallID.Ebonwood;
        static readonly int E4 = WallID.EbonwoodFence;
        static readonly int F5 = WallID.Wood;
        static readonly int G6 = WallID.Planked;
        static readonly int H7 = WallID.PalmWoodFence;
        static readonly int I8 = WallID.RichMaogany;
        static readonly int J9 = WallID.PalmWood;
        static readonly int K10 = WallID.ShadewoodFence;
        static readonly int[] Zero = new int[]{0,0,0,0,0,0,0,0,0,0};
        public static int[,,] Ship3Array = new int[,,]
        {
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,K10,28,0,0,0,0,0,198,54},{J,A0,0,0,0,0,0,0,162,36},{J,A0,0,0,0,0,0,0,144,72},{J,A0,0,0,0,0,0,0,144,72},{J,A0,0,0,0,0,0,0,144,72},{J,A0,0,0,0,0,0,0,90,54},{A,A0,0,0,0,0,0,0,0,0},{A,H7,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,H7,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,K10,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,H7,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{J,H7,0,0,0,0,0,0,90,18},{A,A0,0,0,0,0,0,0,0,0},{A,H7,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,K10,28,0,0,0,0,0,198,54},{J,A0,0,0,0,0,0,0,162,18},{J,A0,0,0,0,0,0,0,126,72},{J,A0,0,0,0,0,0,0,108,72},{J,A0,0,0,0,0,0,0,54,54},{A,A0,0,0,0,0,0,0,0,0},{C,A0,20,0,0,0,0,0,0,54},{C,A0,20,1,0,0,0,0,18,54},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{A,H7,0,0,28,0,0,0,0,0},{A,H7,0,0,28,0,0,0,0,0},{A,K10,0,0,28,0,0,0,0,0},{B,B1,0,0,0,0,0,0,0,54},{B,H7,0,0,0,0,0,0,126,72},{B,A0,0,0,0,0,0,0,90,54},{A,A0,0,0,0,0,0,0,0,0},{J,H7,0,0,28,0,0,0,144,54},{A,H7,0,0,28,0,0,0,0,0},{A,H7,0,0,28,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{A,K10,0,0,28,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{J,K10,0,0,28,0,0,0,144,54},{C,A0,20,0,0,0,0,0,72,54},{C,A0,20,0,0,0,0,0,54,18},{C,A0,20,0,0,0,0,0,72,18},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,126,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,E4,0,0,28,0,0,0,0,0},{F,H7,28,0,28,0,0,0,36,54},{E,A0,28,0,0,0,0,0,54,0},{G,A0,28,0,0,0,0,0,18,0},{E,A0,28,0,0,0,0,0,54,0},{H,A0,0,0,0,0,0,0,54,0},{E,A0,28,0,0,0,0,0,54,18},{E,B1,28,1,0,0,0,0,54,54},{B,B1,0,0,0,0,0,0,0,0},{B,B1,0,0,0,0,0,0,54,0},{B,E4,0,0,28,0,0,0,90,54},{H,H7,0,0,0,0,0,0,36,54},{E,H7,28,0,0,0,0,0,36,0},{H,A0,28,0,0,0,0,0,36,0},{F,A0,28,0,0,0,0,0,54,0},{E,A0,28,0,0,0,0,0,54,0},{E,E4,28,1,28,0,0,0,18,54},{C,H7,20,0,28,0,0,0,162,54},{C,K10,20,2,28,0,0,0,72,54},{C,H7,20,0,0,0,0,0,180,18},{C,A0,20,0,0,0,0,0,54,18},{C,A0,20,0,0,0,0,0,36,18},{C,H7,20,0,0,0,0,0,18,54},{I,K10,28,0,28,0,0,0,108,54},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{E,A0,28,2,0,0,0,0,162,36},{G,A0,28,0,0,0,0,0,126,72},{E,A0,28,0,0,0,0,0,54,36},{H,A0,0,0,0,0,0,0,54,18},{E,A0,28,0,0,0,0,0,18,18},{F,A0,28,0,0,0,0,0,54,18},{H,A0,28,0,0,0,0,0,54,36},{E,B1,28,3,0,0,0,0,18,72},{H,B1,28,0,0,0,0,0,0,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,36,18},{H,B1,0,0,0,0,0,0,18,18},{E,A0,28,0,0,0,0,0,54,18},{F,A0,28,0,0,0,0,0,54,36},{G,A0,28,0,0,0,0,0,18,36},{E,A0,28,0,0,0,0,0,54,18},{F,A0,28,0,0,0,0,0,36,18},{G,A0,28,0,0,0,0,0,54,18},{H,A0,28,0,0,0,0,0,18,18},{E,H7,28,0,0,0,0,0,72,0},{C,H7,20,0,0,0,0,0,72,72},{C,H7,20,0,0,0,0,0,54,36},{C,H7,20,0,0,0,0,0,54,36},{C,H7,20,0,0,0,0,0,54,72},{G,H7,28,0,0,0,0,0,144,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,F5,0,0,28,0,0,0,0,0},{A,G6,0,0,0,0,0,0,0,0},{E,G6,28,4,0,0,0,0,72,72},{F,A0,28,0,0,0,0,0,36,36},{E,A0,28,0,0,0,0,0,72,18},{A,G6,0,0,0,0,0,0,0,0},{A,G6,0,0,0,0,0,0,0,0},{B,B1,0,0,0,0,0,0,72,72},{D,A0,8,0,0,0,0,0,54,18},{B,B1,0,0,0,0,0,0,72,36},{E,B1,28,4,0,0,0,0,36,72},{F,A0,28,0,0,0,0,0,54,72},{A,F5,0,0,28,0,0,0,0,0},{H,G6,28,0,0,0,0,0,162,18},{H,G6,28,0,0,0,0,0,36,36},{H,A0,0,0,0,0,0,0,54,18},{G,A0,28,0,0,0,0,0,54,18},{E,A0,28,0,0,0,0,0,54,18},{G,A0,28,0,0,0,0,0,18,18},{H,A0,28,0,0,0,0,0,36,0},{F,A0,28,0,0,0,0,0,36,0},{E,A0,28,0,0,0,0,0,54,0},{H,A0,28,0,0,0,0,0,18,0},{E,K10,28,0,28,0,0,0,54,36},{F,A0,28,0,0,0,0,0,108,72}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,J9,0,0,28,0,0,0,0,0},{A,D3,0,0,28,0,0,0,0,0},{H,I8,28,3,28,0,0,0,108,54},{A,I8,0,0,28,0,0,0,0,0},{A,D3,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{B,B1,0,4,0,0,0,0,0,72},{B,B1,0,3,0,0,0,0,18,72},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,I8,0,0,28,0,0,0,0,0},{M,A0,28,0,0,0,0,0,0,0},{M,B1,28,0,0,0,0,0,18,0},{E,B1,28,0,0,0,0,0,0,18},{H,A0,28,0,0,0,0,0,18,18},{H,A0,28,0,0,0,0,0,18,18},{E,K10,28,0,28,0,0,0,18,18},{E,A0,28,0,0,0,0,0,54,36},{G,A0,28,0,0,0,0,0,108,36},{E,A0,28,0,0,0,0,0,18,36},{E,A0,28,3,0,0,0,0,18,72},{I,K10,28,0,28,0,0,0,144,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,F5,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,I8,0,0,28,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,J9,0,0,28,0,0,0,0,0},{A,I8,0,0,28,0,0,0,0,0},{M,A0,28,0,0,0,0,0,0,18},{M,B1,28,0,0,0,0,0,18,18},{F,A0,28,0,0,0,0,0,0,36},{E,A0,28,0,0,0,0,0,36,18},{F,A0,28,0,0,0,0,0,36,18},{E,K10,0,3,28,0,0,0,90,72},{A,A0,0,0,0,0,0,0,0,0},{G,A0,28,4,0,0,0,0,144,54},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,36},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,I8,0,0,28,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,J9,0,0,28,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,F5,0,0,28,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{L,B1,28,0,0,0,0,0,36,180},{L,A0,28,0,0,0,0,0,72,180},{H,A0,0,0,0,0,0,0,72,54},{E,A0,28,0,0,0,0,0,36,36},{H,A0,28,0,0,0,0,0,36,18},{E,A0,28,0,0,0,0,0,90,72},{I,K10,28,0,28,0,0,0,126,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,I8,0,0,28,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,F5,0,0,28,0,0,0,0,0},{A,G6,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,J9,0,0,28,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{K,F5,8,0,28,0,0,0,272,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{E,D3,28,2,28,0,0,0,0,54},{H,A0,0,0,0,0,0,0,18,18},{E,A0,28,1,0,0,0,0,216,0},{E,A0,0,3,0,0,0,0,126,54},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,36},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,36},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,G6,0,0,0,0,0,0,0,0},{A,G6,0,0,0,0,0,0,0,0},{A,I8,0,0,28,0,0,0,0,0},{A,C2,0,0,20,0,0,0,0,0},{C,C2,20,0,20,0,0,0,72,54},{C,C2,20,0,20,0,0,0,54,0},{C,C2,20,1,20,0,0,0,54,54},{A,A0,0,0,0,0,0,0,0,0},{B,B1,0,0,0,0,0,0,36,54},{B,B1,0,0,0,0,0,0,54,0},{B,B1,0,0,0,0,0,0,108,72},{B,B1,0,0,0,0,0,0,54,54},{F,B1,28,2,0,0,0,0,36,54},{F,A0,28,0,0,0,0,0,54,18},{E,A0,0,3,0,0,0,0,54,72},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,18},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,0},{A,A0,0,0,0,0,0,0,0,0}},
            {{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{C,A0,20,0,0,0,0,0,108,0},{A,A0,0,0,0,0,0,0,0,0},{E,A0,28,4,0,0,0,0,162,0},{F,D3,28,0,28,0,0,0,108,72},{G,A0,28,0,0,0,0,0,18,0},{E,G6,28,1,0,0,0,0,18,54},{C,C2,20,0,20,0,0,0,162,0},{C,C2,20,0,20,0,0,0,144,72},{C,C2,20,0,20,0,0,0,180,0},{C,C2,20,0,20,0,0,0,54,18},{C,C2,20,0,20,0,0,0,18,36},{B,B1,0,0,0,0,0,0,18,0},{B,B1,0,0,0,0,0,0,54,18},{B,B1,0,0,0,0,0,0,72,18},{B,B1,0,2,0,0,0,0,72,54},{B,B1,0,0,0,0,0,0,18,36},{H,A0,0,0,0,0,0,0,18,18},{G,A0,28,0,0,0,0,0,54,72},{A,A0,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,A0,0,0,0,0,0,0,0,0},{I,A0,28,0,0,0,0,0,90,18},{A,A0,0,0,0,0,0,0,0,0},{B,A0,0,2,0,0,0,0,36,54},{B,A0,0,0,0,0,0,0,36,0},{B,A0,0,0,0,0,0,0,36,0},{B,A0,0,0,0,0,0,0,18,0},{B,A0,0,0,0,0,0,0,18,54}},
            {{A,A0,0,0,0,0,0,0,0,0},{C,A0,20,0,0,0,0,0,144,0},{C,A0,20,2,0,0,0,0,36,54},{C,A0,20,0,0,0,0,0,108,18},{C,A0,20,0,0,0,0,0,18,54},{A,A0,0,0,0,0,0,0,0,0},{E,D3,28,0,28,0,0,0,36,54},{H,A0,0,0,0,0,0,0,0,0},{E,A0,28,0,0,0,0,0,18,36},{H,A0,0,0,0,0,0,0,36,18},{E,A0,28,0,0,0,0,0,72,18},{C,C2,20,0,20,0,0,0,72,72},{C,C2,20,0,20,0,0,0,36,18},{C,C2,20,1,20,0,0,0,54,54},{B,A0,0,0,0,0,0,0,0,0},{B,B1,0,0,0,0,0,0,36,36},{D,B1,8,0,0,0,0,0,54,18},{B,B1,0,3,0,0,0,0,18,72},{G,B1,28,0,0,0,0,0,0,54},{H,A0,0,0,0,0,0,0,0,36},{H,B1,0,0,0,0,0,0,54,54},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{A,B1,0,0,0,0,0,0,0,0},{B,B1,0,0,0,0,0,0,72,54},{B,A0,0,0,0,0,0,0,36,0},{B,B1,0,0,0,0,0,0,36,0},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,54,18},{D,A0,8,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18}},
            {{B,A0,0,0,0,0,0,0,72,54},{B,A0,0,0,0,0,0,0,36,18},{D,A0,8,0,0,0,0,0,72,18},{C,B1,20,0,0,0,0,0,36,72},{C,A0,20,0,0,0,0,0,36,18},{B,C2,0,0,20,0,0,0,18,0},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,90,54},{E,A0,28,0,0,0,0,0,0,0},{F,A0,28,0,0,0,0,0,18,18},{E,A0,28,0,0,0,0,0,18,0},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,54,18},{B,B1,0,0,0,0,0,0,18,18},{B,B1,0,1,0,0,0,0,18,54},{H,A0,0,0,0,0,0,0,0,0},{E,A0,28,0,0,0,0,0,36,0},{H,A0,0,0,0,0,0,0,36,18},{E,A0,28,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,0},{B,A0,0,0,0,0,0,0,18,0},{B,B1,0,0,0,0,0,0,36,0},{B,B1,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,54,18},{D,A0,8,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,36,18}},
            {{B,A0,0,0,0,0,0,0,0,36},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,18},{D,B1,8,0,0,0,0,0,54,54},{C,A0,20,0,0,0,0,0,0,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,18,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,36,18},{B,A0,0,0,0,0,0,0,54,18},{B,A0,0,0,0,0,0,0,18,18}},
        };
    }
}
