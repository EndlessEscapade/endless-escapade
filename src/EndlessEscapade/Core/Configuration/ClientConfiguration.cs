using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Core.Configuration;

public sealed class ClientConfiguration : ModConfig
{
	public static ClientConfiguration Instance => ModContent.GetInstance<ClientConfiguration>();

	public override ConfigScope Mode { get; } = ConfigScope.ClientSide;

	/// <summary>
	///		Whether ambience tracks are enabled or not.
	/// </summary>
	[Header("Ambience")]
	[DefaultValue(true)]
	public bool EnableAmbienceTracks { get; set; } = true;

	/// <summary>
	///		Whether ambience sounds are enabled or not.
	/// </summary>
	public bool EnableAmbienceSounds { get; set; } = true;

	/// <summary>
	///		Whether the low pass filter is enabled or not.
	/// </summary>
	[Header("Audio")]
	[DefaultValue(true)]
	public bool EnableLowPassFilter { get; set; } = true;
}
