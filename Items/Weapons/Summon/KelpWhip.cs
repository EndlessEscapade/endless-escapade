using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Items.Weapons.Summon
{
    public class KelpWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Whip");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.knockBack = 4f;
            item.crit = 9;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 40;
            item.useTime = 30;
            item.width = 30;
            item.height = 40;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/WhipUse");
            item.noMelee = true;
            item.summon = true;
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<KelpWhipProj>();
            item.shootSpeed = 4.5f;
            item.value = 1000;
        }
    }
}