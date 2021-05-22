using EEMod.Buffs.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class HyoiPear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyoi Pear");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 13;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<BabyHydros>();
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.LocalPlayer.HasBuff(ModContent.BuffType<BabyHydrosBuff>());
        }
    }
}