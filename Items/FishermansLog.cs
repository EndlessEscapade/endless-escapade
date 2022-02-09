using EEMod.UI.States;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.Items
{
    public class FishermansLog : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fisherman's Log");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Orange;
        }
        public override bool? UseItem(Player player)
        {
            if (EEMod.UI.HasTile("EEInterfacee"))
            {
                //SoundEngine.PlaySound(Mod.GetLegacySoundSlot(Terraria.Audio.SoundType.Music, "Sounds/Sounds/FishermansLogClose"));
                (EEMod.UI.GetState("EEInterfacee") as FishermansLogUI).ClosingUI = true;
                (EEMod.UI.GetState("EEInterfacee") as FishermansLogUI).SlideTimer = 0;
            }
            else
            {
                //SoundEngine.PlaySound(mod.GetLegacySoundSlot(Terraria.Audio.SoundType.Music, "Sounds/Sounds/FishermansLogOpen"));
                EEMod.UI.SetState("EEInterfacee", "FishermansLogUI");
            }
            return true;
        }
    }
}