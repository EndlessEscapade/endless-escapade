using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Head)]
    public class ExitiumsHead : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Head");
            Tooltip.SetDefault("':feelssnalechog:'\n'Great for impersonating mod devs!'");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
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

        public override bool DrawHead()
        {
            return false;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = false;
        }
    }
}