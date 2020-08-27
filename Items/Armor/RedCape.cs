using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials.Fruit;

namespace EEMod.Items.Armor
{
    public class RedCape : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Cape");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EEPlayer>().isWearingCape = true;
            Main.NewText("ae");
        }
    }
}