using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience;

public abstract class AmbienceTrack : ModType
{
    private bool styleDefined;
    private SoundStyle style;
    
    public SoundStyle Style {
        get => style;
        protected set {
            style = value;
            styleDefined = true;
        }
    }

    public SlotId SoundSlot { get; protected set; }

    protected sealed override void Register() {
        Initialize();

        if (!styleDefined) {
            return;
        }
        
        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    internal abstract void Update();
    
    protected abstract void Initialize();
    
    protected abstract bool Active(Player player);
}
