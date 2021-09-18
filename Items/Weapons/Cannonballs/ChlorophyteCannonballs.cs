using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Cannonballs
{
    public class ChlorophyteCannonballs : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Cannonballs");
            Tooltip.SetDefault("Use this to make your boat shoot homing chlorophyte cannonballs.");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item1;
            // Item.consumable = false;
            Item.useTime = 15;
            Item.useAnimation = 15;
        }

        public override void OnConsumeItem(Player player)
        {
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
            modPlayer.cannonballType = 4;
            Main.NewText(modPlayer.boatSpeed);
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}