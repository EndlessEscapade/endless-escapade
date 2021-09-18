using EEMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class Stormspear : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormspear");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 24f;
            Item.knockBack = 5f;
            Item.width = 16;
            Item.height = 16;
            Item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
            Item.shoot = ModContent.ProjectileType<StormspearProj>();
            Item.rare = ItemRarityID.Purple;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 140;
        }
    }
}