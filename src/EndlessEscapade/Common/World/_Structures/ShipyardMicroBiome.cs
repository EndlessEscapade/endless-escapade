using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using StructureHelper;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.World;

public sealed class ShipyardMicroBiome : MicroBiome
{
    public const int SailboatDistance = 100;

    public override bool Place(Point origin, StructureMap structures) {
        return GenerateShipyard(origin, structures);
    }

    private static bool GenerateShipyard(Point origin, StructureMap structures) {
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/Shipyard", mod, ref dims)) {
            return false;
        }

        var offset = new Point16(dims.X / 2, dims.Y - dims.Y / 3);
        var adjustedOrigin = new Point16(origin.X, origin.Y) - offset;

        if (!structures.CanPlace(new Rectangle(adjustedOrigin.X, adjustedOrigin.Y, dims.X, dims.Y))
            || !Generator.GenerateStructure("Assets/Structures/Shipyard", adjustedOrigin, mod)) {
            return false;
        }

        for (var j = adjustedOrigin.Y + 30; j < adjustedOrigin.Y + dims.Y; j++) {
            var strength = WorldGen.genRand.Next(10, 17);
            var steps = WorldGen.genRand.Next(1, 4);

            WorldGen.TileRunner(adjustedOrigin.X + dims.X, j, strength, steps, TileID.Sand, true);
        }

        for (var i = 0; i < 2; i++) {
            GenerationUtils.ExtendDownwards(adjustedOrigin.X + 4 + i, adjustedOrigin.Y + 38);
            GenerationUtils.ExtendDownwards(adjustedOrigin.X + 20 + i, adjustedOrigin.Y + 38);
            GenerationUtils.ExtendDownwards(adjustedOrigin.X + 36 + i, adjustedOrigin.Y + 38);

            GenerationUtils.ExtendDownwards(adjustedOrigin.X + 56 + i, adjustedOrigin.Y + 26);
            GenerationUtils.ExtendDownwards(adjustedOrigin.X + 74 + i, adjustedOrigin.Y + 26);
        }

        var sailorX = (int)((adjustedOrigin.X + 60) * 16f);
        var sailorY = (int)((adjustedOrigin.Y + 10) * 16f);

        var index = NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<SailorNPC>());
        var sailor = Main.npc[index];

        sailor.UpdateHomeTileState(false, (int)(sailorX / 16f), (int)(sailorY / 16f));

        structures.AddProtectedStructure(new Rectangle(adjustedOrigin.X, adjustedOrigin.Y, dims.X, dims.Y));

        return true;
    }
}
