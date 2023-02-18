using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.WorldGen;

public sealed class StructureSystem : ModSystem
{
    public override void PostWorldGen() {
        Generator.GenerateStructure("Assets/Structures/Sailboat", new Point16(Main.spawnTileX, Main.spawnTileY - 50), Mod);
    }
}