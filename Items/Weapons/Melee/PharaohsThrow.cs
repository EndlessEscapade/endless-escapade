using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Melee;

namespace EEMod.Items.Weapons.Melee
{
    public class PharaohsThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pharaoh's Throw");
            Tooltip.SetDefault("Inflicts Shadowflame on enemies" + "\nHas a 20% chance to charge on near enemies");

            // These are all related to gamepad controls and don't seem to affect anything else
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 24;
            item.height = 24;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 16f;
            item.knockBack = 2.5f;
            item.damage = 52;
            item.rare = ItemRarityID.LightRed;

            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(silver: 1);
            item.shoot = ModContent.ProjectileType<PharaohsThrowProjectile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HardenedSand, 20);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}