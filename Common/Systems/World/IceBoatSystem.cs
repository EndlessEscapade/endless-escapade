using StructureHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Systems.World
{
    public class IceBoatSystem : ModSystem {
        public override void PostWorldGen() {
            const int maxTries = 1000;
            const int minimumScan = 1000;

            var tries = 0;

            while (tries < maxTries) {
                var x = WorldGen.genRand.Next(500, Main.maxTilesX - 500);
                var y = WorldGen.genRand.Next((int)Main.worldSurface + 200, (int)Main.maxTilesY - 200);

                var dims = Point16.Zero;

                if (!Generator.GetDimensions("Assets/Structures/IceSailboat", Mod, ref dims)) {
                    break;
                }

                var tileData = new Dictionary<ushort, int>();

                var shape = new Shapes.Rectangle(dims.X, dims.Y);
                var action = new Actions.TileScanner(TileID.IceBlock, TileID.SnowBlock).Output(tileData);

                int[,] snapshot = new int[dims.X + 1, dims.Y + 1];

                bool spawnedOnDungeon = false;

                for (int i = x; i < x + dims.X + 1; i++) {
                    for(int j = y; j < y + dims.Y + 1; j++) {
                        if (!WorldGen.InWorld(i, j)) continue;

                        if (!Main.tile[i, j].HasTile) {
                            snapshot[i - x, j - y] = -2;
                        }
                        else {
                            snapshot[i - x, j - y] = WorldGen.TileType(i, j);
                        }

                        if (WorldGen.TileType(i, j) == TileID.BlueDungeonBrick || WorldGen.TileType(i, j) == TileID.PinkDungeonBrick || WorldGen.TileType(i, j) == TileID.GreenDungeonBrick) {
                            spawnedOnDungeon = true;
                            break;
                        }
                    }
                }

                if (spawnedOnDungeon) continue;

                WorldUtils.Gen(new Point(x, y), shape, action);

                var tileCount = tileData[TileID.IceBlock] + tileData[TileID.SnowBlock];

                if (tileCount >= minimumScan) {
                    var offsetX = dims.X / 2;

                    var amount = WorldGen.genRand.Next(7, 12);

                    for (int i = 0; i < amount; i++) {
                        var steps = WorldGen.genRand.Next(20, 50);
                        var size = WorldGen.genRand.Next(5, 10);

                        var offsetY = WorldGen.genRand.Next(50);

                        WorldGen.digTunnel(x + offsetX, y - offsetX, 0, -1, steps, size);
                    }

                    Generator.GenerateStructure("Assets/Structures/IceSailboat", new Point16(x, y), Mod);

                    var replace_dirt = new SmoothDirt(snapshot);

                    var oopsie = new Shapes.Rectangle(dims.X + 1, dims.Y + 1);

                    WorldUtils.Gen(new Point(x, y), oopsie, replace_dirt);

                    break;
                }

                tries++;
            }
        }
    }

    public class SmoothDirt : GenAction {
        int[,] snapshot;

        public SmoothDirt(int[,] snapshot) {
            this.snapshot = snapshot;
        }

        public override bool Apply(Point origin, int x, int y, params object[] args) 
        {
            if (WorldGen.InWorld(x, y) && WorldGen.TileType(x, y) == TileID.Dirt) {
                if (snapshot[x - origin.X, y - origin.Y] == -2) {
                    Main.tile[x, y].ClearTile();
                }
                else {
                    Main.tile[x, y].TileType = (ushort)snapshot[x - origin.X, y - origin.Y];
                }
            }

            return UnitApply(origin, x, y, args);
        }
    }
}