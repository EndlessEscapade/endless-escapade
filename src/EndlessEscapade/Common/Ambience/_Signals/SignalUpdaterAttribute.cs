namespace EndlessEscapade.Common.Ambience;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SignalUpdaterAttribute(string? name = null) : Attribute
{
	/// <summary>
	///     The name of this attribute's signal.
	/// </summary>
	public readonly string? Name = name;
}
