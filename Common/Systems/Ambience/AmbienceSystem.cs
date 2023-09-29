using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience;

[Autoload(Side = ModSide.Client)]
public class AmbienceSystem : ModSystem
{
    // TODO: Note to self - Naka - Figure out a way for me to be satisfied with this code.
    public override void PostUpdateEverything() {
        foreach (var track in ModContent.GetContent<AmbienceTrack>()) {
            track.Update();
        }
    }
}
