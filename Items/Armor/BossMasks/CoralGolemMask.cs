using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class CoralGolemMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Golem Mask");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = ItemRarityID.Blue;
        }
    }
}