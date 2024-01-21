using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items.Components;

public abstract class ItemComponent : GlobalItem
{
    public sealed override bool InstancePerEntity { get; } = true;
    
    public bool Enabled { get; set; }
}
