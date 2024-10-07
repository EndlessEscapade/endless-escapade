namespace EndlessEscapade.Core.EC;

public interface IEntity
{
	T Get<T>() where T : Component;

	T Set<T>(T value) where T : Component;

	bool Has<T>() where T : Component;

	bool Remove<T>() where T : Component;
}
