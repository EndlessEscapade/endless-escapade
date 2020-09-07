using EEMod.Items;
using Terraria;

namespace EEMod.Extensions
{
    public static class ItemExtensions
    {
        public static EEGlobalItem EEGlobalItem(this Item item) => item.GetGlobalItem<EEGlobalItem>();
    }
}