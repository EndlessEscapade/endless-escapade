using EEMod.ID;
using EEMod.Systems.Subworlds.EESubworlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class Conch : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dev Conch");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 34;
            Item.maxStack = 1;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.consumable = false;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.EatingUsing;
            Item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/ConchBlown");
        }

        public override bool UseItem(Player player)
        {
            Main.LocalPlayer.GetModPlayer<EEPlayer>().Initialize();
            SubworldManager.EnterSubworld<CoralReefs>(); 
            return true;
        }
    }
}