using Newtonsoft.Json;

namespace EndlessEscapade.Common.EC;

[JsonConverter(typeof(Entity))]
public readonly struct Entity
{
    public readonly int Id;

    public bool Active {
        get => EntitySystem.GetActive(Id);
        set => EntitySystem.SetActive(Id, value);
    }

    public Entity(int id) {
        Id = id;
    }

    public bool Has<T>() where T : Component {
        return ComponentSystem.Has<T>(Id);
    }

    public T Get<T>() where T : Component {
        return ComponentSystem.Get<T>(Id);
    }

    public T Set<T>(T component) where T : Component {
        return ComponentSystem.Set(Id, component);
    }

    public void Remove<T>() where T : Component {
        ComponentSystem.Remove<T>(Id);
    }
}
