namespace EndlessEscapade.Core.EC;

public sealed class ComponentSystem : ModSystem
{
	private static class ComponentData<T> where T : struct
	{
		public static readonly int Id = componentTypeCount++;
		public static readonly int Mask = 1 << Id;

		public static T Default = new();

		public static long[] Flags = [];

		public static T[] Components = [];

		static ComponentData() {
			OnUpdate += OnUpdateEvent;
			OnRender += OnRenderEvent;
		}

		private static void OnUpdateEvent() {
			for (var i = 0; i < Components.Length; i++) {
				var component = Components[i];
			}
		}

		private static void OnRenderEvent() {
			for (var i = 0; i < Components.Length; i++) {
				var component = Components[i];
			}
		}
	}

	private static int componentTypeCount;

	private static event Action? OnUpdate;
	private static event Action? OnRender;

	public override void PostUpdateWorld() {
		base.PostUpdateWorld();

		// Despite being different callbacks, render and update are called under the same hook because
		// rendering components are meant to direct their logic to external renderers instead of executing
		// it by themselves.
		OnUpdate?.Invoke();
		OnRender?.Invoke();
	}

	/// <summary>
	///		Retrieves a component of the specified type from an entity.
	/// </summary>
	/// <param name="id">The identify of the entity to retrieve the component from.</param>
	/// <typeparam name="T">The type of the component to retrieve.</typeparam>
	/// <returns>The instance of the component if found; otherwise, <c>null</c>.</returns>
	public static ref T Get<T>(int id) where T : struct {
		return ref ComponentData<T>.Components[id];
	}

	/// <summary>
	///		Sets the value of a component of the specified type to an entity.
	/// </summary>
	/// <param name="id">The identity of the entity to set the component to.</param>
	/// <param name="value">The value of the component.</param>
	/// <typeparam name="T">The type of the component to set.</typeparam>
	/// <returns>The assigned component instance.</returns>
	public static ref T Set<T>(int id, T value) where T : struct {
		if (id >= ComponentData<T>.Components.Length) {
			var newSize = Math.Max(1, ComponentData<T>.Components.Length);

			while (newSize <= id) {
				newSize *= 2;
			}

			Array.Resize(ref ComponentData<T>.Components, newSize);
		}

		if (id >= ComponentData<T>.Flags.Length) {
			var newSize = Math.Max(1, ComponentData<T>.Flags.Length);

			while (newSize <= id) {
				newSize *= 2;
			}

			Array.Resize(ref ComponentData<T>.Flags, newSize);
		}

		ComponentData<T>.Components[id] = value;
		ComponentData<T>.Flags[id] |= ComponentData<T>.Mask;

		return ref ComponentData<T>.Components[id];
	}

	/// <summary>
	///		Checks whether an entity has a component of the specified type or not.
	/// </summary>
	/// <param name="id">The identity of the entity to check.</param>
	/// <typeparam name="T">The type of the component to check.</typeparam>
	/// <returns><c>true</c> if the component was found; otherwise, <c>false</c>.</returns>
	public static bool Has<T>(int id) where T : struct {
		return (ComponentData<T>.Flags[id] & ComponentData<T>.Mask) != 0;
	}

	/// <summary>
	///		Attempts to remove a component of the specified type from an entity.
	/// </summary>
	/// <param name="id">The identity of the entity to remove the component from.</param>
	/// <typeparam name="T">The type of the component to remove.</typeparam>
	/// <returns><c>true</c> if the component was successfully removed; otherwise, <c>false</c>.</returns>
	public static bool Remove<T>(int id) where T : struct {
		ComponentData<T>.Components[id] = default;
		ComponentData<T>.Flags[id] &= ~ComponentData<T>.Mask;

		return true;
	}
}
