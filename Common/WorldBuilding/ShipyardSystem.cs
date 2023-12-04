using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework.Input;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class ShipyardSystem : ModSystem
{
    public override void PostWorldGen() {
        var foundOcean = false;
        var foundBeach = false;

        var x = 0;
        var y = (int)(Main.worldSurface * 0.35f);

        while (!foundOcean) {
            var tile = Framing.GetTileSafely(x, y);

            if (tile.LiquidAmount >= 255 && tile.LiquidType == LiquidID.Water) {
                foundOcean = true;
                break;
            }

            y++;
        }

        while (!foundBeach) {
            if (WorldGen.SolidTile(x, y) && WorldGen.TileType(x, y) == TileID.Sand) {
                foundBeach = true;
                break;
            }

            x++;
        }

        GenerateShipyard(x, y);
    }

    private void GenerateShipyard(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/Shipyard", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(53, dims.Y - 10);
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
