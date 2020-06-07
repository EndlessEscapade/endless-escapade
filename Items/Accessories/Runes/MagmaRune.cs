using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace InteritosMod.Items.Accessories.Runes
{
    public class MagmaRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Rune");
            Tooltip.SetDefault("4% increased damage" + "\nTrue melee attacks deals the" + "\ndebuff OnFire for 2 seconds");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 32);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return rand.Next(new int[] { PrefixID.Arcane, PrefixID.Lucky, PrefixID.Menacing, PrefixID.Quick, PrefixID.Violent, PrefixID.Warding });
        }

        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.04f;
            player.GetModPlayer<InteritosPlayer>().magmaRune = true; ;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Obsidian, 20);
            recipe.AddIngredient(ItemID.HellstoneBar, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}