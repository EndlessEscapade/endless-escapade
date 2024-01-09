namespace EndlessEscapade.Common.EC;

// TODO: Hook callbacks.
public abstract class Component
{
    public virtual void Update() { }

    public virtual void Draw() { }

    public virtual void Spawn() { }

    public virtual void Death() { }
}
