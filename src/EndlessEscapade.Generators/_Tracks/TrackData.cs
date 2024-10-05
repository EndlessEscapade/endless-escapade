using Newtonsoft.Json;
using System;
using System.Linq;

namespace EndlessEscapade.Generators
{
	public sealed class TrackData : IEquatable<TrackData>
	{
		[JsonRequired]
		public string SoundPath;

		[JsonRequired]
		public string[] Signals;

		public float StepIn = 0.05f;

		public float StepOut = 0.05f;

        public bool Equals(TrackData other)
        {
			return other != null 
				&& other.SoundPath == SoundPath 
				&& other.Signals.AsSpan().SequenceEqual(other.Signals)
				&& other.StepIn == StepIn
				&& other.StepOut == StepOut;
        }
		public override bool Equals(object obj)
		{
			return Equals(obj as TrackData);
		}
        public override int GetHashCode()
        {
            return HashCode.Combine(SoundPath, Signals.GetHashCode(), StepIn, StepOut);
        }
    }
}
