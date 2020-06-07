using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InteritosMod.Items.Placeables;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Items.Armor.Hydrite
{
	[AutoloadEquip(EquipType.Head)]
	public class HydriteVisage : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrite Visage");
            Tooltip.SetDefault("12% increased magic damage" +
                "/n 10% decreased mana usage" +
                "/n Increases maximum mana by 100");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.LightRed;
            item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HydriteChestplate>() && legs.type == ModContent.ItemType<HydriteLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 12;
            player.GetModPlayer<InteritosPlayer>().hydriteVisage = true;
            player.statManaMax += 100;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.magicDamage += 10;
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
