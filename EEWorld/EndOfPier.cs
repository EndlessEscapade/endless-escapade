using static EEMod.EEWorld.EEWorld.EndOfPierVals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEMod.Tiles;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria.ID;
namespace EEMod.EEWorld
{
public partial class EEWorld
{
internal static class EndOfPierVals
{
internal const ushort A = 0;
internal const ushort B = TileID.Rope;
internal const ushort C = TileID.Sand;
internal const ushort D = TileID.PalmWood;
internal const ushort E = TileID.WoodBlock;
internal const ushort F = TileID.LivingWood;
internal const ushort G = TileID.Banners;
internal const ushort H = TileID.Ebonwood;
internal const ushort I = TileID.HangingLanterns;
internal const ushort J = TileID.Lamps;
internal const ushort K = TileID.FishingCrate;
internal const ushort A0 = 0;
internal const ushort B1 = WallID.BorealWoodFence;
internal const ushort C2 = WallID.RichMahoganyFence;
internal const ushort D3 = WallID.ShadewoodFence;
internal const ushort E4 = WallID.EbonwoodFence;
internal const ushort F5 = WallID.RichMaogany;
internal const ushort G6 = WallID.PalmWoodFence;
}
public static int[,,] EndOfPier = new int[,,]
{
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{E,0,0,0,0,0,0,0,144,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{B,B1,0,0,0,0,0,0,126,72,252,144},{B,B1,0,0,0,0,0,0,144,72,36,0},{E,B1,0,0,0,0,0,0,162,0,72,0},{D,E4,28,0,0,0,0,0,18,36,180,108},{E,0,0,1,0,0,0,0,216,18,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0,0,144},{G,G6,0,0,28,0,0,0,72,0,396,72},{I,E4,28,0,28,0,0,0,18,936,144,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{G,G6,0,0,28,0,0,0,72,18,0,72},{I,E4,28,0,28,0,0,0,18,954,144,36},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{G,G6,0,0,28,0,0,0,72,36,0,0},{0,E4,0,0,28,0,0,0,0,0,144,36},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0,0,72},{0,E4,0,0,28,0,0,0,0,0,144,72},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0,0,72},{0,E4,0,0,28,0,0,0,0,0,144,36},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0,0,36},{0,E4,0,0,28,0,0,0,0,0,144,72},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,28,0,0,0,0,0,0,72},{0,E4,0,0,28,0,0,0,0,0,144,72},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,G6,0,0,0,0,0,0,0,0,288,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{C,C2,0,0,28,0,0,0,54,54,72,0},{0,G6,0,0,28,0,0,0,0,0,252,144},{0,G6,0,0,28,0,0,0,0,0,72,72},{0,G6,0,0,28,0,0,0,0,0,36,72},{0,G6,0,0,0,0,0,0,0,0,252,144},{0,G6,0,0,28,0,0,0,0,0,252,144},{0,G6,0,0,28,0,0,0,0,0,144,72},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{D,D3,0,0,0,0,0,0,54,36,180,72},{D,0,0,0,0,0,0,0,108,72,0,0},{D,0,0,0,0,0,0,0,36,0,0,0},{D,0,0,0,0,0,0,0,126,72,0,0},{D,0,0,0,0,0,0,0,126,72,0,0},{D,0,0,0,0,0,0,0,144,72,0,0},{D,G6,0,1,28,0,0,0,90,54,0,0},{0,G6,0,0,28,0,0,0,0,0,108,108},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{J,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{0,D3,0,0,28,0,0,0,0,0,108,36},{0,F5,0,0,28,0,0,0,0,0,72,0},{H,F5,28,0,28,0,0,0,90,36,288,144},{0,F5,0,0,28,0,0,0,0,0,252,144},{0,F5,0,0,28,0,0,0,0,0,36,0},{0,F5,0,0,28,0,0,0,0,0,36,0},{D,F5,0,4,28,0,0,0,72,72,72,36},{D,G6,0,1,28,0,0,0,54,54,72,72},{0,G6,0,0,28,0,0,0,0,0,180,108},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{J,0,0,0,0,0,0,0,0,18,0,0},{K,0,0,0,0,0,0,0,0,0,0,0},{K,0,0,0,0,0,0,0,18,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{E,E4,0,0,28,0,0,0,108,0,36,36},{0,C2,0,0,28,0,0,0,0,0,144,72},{E,0,0,0,0,0,0,0,0,18,0,0},{E,0,0,0,0,0,0,0,126,72,0,0},{E,F5,0,1,28,0,0,0,18,54,72,144},{0,F5,0,0,28,0,0,0,0,0,108,36},{0,F5,0,0,28,0,0,0,0,0,144,72},{D,0,0,0,0,0,0,0,36,72,0,0},{D,G6,0,1,28,0,0,0,54,54,72,144},{0,G6,0,0,28,0,0,0,0,0,252,144},{0,G6,0,0,28,0,0,0,0,0,288,144},{J,G6,0,0,28,0,0,0,0,36,288,144},{K,G6,0,0,28,0,0,0,0,18,288,144},{K,G6,0,0,28,0,0,0,18,18,216,144},{C,G6,0,0,28,0,0,0,72,54,216,144}},
{{F,C2,0,0,28,0,0,0,72,0,72,72},{F,C2,0,2,28,0,0,0,0,54,144,36},{F,0,0,3,0,0,0,0,18,72,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{E,0,0,4,0,0,0,0,36,72,0,0},{E,F5,0,0,28,0,0,0,54,54,72,144},{0,F5,0,0,28,0,0,0,0,0,360,0},{0,F5,0,0,28,0,0,0,0,0,108,108},{D,0,0,0,0,0,0,0,36,72,0,0},{D,0,0,0,0,0,0,0,144,72,0,0},{D,0,0,0,0,0,0,0,126,72,0,0},{D,0,0,0,0,0,0,0,108,72,0,0},{D,0,0,0,0,0,0,0,108,72,0,0},{D,0,0,0,0,0,0,0,36,0,0,0},{D,0,0,0,0,0,0,0,54,36,0,0}},
{{F,0,0,0,0,0,0,0,18,18,0,0},{F,D3,0,3,28,0,0,0,90,72,180,72},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{E,0,0,4,0,0,0,0,72,72,0,0},{E,F5,0,1,28,0,0,0,90,54,72,144},{0,F5,0,0,28,0,0,0,0,0,72,72},{0,F5,0,0,28,0,0,0,0,0,216,144},{0,F5,0,0,28,0,0,0,0,0,288,144},{0,F5,0,0,28,0,0,0,0,0,252,144},{0,F5,0,0,28,0,0,0,0,0,252,144},{0,F5,0,0,28,0,0,0,0,0,432,72},{D,0,0,0,0,0,0,0,90,18,0,0},{0,F5,0,0,28,0,0,0,0,0,360,108}},
{{F,0,0,0,0,0,0,0,72,0,0,0},{0,D3,0,0,28,0,0,0,0,0,180,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{E,0,0,4,0,0,0,0,36,72,0,0},{E,0,0,0,0,0,0,0,126,72,0,0},{E,0,28,0,0,0,0,0,144,72,0,0},{E,0,28,0,0,0,0,0,126,72,0,0},{E,0,28,0,0,0,0,0,144,72,0,0},{E,0,28,0,0,0,0,0,144,72,0,0},{E,0,28,0,0,0,0,0,108,72,0,0},{E,0,28,0,0,0,0,0,54,36,0,0},{E,0,28,0,0,0,0,0,216,36,0,0}},
{{F,F5,0,0,0,0,0,0,72,18,288,144},{0,F5,0,0,0,0,0,0,0,0,108,72},{0,F5,0,0,0,0,0,0,0,0,432,36},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,18,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,36,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,36,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,36,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
{{F,0,0,0,0,0,0,0,72,18,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0,0,0}},
};
}
}
