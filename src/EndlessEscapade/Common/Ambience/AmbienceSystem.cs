using EndlessEscapade.Core.Configuration;
using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceSystem : ModSystem
{
    public override void PostUpdateWorld() {
        base.PostUpdateWorld();

        UpdateSounds();
        UpdateTracks();
    }

    private static void UpdateSounds() {
        if (!ClientConfiguration.Instance.EnableAmbienceSounds) {
            return;
        }

        foreach (var sound in ModContent.GetContent<IAmbienceSound>()) {
            var active = SignalsSystem.GetSignal(sound.Signals);

            if (!active || !Main.rand.NextBool(sound.Chance)) {
                continue;
            }

            SoundEngine.PlaySound(sound.Sound);
        }
    }

    private static void UpdateTracks() {
        if (!ClientConfiguration.Instance.EnableAmbienceTracks) {
            return;
        }

        foreach (var track in ModContent.GetContent<IAmbienceTrack>()) {
            var active = SignalsSystem.GetSignal(track.Signals);

            if (active) {
                track.Volume += track.StepIn;
            }
            else {
                track.Volume -= track.StepOut;
            }

            var instancePlaying = SoundEngine.TryGetActiveSound(track.Slot, out var instance);
            var soundPlaying = instance?.IsPlaying == true;

            var trackPlaying = instancePlaying && soundPlaying;

            if (active) {
                if (trackPlaying) {
                    instance.Volume = track.Volume;
                }
                else {
                    track.Slot = SoundEngine.PlaySound(track.Sound);
                    track.Volume = 0f;

                    SoundEngine.TryGetActiveSound(track.Slot, out instance);

                    instance.Volume = 0f;
                }
            }
            else if (trackPlaying) {
                if (track.Volume > 0f) {
                    instance.Volume = track.Volume;
                }
                else {
                    instance.Stop();
                    track.Slot = SlotId.Invalid;
                }
            }
        }
    }
}
