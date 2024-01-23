using Terraria.ModLoader;

namespace EndlessEscapade.Common.NPCs.Components;

public abstract class NPCComponent : GlobalNPC
{
    public sealed override bool InstancePerEntity { get; } = true;
    
    public bool Enabled { get; set; }
}
