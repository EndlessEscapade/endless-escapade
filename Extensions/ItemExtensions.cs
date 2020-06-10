using Terraria;
using Terraria.ModLoader;
using EEMod.Items;

namespace EEMod.Extensions
{
    public static class ItemExtensions
    {
        public static EEGlobalItem EEGlobalItem(this Item item) => item.GetGlobalItem<EEGlobalItem>();
    }
}
