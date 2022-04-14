using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Dalantinium
{
    [AutoloadEquip(EquipType.Legs)]
    public class DalantiniumGreaves : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Greaves");
            Tooltip.SetDefault("7% increased movement speed");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.07f;
            player.maxRunSpeed += 0.07f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumBar>(), 13).AddTile(TileID.Anvils).Register();
        }
    }
}