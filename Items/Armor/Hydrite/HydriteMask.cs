using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.Hydrite
{
    [AutoloadEquip(EquipType.Head)]
    public class HydriteMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrite Mask");
            Tooltip.SetDefault("16% increased melee damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.LightRed;
            item.defense = 21;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HydriteChestplate>() && legs.type == ModContent.ItemType<HydriteLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.16f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.meleeDamage += 0.8f;
            player.meleeSpeed += 0.8f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}