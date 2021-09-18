using EEMod.Projectiles.Runes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Launchers
{
    public class EggPistol : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Egg Pistol");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 12;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.width = 20;
            Item.height = 20;
            Item.shoot = ModContent.ProjectileType<CycloneStormRune>();
            Item.rare = ItemRarityID.Yellow;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 17f;
            Item.UseSound = SoundID.Item11;
            //item.useAmmo = AmmoID.Rocket;
        }
    }
}