using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding.Biomes;

public sealed class Shipyard : MicroBiome
{
    public override bool Place(Point origin, StructureMap structures) {
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
}
