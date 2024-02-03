using System;
using Terraria;

namespace EndlessEscapade.Common.Items.Components;

public static class ItemComponentExtensions
{
    public static bool TryEnableComponent<T>(this Item item, Action<T> initializer = null) where T : ItemComponent {
        if (!item.TryGetGlobalItem(out T component)) {
            return false;
        }

        component.Enabled = true;

        initializer?.Invoke(component);

        return true;
    }
}
