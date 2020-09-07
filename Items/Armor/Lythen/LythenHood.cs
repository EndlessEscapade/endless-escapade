using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Lythen
{
    [AutoloadEquip(EquipType.Head)]
    public class LythenHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Hood");
            Tooltip.SetDefault("6% increased magic damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 21);
            item.rare = ItemRarityID.Green;
            item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<LythenChestplate>() && legs.type == ModContent.ItemType<LythenBoots>();
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.06f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Kelp erupts from the ground when enemies come near";
            player.GetModPlayer<EEPlayer>().lythenSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LythenBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}