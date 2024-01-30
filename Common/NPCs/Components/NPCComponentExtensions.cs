using System;
using Terraria;

namespace EndlessEscapade.Common.NPCs.Components;

public static class NPCComponentExtensions
{
    public static bool TryEnableComponent<T>(this NPC npc, Action<T>? initializer = null) where T : NPCComponent {
        if (!npc.TryGetGlobalNPC(out T component)) {
            return false;
        }

        component.Enabled = true;

        initializer?.Invoke(component);

        return true;
    }
}
