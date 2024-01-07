namespace EndlessEscapade.Common.EC;

public abstract class Component
{
    public Entity Parent { get; internal set; }
    
    public virtual void Update() { }
    
    public virtual void Draw() { }
    
    public virtual void Spawn() { }
    
    public virtual void Death() { }
}
