using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Back)]
    public class CaTail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Tail");
            Tooltip.SetDefault("'Great for impersonating mod devs!'");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 12;
            item.rare = ItemRarityID.Cyan;
            item.vanity = true;
            item.accessory = true;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            player.armorEffectDrawShadowBasilisk = true;
            player.armorEffectDrawShadowEOCShield = true;
        }
    }
}