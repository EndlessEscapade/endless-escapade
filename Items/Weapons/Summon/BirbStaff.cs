using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;

namespace EEMod.Items.Weapons.Summon
{
    public class BirbStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Birb Staff");
            Tooltip.SetDefault("Isn't he so cute.");
        }

        public override void SetDefaults()
        {
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 36;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<AkumoMinion>();
            item.buffType = ModContent.BuffType<AkumoBuff>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(ModContent.BuffType<AkumoBuff>(), 2, true);
            position = Main.MouseWorld;
            return true;
        }
    }
}