using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Head)]

    public class Cat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Head");
            Tooltip.SetDefault("'Great for impersonating mod devs!'");
        }
        public override void SetDefaults()
        {
            item.width = 18;
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