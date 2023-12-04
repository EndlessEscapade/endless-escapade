using System;
using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class ShipyardSystem : ModSystem
{
    public override void PostWorldGen() {
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

        var biggestX = startX;
        var biggestY = startY;
        
        for (int i = startX; i < startX + 50; i++) {
            for (int j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.Sand && tile.LiquidAmount <= 0) {
                    if (j < biggestY) {
                        biggestX = i;
                        biggestY = j;
                    }
                }
            }
        }
        
        GenerateShipyard(biggestX, biggestY);
    }

    private void GenerateShipyard(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/Shipyard", Mod, ref dims)) {
            return;
        }

        var offset = dims;
        var origin = new Point16(x, y) - offset;
 
        if (!Generator.GenerateStructure("Content/Structures/Shipyard", origin, Mod)) {
            return;
        }

        // Extends dock pillars.
        for (var i = 0; i < 2; i++) {
            WorldGenUtils.ExtendDownwards(origin.X + 4 + i, origin.Y + 39, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(origin.X + 20 + i, origin.Y + 39, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(origin.X + 36 + i, origin.Y + 39, TileID.LivingWood);
        }
        
        var sailorX = (int)((origin.X + 60) * 16f);
        var sailorY = (int)((origin.Y + 10) * 16f);

        var index = NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
        var sailor = Main.npc[index];

        sailor.UpdateHomeTileState(false, (int)(sailorX / 16f), (int)(sailorY / 16f));
    }
}
