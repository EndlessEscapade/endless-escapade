using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Legs)]
    public class ExitiumsPants : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Pants");
            Tooltip.SetDefault("'The dev server is indeed an asylum'\n'Great for impersonating mod devs!'");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            player.armorEffectDrawShadowBasilisk = true;
            player.armorEffectDrawShadowEOCShield = true;
        }

        /*public override bool DrawLegs()
        {
            return false;
        }*/
    }
}