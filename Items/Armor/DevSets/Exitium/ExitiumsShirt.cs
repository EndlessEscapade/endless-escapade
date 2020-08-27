using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Body)]

    public class ExitiumsShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Fabulous Shirt");
            Tooltip.SetDefault("'To know your Enemy, you must become your Enemy.'");
        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 22;
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
        public override bool DrawBody()
        {
            return false;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = false;
        }
    }
}