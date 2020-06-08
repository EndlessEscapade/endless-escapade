using Terraria;
using Terraria.ModLoader;
using EEMod.Items;

namespace EEMod.Extensions
{
    public static class ItemExtensions
    {
        public static EEGlobalItem EEMod(this Item item) => item.GetGlobalItem<EEGlobalItem>();
    }
}
