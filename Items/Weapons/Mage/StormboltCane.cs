using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Mage;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class StormboltCane : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormbolt Cane");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;
            Item.crit = 3;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.shoot = ModContent.ProjectileType<LythenStaffProjectile>();
            Item.shootSpeed = 16f;
            Item.mana = 5;
            Item.UseSound = SoundID.Item8;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile projectile = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_Item(player, Item),
position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            if (Main.netMode != NetmodeID.Server)
            {
                //EEMod.prims.CreateTrail(projectile);
            }

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, -4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LythenBar>(), 12).AddTile(TileID.Anvils).Register();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 3;
        }
    }
}