using EEMod.UI.States;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using JohnSnail.SubworldLibrary;

namespace EEMod.Items
{
    public class FishermansLog : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fisherman's Log");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 22;
            item.useAnimation = 22;
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 0, 18, 0);
            item.rare = ItemRarityID.Orange;
        }
        public override bool UseItem(Player player)
        {
            if (EEMod.UI.IsActive("EEInterfacee"))
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/FishermansLogClose"));
                (EEMod.UI.GetState("EEInterfacee") as FishermansLogUI).ClosingUI = true;
                (EEMod.UI.GetState("EEInterfacee") as FishermansLogUI).SlideTimer = 0;
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/FishermansLogOpen"));
                EEMod.UI.SetState("EEInterfacee", "FishermansLogUI");
            }
            return true;
        }
    }
}