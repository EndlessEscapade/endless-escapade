using System.Collections.Generic;
using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class ShipyardSystem : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
        var index = tasks.FindIndex(pass => pass.Name == "Final Cleanup");

        tasks.Insert(index + 1, new PassLegacy($"{nameof(EndlessEscapade)}:Shipyard", GenerateShipyard));
    }

    public override void PostWorldGen() {
        base.PostWorldGen();
    }

    private void GenerateShipyard(GenerationProgress progress, GameConfiguration configuration) {
        progress.Message = "Constructing the Shipyard...";
        
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

                if (tile.HasTile && tile.TileType == TileID.Sand && tile.LiquidAmount <= 0) {
                    if (j < biggestY) {
                        biggestY = j;
                    }
                }
            }
        }

        PlaceShipyard(startX, biggestY);
    }

    private void PlaceShipyard(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/Shipyard", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y - dims.Y / 3);
        var origin = new Point16(x, y) - offset;

        if (!Generator.GenerateStructure("Content/Structures/Shipyard", origin, Mod)) {
            return;
        }

        for (var i = 0; i < 2; i++) {
            // Extends dock pillars.
            WorldGenUtils.ExtendDownwards(origin.X + 4 + i, origin.Y + 39, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(origin.X + 20 + i, origin.Y + 39, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(origin.X + 36 + i, origin.Y + 39, TileID.LivingWood);

            // Extends house pillars
            WorldGenUtils.ExtendDownwards(origin.X + 56 + i, origin.Y + 27, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(origin.X + 74 + i, origin.Y + 27, TileID.LivingWood);
        }

        var sailorX = (int)((origin.X + 60) * 16f);
        var sailorY = (int)((origin.Y + 10) * 16f);

        var index = NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
        var sailor = Main.npc[index];

        sailor.UpdateHomeTileState(false, (int)(sailorX / 16f), (int)(sailorY / 16f));
    }
}
