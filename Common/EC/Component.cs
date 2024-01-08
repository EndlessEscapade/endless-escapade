namespace EndlessEscapade.Common.EC;

// TODO: Hook callbacks.
public abstract class Component
{
    public Entity Parent { get; }

    public virtual void Update() { }

    public virtual void Draw() { }

    public virtual void Spawn() { }

    public virtual void Death() { }
}
