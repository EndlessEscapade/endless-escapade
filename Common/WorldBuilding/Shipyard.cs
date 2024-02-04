using EndlessEscapade.Content.NPCs.Town;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding;

// TODO: Separate Shipyard and Sailboat (s) generation.
public sealed class Shipyard : MicroBiome
{
    public const int SailboatDistance = 100;
    
    public override bool Place(Point origin, StructureMap structures) {
        return GenerateShipyard(origin, structures) && GenerateBrokenBoat(origin - new Point(SailboatDistance, 0), structures);
    }

    private static bool GenerateShipyard(Point origin, StructureMap structures) {
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/Shipyard", mod, ref dims)) {
            return false;
        }

        var offset = new Point16(dims.X / 2, dims.Y - dims.Y / 3);
        var adjustedOrigin = new Point16(origin.X, origin.Y) - offset;

        if (!structures.CanPlace(new Rectangle(adjustedOrigin.X, adjustedOrigin.Y, dims.X, dims.Y)) || !Generator.GenerateStructure("Content/Structures/Shipyard", adjustedOrigin, mod)) {
            return false;
        }

        // Fills up a blotch to make the structure naturally blend within the pre-existing terrain.
        for (var j = adjustedOrigin.Y + 30; j < adjustedOrigin.Y + dims.Y; j++) {
            var strength = WorldGen.genRand.Next(10, 17);
            var steps = WorldGen.genRand.Next(1, 4);

            WorldGen.TileRunner(adjustedOrigin.X + dims.X, j, strength, steps, TileID.Sand, true);
        }

        for (var i = 0; i < 2; i++) {
            // Extends dock pillars.
            WorldGenUtils.ExtendDownwards(adjustedOrigin.X + 4 + i, adjustedOrigin.Y + 39, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(adjustedOrigin.X + 20 + i, adjustedOrigin.Y + 39, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(adjustedOrigin.X + 36 + i, adjustedOrigin.Y + 39, TileID.LivingWood);

            // Extends house pillars
            WorldGenUtils.ExtendDownwards(adjustedOrigin.X + 56 + i, adjustedOrigin.Y + 27, TileID.LivingWood);
            WorldGenUtils.ExtendDownwards(adjustedOrigin.X + 74 + i, adjustedOrigin.Y + 27, TileID.LivingWood);
        }

        var sailorX = (int)((adjustedOrigin.X + 60) * 16f);
        var sailorY = (int)((adjustedOrigin.Y + 10) * 16f);

        var index = NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
        var sailor = Main.npc[index];

        sailor.UpdateHomeTileState(false, (int)(sailorX / 16f), (int)(sailorY / 16f));

        structures.AddProtectedStructure(new Rectangle(adjustedOrigin.X, adjustedOrigin.Y, dims.X, dims.Y));

        return true;
    }
    
    private static bool GenerateBrokenBoat(Point origin, StructureMap structures) {
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/BrokenSailboat", mod, ref dims)) {
            return false;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var adjustedOrigin = new Point16(origin.X, origin.Y) - offset;

        if (!Generator.GenerateStructure("Content/Structures/BrokenSailboat", adjustedOrigin, mod)) {
            return false;
        }
        
        structures.AddProtectedStructure(new Rectangle(adjustedOrigin.X, adjustedOrigin.Y, dims.X, dims.Y));

        return true;
    }
}
