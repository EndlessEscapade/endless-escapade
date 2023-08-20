using EndlessEscapade.Common.Systems.Net;
using System.IO;
using Terraria.ModLoader;

namespace EndlessEscapade;

public class EndlessEscapade : Mod
{
    /// <summary>Shorthand for <c>Modcontent.GetInstance&lt;EndlessEscapade&gt;()</c></summary>
    public static EndlessEscapade Instance => ModContent.GetInstance<EndlessEscapade>();
    public override void HandlePacket(BinaryReader reader, int whoAmI) => EENet.HandleModRecievedPacket(reader, whoAmI);
}
