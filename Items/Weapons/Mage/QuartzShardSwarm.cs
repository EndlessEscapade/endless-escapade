using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Items.Materials;

namespace EEMod.Items.Weapons.Mage
{
    public class QuartzShardSwarm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartzshard Swarm");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.magic = true;
            item.mana = 4;
            item.width = 46;
            item.height = 46;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.UseSound = SoundID.Item73;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<QuartzProj>();
            item.shootSpeed = 6f;
            item.rare = ItemRarityID.Pink;
            item.crit = 5;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -5);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX *= 2f;
            position += new Vector2(speedX, speedY * 2) * 5;
            speedY += Main.rand.NextFloat(0.9f, -0.9f);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 4);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 4);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
