using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class AkumoMask : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akumo Mask");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
        }
    }
}