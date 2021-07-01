using EEMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class Stormspear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormspear");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 24f;
            item.knockBack = 5f;
            item.width = 16;
            item.height = 16;
            item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
            item.shoot = ModContent.ProjectileType<StormspearProj>();
            item.rare = ItemRarityID.Purple;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.melee = true;
            item.damage = 140;
        }
    }
}