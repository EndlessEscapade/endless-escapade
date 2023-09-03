using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Ambience;

public abstract class AmbienceTrack : ModType
{
    

    protected sealed override void Register() {
        ModTypeLookup<AmbienceTrack>.Register(this);
    }
}
