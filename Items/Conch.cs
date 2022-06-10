using EEMod.ID;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Systems;
using EEMod.Subworlds;

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

            SubworldLibrary.SubworldSystem.Enter<GoblinFort>();

            //Structure.SaveWorldStructureTo(1980, 165, 160, 68, System.IO.Path.Combine(Main.SavePath, "goblinhall"));

            //Structure.SaveWorldStructureTo(690, 273, 8, 4, System.IO.Path.Combine(Main.SavePath, "GoblinRaft2"));

            return true;
        }
    }
}