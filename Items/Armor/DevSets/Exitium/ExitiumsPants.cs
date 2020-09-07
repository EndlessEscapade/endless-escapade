using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Legs)]
    public class ExitiumsPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Fabulous Pants");
            Tooltip.SetDefault("'Even the finest sword plunged into salt water will eventually rust.'");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.rare = ItemRarityID.Cyan;
            item.vanity = true;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            player.armorEffectDrawShadowBasilisk = true;
            player.armorEffectDrawShadowEOCShield = true;
        }

        public override bool DrawLegs()
        {
            return false;
        }
    }
}