namespace EndlessEscapade.Core.EC;

public interface IEntity
{
	ref T Get<T>() where T : struct;

	ref T Set<T>(T value) where T : struct;

	bool Has<T>() where T : struct;

	bool Remove<T>() where T : struct;
}
