using EndlessEscapade.Common.Systems.Net;
using System.IO;
using Terraria.ModLoader;

namespace EndlessEscapade;

public class EndlessEscapade : Mod
{
    public static EndlessEscapade Instance => ModContent.GetInstance<EndlessEscapade>();

    public override void HandlePacket(BinaryReader reader, int whoAmI) {
        NetSystem.HandleModRecievedPacket(reader, whoAmI);
    }
}
