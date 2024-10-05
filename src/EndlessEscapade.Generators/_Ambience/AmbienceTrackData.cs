using System;
using Newtonsoft.Json;

namespace EndlessEscapade.Generators;

public sealed class AmbienceTrackData : IEquatable<AmbienceTrackData>
{
    [JsonRequired]
    public string[] Signals;

    [JsonRequired]
    public string SoundPath;

    public float StepIn = 0.05f;

    public float StepOut = 0.05f;

    public bool Equals(AmbienceTrackData other) {
        return other != null
            && other.SoundPath == SoundPath
            && other.Signals.AsSpan().SequenceEqual(other.Signals)
            && other.StepIn == StepIn
            && other.StepOut == StepOut;
    }

    public override bool Equals(object obj) {
        return Equals(obj as AmbienceTrackData);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Signals.GetHashCode(), SoundPath, StepIn, StepOut);
    }
}
