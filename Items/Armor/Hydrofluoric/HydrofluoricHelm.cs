using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.Hydrofluoric
{
    [AutoloadEquip(EquipType.Head)]
    public class HydrofluoricHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Helm");
            Tooltip.SetDefault("18% increased melee damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Lime;
            item.defense = 22;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HydrofluoricPlatemail>() && legs.type == ModContent.ItemType<HydrofluoricBoots>();
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.18f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.meleeDamage += 0.10f;
            player.meleeSpeed += 0.06f;
            player.GetModPlayer<EEPlayer>().hydrofluoricSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydrofluoricBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}