using EEMod.Buffs.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Items.Weapons.Summon.Whips
{
    public class Arclash : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arclash");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.knockBack = 4f;
            Item.crit = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 30;
            Item.useTime = 20;
            Item.width = 58;
            Item.height = 44;
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<ArclashProj>();
            Item.shootSpeed = 4.5f;
            Item.value = 1000;
        }
    }
}