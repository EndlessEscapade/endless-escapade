using EEMod.Buffs.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Items.Weapons.Summon.Whips
{
    public class KelpWhip : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Whip");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.knockBack = 4f;
            Item.crit = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 40;
            Item.useTime = 30;
            Item.width = 58;
            Item.height = 44;
            Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/WhipUse");
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
            // Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<KelpWhipProj>();
            Item.shootSpeed = 4.5f;
            Item.value = 1000;
        }
    }
}