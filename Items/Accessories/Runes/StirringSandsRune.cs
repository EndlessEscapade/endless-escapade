using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories.Runes
{
    public class StirringSandsRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stirring Sands Rune");
            Tooltip.SetDefault("6% increased attack speed + \nBy pressing [Bind Key] in the air you will summon a sand cyclone");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 32);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return rand.Next(new int[] { PrefixID.Arcane, PrefixID.Lucky, PrefixID.Menacing, PrefixID.Quick, PrefixID.Violent, PrefixID.Warding });
        }

        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.04f;
            player.GetModPlayer<EEPlayer>().duneRune = true;
        }
    }
}