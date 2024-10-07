using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EndlessEscapade.Core.EC;

public sealed class EntitySystem : ModSystem
{
	private static readonly List<int> ActiveEntityIds = [];
	private static readonly List<int> InactiveEntityIds = [];

	private static readonly ConcurrentBag<int> FreeEntityIds = [];

	private static int nextEntityId;

	/// <summary>
	///		Creates a new instance of an entity.
	/// </summary>
	/// <param name="activate">Whether to activate the entity instance or not.</param>
	/// <returns>The created entity instance.</returns>
	public static Entity Create(bool activate) {
		int id;

		if (!FreeEntityIds.TryTake(out id)) {
			id = nextEntityId++;
		}

		if (activate) {
			ActiveEntityIds.Add(id);
		}

		return new Entity(id);
	}

	/// <summary>
	///		Removes an instance of an entity.
	/// </summary>
	/// <param name="id">The identity of the entity to remove.</param>
	/// <returns><c>true</c> if the entity was successfully removed; otherwise, <c>false</c>.</returns>
	public static bool Remove(int id) {
		if (id < 0) {
			return false;
		}

		ActiveEntityIds.Remove(id);
		InactiveEntityIds.Remove(id);

		FreeEntityIds.Add(id);

		return true;
	}

	/// <summary>
	///		Checks whether an entity is active or not.
	/// </summary>
	/// <param name="id">The identity of the entity to check.</param>
	/// <returns><c>true</c> if the entity is active; otherwise, <c>false</c>.</returns>
	internal static bool GetActive(int id) {
		if (id < 0) {
			return false;
		}

		return ActiveEntityIds.Contains(id);
	}

	/// <summary>
	///		Sets the active status of an entity.
	/// </summary>
	/// <param name="id">The identity of the entity to set.</param>
	/// <param name="value">The value of the status to set.</param>
	internal static void SetActive(int id, bool value) {
		if (id < 0) {
			return;
		}

		if (value) {
			ActiveEntityIds.Add(id);
			InactiveEntityIds.Remove(id);
		}
		else {
			ActiveEntityIds.Remove(id);
			InactiveEntityIds.Add(id);
		}
	}
}
