using System.IO;
using System.Text;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience.Tracks;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceTrackSystem : ModSystem
{
    public override void Load() { }
}
