using EEMod.ID;
using EEMod.Systems.Subworlds.EESubworlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Systems;

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
            // Item.consumable = false;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.EatFood;
            //Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/ConchBlown");
        }

        public override bool? UseItem(Player player)
        {
            //Main.LocalPlayer.GetModPlayer<EEPlayer>().Initialize();
            //SubworldManager.EnterSubworld<CoralReefs>();

            //Structure.SaveWorldStructureTo(306, 396, 11, 15, System.IO.Path.Combine(Main.SavePath, "RockSpire1"));

            //Structure.SaveWorldStructureTo(325, 396, 11, 15, System.IO.Path.Combine(Main.SavePath, "RockSpire2"));

            //Structure.SaveWorldStructureTo(340, 402, 11, 9, System.IO.Path.Combine(Main.SavePath, "SunkRaft"));

            //Structure.SaveWorldStructureTo(356, 406, 9, 5, System.IO.Path.Combine(Main.SavePath, "SandChest"));

            return true;
        }
    }
}