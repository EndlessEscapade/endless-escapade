/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Mage;

namespace EEMod.Items.Weapons.Mage
{
    public class DruidsRelic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Druid's Relic");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Vilethorn);
            item.melee = false;
            item.magic = true;
            item.width = 20;
            item.height = 20;
            item.mana = 11;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = 22;
            item.useAnimation = 22;
            item.value = Item.sellPrice(0, 0, 17);
            item.rare = ItemRarityID.Green;
            item.shootSpeed = 12f;
            item.UseSound = SoundID.Item19;
            item.autoReuse = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RichMahogany, 12);
            recipe.AddIngredient(ItemID.Vine, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}*/