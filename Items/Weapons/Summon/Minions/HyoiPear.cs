using EEMod.Buffs.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class HyoiPear : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyoi Pear");
        }

        public override void SetDefaults()
        {
            // Item.melee = false;
            Item.DamageType = DamageClass.Summon;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 13;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item8;
            //Item.shoot = ModContent.ProjectileType<BabyHydros>();
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.LocalPlayer.HasBuff(ModContent.BuffType<BabyHydrosBuff>());
        }
    }
}