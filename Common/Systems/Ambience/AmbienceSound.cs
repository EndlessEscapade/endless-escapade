using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Systems.Ambience;

public abstract class AmbienceSound : AmbienceTrack
{
    public virtual int PlaybackRate { get; protected set; } = 100;

    internal sealed override void Update() {
        if (!Active(Main.LocalPlayer) || SoundEngine.TryGetActiveSound(SoundSlot, out _)) {
            return;
        }

        SoundSlot = SlotId.Invalid;

        if (Main.rand.NextBool(PlaybackRate)) {
            SoundSlot = SoundEngine.PlaySound(Style);
        }
    }
}
