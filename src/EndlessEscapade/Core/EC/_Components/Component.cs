namespace EndlessEscapade.Core.EC;

public abstract class Component
{
	public EEEntity Entity { get; set; }

	public virtual void Update() { }

	public virtual void Render() { }
}
