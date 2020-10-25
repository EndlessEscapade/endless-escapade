using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon
{
    public class FlamingPumpkinStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Pumpkin Staff");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.value = Item.sellPrice(0, 0, 5, 0);
            item.damage = 13;
            item.useTime = 60;
            item.useAnimation = 60;
            item.width = 38;
            item.height = 40;
            item.rare = ItemRarityID.Blue;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<FlamingPumpkinProjectile>();
        }
    }
}