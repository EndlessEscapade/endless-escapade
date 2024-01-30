using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

public abstract class AmbienceTrack : ModType
{
    public SoundStyle Style { get; protected set; }
    public SlotId SoundSlot { get; protected set; }

    protected sealed override void Register() {
        Initialize();

        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    internal abstract void Update();

    protected abstract void Initialize();

    protected abstract bool Active(Player player);
}
