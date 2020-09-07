using EEMod.Items.Materials.Fruit;
using EEMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger
{
    public class CoconutCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Cannon");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.ranged = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 35;
            item.useTime = 60;
            item.useAnimation = 60;
            item.width = 20;
            item.height = 20;
            item.shoot = ModContent.ProjectileType<CoconutProjectile>();
            item.rare = ItemRarityID.Orange;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 17f;
            item.UseSound = SoundID.Item11;
            item.ammo = ModContent.ItemType<Coconut>();
            item.notAmmo = true;
        }
    }
}