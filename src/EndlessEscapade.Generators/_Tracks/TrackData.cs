using Newtonsoft.Json;

namespace EndlessEscapade.Generators
{
	public sealed class TrackData
	{
		[JsonRequired]
		public string SoundPath;

		[JsonRequired]
		public string[] Signals;

		public float StepIn = 0.05f;

		public float StepOut = 0.05f;
	}
}
