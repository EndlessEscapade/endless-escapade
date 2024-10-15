namespace EndlessEscapade.Core.EC;

public struct Entity : IEntity, IEquatable<Entity>
{
	/// <summary>
    ///		Whether this entity is active or not.
    /// </summary>
    public bool Active {
    	get => EntitySystem.GetActive(Id);
    	set => EntitySystem.SetActive(Id, value);
    }

	/// <summary>
	///		The unique identifier of this entity.
	/// </summary>
	public readonly int Id;

	internal Entity(int id) {
		Id = id;
	}

	public override bool Equals(object? obj) {
		return Equals((Entity)obj);
	}

	public override string ToString() {
		return $"Id: {Id}";
	}

	public bool Equals(Entity other) {
		return other.Id == Id;
	}

	/// <summary>
	///		Retrieves a component of the specified type from this entity.
	/// </summary>
	/// <typeparam name="T">The type of the component to retrieve.</typeparam>
	/// <returns>The instance of the component if found; otherwise, <c>null</c>.</returns>
	public ref T Get<T>() where T : struct {
		return ref ComponentSystem.Get<T>(Id);
	}

	/// <summary>
	///		Sets the value of a component of the specified type to this entity.
	/// </summary>
	/// <param name="value">The value of the component to set.</param>
	/// <typeparam name="T">The type of the component to set.</typeparam>
	/// <returns>The assigned component instance.</returns>
	public ref T Set<T>(T value) where T : struct {
		return ref ComponentSystem.Set(Id, value);
	}

	/// <summary>
	///		Checks whether this entity has a component of the specified type or not.
	/// </summary>
	/// <typeparam name="T">The type of component to check.</typeparam>
	/// <returns><c>true</c> if the component was found; otherwise, <c>false</c>.</returns>
	public bool Has<T>() where T : struct {
		return ComponentSystem.Has<T>(Id);
	}

	/// <summary>
	///		Attempts to remove a component of the specified type from this entity.
	/// </summary>
	/// <typeparam name="T">The type of the component to remove.</typeparam>
	/// <returns><c>true</c> if the component was successfully removed; otherwise, <c>false</c>.</returns>
	public bool Remove<T>() where T : struct {
		return ComponentSystem.Remove<T>(Id);
	}
}
