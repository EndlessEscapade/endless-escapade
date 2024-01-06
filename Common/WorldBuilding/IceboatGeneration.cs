using System;
using System.Collections.Generic;
using EndlessEscapade.Common.WorldBuilding.Biomes;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class IceboatGeneration : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
        var index = tasks.FindIndex(pass => pass.Name == "Ice");

        tasks.Insert(index + 1, new PassLegacy($"{nameof(EndlessEscapade)}:Iceboat", GenerateIceboat));
    }

    private void GenerateIceboat(GenerationProgress progress, GameConfiguration configuration) {
        progress.Message = "Sinking the iceboat...";

        var tundraStart = 0;
        var tundraEnd = 0;

        var tundraBottom = 0;

        for (var i = Main.maxTilesX; i > 0; i--) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                    tundraStart = i;
                    break;
                }
            }
        }

        for (var i = 0; i < Main.maxTilesX; i++) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                    tundraEnd = i;
                    break;
                }
            }
        }

        var tundraWidth = Math.Abs(tundraStart - tundraEnd);
        var tundraCenter = tundraStart + tundraWidth / 2;

        for (var i = Main.maxTilesY; i > 0; i--) {
            var tile = Framing.GetTileSafely(tundraCenter, i);

            if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                tundraBottom = i;
                break;
            }
        }

        var biome = GenVars.configuration.CreateBiome<Iceboat>();
        var biomeGenerated = false;

        while (!biomeGenerated) {
            var x = WorldGen.genRand.Next(tundraStart + 100, tundraEnd - 100);

            WorldUtils.Find(new Point(x, 0), Searches.Chain(new Searches.Down(Main.maxTilesY), new Conditions.IsSolid(), new Conditions.IsTile(TileID.IceBlock, TileID.SnowBlock)), out var origin);

            if (biome.Place(origin, GenVars.structures)) {
                biomeGenerated = true;
                break;
            }
        }
    }
}
