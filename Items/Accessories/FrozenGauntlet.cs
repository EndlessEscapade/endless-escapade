using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class FrozenGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Gauntlet");
            Tooltip.SetDefault("coldldldododldoddllodlloldoolololdolldo \nCLOUD SPRITE\nGRAYDEE WAEPON\n:heart_eyes:");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fishingSkill += 30;
        }
    }
}