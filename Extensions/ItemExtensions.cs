using Terraria;
using Terraria.ModLoader;
using InteritosMod.Items;

namespace InteritosMod.Extensions
{
    public static class ItemExtensions
    {
        public static InteritosGlobalItem Interitos(this Item item) => item.GetGlobalItem<InteritosGlobalItem>();
    }
}
