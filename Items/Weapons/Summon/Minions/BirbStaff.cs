using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class BirbStaff : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Birb Staff");
            Tooltip.SetDefault("Isn't he so cute.");
        }

        public override void SetDefaults()
        {
            Item.summon = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 36;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<AkumoMinion>();
            Item.buffType = ModContent.BuffType<AkumoBuff>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(ModContent.BuffType<AkumoBuff>(), 2, true);
            position = Main.MouseWorld;
            return true;
        }
    }
}