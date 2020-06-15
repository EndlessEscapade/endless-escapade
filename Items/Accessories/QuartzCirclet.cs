using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Accessories
{

    public class QuartzCirclet : ModItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Circlet");
            Tooltip.SetDefault("When Equipped: \n4% damage increase, \n2% crit increase overall, \n3% move speed increase \nPower surges through you..");
        }
        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Pink;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // hideVisual = true; // note a ref param

            player.allDamageMult *= 1.04f;
            player.magicCrit += 2;
            player.meleeCrit += 2;
            player.rangedCrit += 2;
            player.thrownCrit += 2;
            player.moveSpeed *= 1.03f;
            //reminder: make crit increasing method for compatability and stuff
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldCrown);
            recipe.AddIngredient(ItemType<QuartzGem>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumCrown);
            recipe.AddIngredient(ItemType<QuartzGem>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

