using System.Collections.Generic;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceTrackSystem : ModSystem
{
    public readonly List<AmbienceTrack> Tracks = new();

    public override void Load() { }
}
