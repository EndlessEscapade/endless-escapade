using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.World;

/// <summary>
///     Handles the world generation of the shipyard.
/// </summary>
public sealed class ShipyardSystem : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
        base.ModifyWorldGenTasks(tasks, ref totalWeight);

        var index = tasks.FindIndex(pass => pass.Name == "Final Cleanup");

        if (index == -1) {
            return;
        }

        tasks.Insert(index + 1, new PassLegacy($"{nameof(EndlessEscapade)}:Shipyard", GenerateShipyard));
    }

    private static void GenerateShipyard(GenerationProgress progress, GameConfiguration configuration) {
        progress.Message = EndlessEscapade.Instance.GetLocalizationValue("UI.Generation.Shipyard");

        var foundOcean = false;
        var foundBeach = false;

        var startX = 0;
        var startY = (int)(Main.worldSurface * 0.35f);

        while (!foundOcean) {
            var tile = Framing.GetTileSafely(startX, startY);

            if (tile.LiquidAmount >= 255 && tile.LiquidType == LiquidID.Water) {
                foundOcean = true;
                break;
            }

            startY++;
        }

        while (!foundBeach) {
            if (WorldGen.SolidTile(startX, startY) && WorldGen.TileType(startX, startY) == TileID.Sand) {
                foundBeach = true;
                break;
            }

            startX++;
        }

        if (!foundOcean || !foundBeach) {
            return;
        }

        var biggestY = startY;

        for (var i = startX; i < startX + 50; i++) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.Sand && tile.LiquidAmount <= 0 && j < biggestY) {
                    biggestY = j;
                    break;
                }
            }
        }

        var shipyard = GenVars.configuration.CreateBiome<ShipyardMicroBiome>();

        shipyard.Place(new Point(startX, biggestY), GenVars.structures);
    }
}
