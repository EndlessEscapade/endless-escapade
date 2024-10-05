using System.IO;
using System.Text;
using Hjson;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace EndlessEscapade.Generators
{
	[Generator(LanguageNames.CSharp)]
	public sealed class TrackGenerator : IIncrementalGenerator
	{
		/// <summary>
		///     The file extension associated with this generator.
		/// </summary>
		public const string Extension = ".track";

		public void Initialize(IncrementalGeneratorInitializationContext initializationContext) {
			var files = initializationContext.AdditionalTextsProvider.Where(file => file.Path.EndsWith(Extension));

			var contents = files.Select(
				(text, token) => (
					Name: Path.GetFileNameWithoutExtension(text.Path),
					Text: text.GetText(token).ToString()
				)
			);

			initializationContext.RegisterSourceOutput(
				contents,
				(sourceContext, content) => {
					var json = HjsonValue.Parse(content.Text).ToString(Stringify.Plain);
					var data = JsonConvert.DeserializeObject<TrackData>(json);

					var name = content.Name.Split('.')[0];

					sourceContext.AddSource($"{name}.g.cs", GenerateTrack(name, in data));
				}
			);
		}

		private static string GenerateTrack(string name, in TrackData data) {
			var builder = new StringBuilder();

			builder.Append("[");

			for (var i = 0; i < data.Signals.Length; i++) {
				builder.Append('"');
				builder.Append(data.Signals[i]);
				builder.Append('"');

				if (i >= data.Signals.Length - 1) {
					continue;
				}

				builder.Append(',');
				builder.Append(' ');
			}

			builder.Append("]");

			return $@"using Terraria.Audio;
using ReLogic.Utilities;

namespace EndlessEscapade.Common.Ambience;

public sealed class {name} : ITrack
{{
	public SoundStyle Sound {{ get; }} = new(""{data.SoundPath}"", SoundType.Ambient) {{
		Volume = 0.8f,
		IsLooped = true
	}};

	public string[] Signals {{ get; }} = {builder};

	public float StepIn {{ get; }} = {data.StepIn}f;

	public float StepOut {{ get; }} = {data.StepOut}f;

	public float Volume {{
		get => _volume;
		set => _volume = MathHelper.Clamp(value, 0f, 1f);
	}}

	private float _volume;

	public SlotId Slot {{ get; set; }}
}}";
		}
	}
}