using EndlessEscapade.Core.Configuration;
using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceTrackSystem : ModSystem
{
    public override void PostUpdateWorld() {
        base.PostUpdateWorld();

        if (!ClientConfiguration.Instance.EnableTracks) {
            return;
        }

        UpdateTracks();
    }

    private static void UpdateTracks() {
        foreach (var track in ModContent.GetContent<IAmbienceTrack>()) {
            var isActive = SignalsSystem.GetSignal(track.Signals);

            if (isActive) {
                track.Volume += track.StepIn;
            }
            else {
                track.Volume -= track.StepOut;
            }

            var isInstancePlaying = SoundEngine.TryGetActiveSound(track.Slot, out var instance);
            var isSoundPlaying = instance?.IsPlaying == true;
            var isTrackPlaying = isInstancePlaying && isSoundPlaying;

            if (isActive) {
                if (isTrackPlaying) {
                    instance.Volume = track.Volume;
                }
                else {
                    track.Slot = SoundEngine.PlaySound(track.Sound);
                    track.Volume = 0f;

                    SoundEngine.TryGetActiveSound(track.Slot, out instance);

                    instance.Volume = 0f;
                }
            }
            else if (isTrackPlaying) {
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
