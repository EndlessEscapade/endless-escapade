using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.World;

public class TundraSystem : ModSystem
{
    public override void PostWorldGen() {
        const int maxTries = 1000;
        const int minimumScan = 1000;

        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/IceSailboat", Mod, ref dims)) {
            return;
        }

        var tries = 0;

        while (tries < maxTries) {
            var x = WorldGen.genRand.Next(500, Main.maxTilesX - 500);
            var y = WorldGen.genRand.Next((int)Main.worldSurface + 200, Main.maxTilesY - 200);

            var spawnedOnDungeon = false;
            var snapshot = new int[dims.X + 1, dims.Y + 1];

            for (var i = x; i < x + dims.X + 1; i++) {
                for (var j = y; j < y + dims.Y + 1; j++) {
                    if (!WorldGen.InWorld(i, j)) {
                        continue;
                    }

                    var type = WorldGen.TileType(i, j);

                    if (!WorldGen.SolidTile(i, j)) {
                        snapshot[i - x, j - y] = -1;
                    }
                    else {
                        snapshot[i - x, j - y] = type;
                    }

                    if (type == TileID.BlueDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.GreenDungeonBrick) {
                        spawnedOnDungeon = true;
                        break;
                    }
                }
            }

            if (spawnedOnDungeon) {
                continue;
            }

            var lookup = new Dictionary<ushort, int>();

            WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(dims.X, dims.Y), new Actions.TileScanner(TileID.IceBlock, TileID.SnowBlock).Output(lookup));

            var count = lookup[TileID.IceBlock] + lookup[TileID.SnowBlock];

            if (count >= minimumScan) {
                var offsetX = dims.X / 2;

                var amount = WorldGen.genRand.Next(30, 40);

                for (var i = 0; i < amount; i++) {
                    var offsetY = WorldGen.genRand.Next(y - (int)Main.worldSurface);

                    var steps = WorldGen.genRand.Next(15, 30);
                    var size = WorldGen.genRand.Next(8, 12);

                    WorldGen.digTunnel(x + offsetX, y - offsetX, 0, -1, steps, size);
                }

                Generator.GenerateStructure("Assets/Structures/IceSailboat", new Point16(x, y), Mod);

                WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(dims.X + 1, dims.Y + 1), new PreserveAction(snapshot, TileID.Dirt));

                break;
            }

            tries++;
        }
    }
}
