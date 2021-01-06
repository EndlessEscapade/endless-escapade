using EEMod.Projectiles.Mage;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class InkFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Base Drink");
        }

        public override void SetDefaults()
        {
            item.noMelee = true;
            item.magic = true;
            item.width = 20;
            item.height = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 30;
            item.useAnimation = 30;
            item.value = Item.sellPrice(0, 20, 0);
            item.rare = ItemRarityID.Cyan;
            item.shootSpeed = 15f;
            item.damage = 160;
            item.knockBack = 1;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<InkFlaskProjectile>();
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                item.UseSound = SoundID.Item1;
                item.useStyle = ItemUseStyleID.SwingThrow;
                return true;
            }
            else
            {
                item.UseSound = SoundID.Item3;
                item.useStyle = ItemUseStyleID.EatingUsing;
                player.AddBuff(BuffID.Ironskin, 600);
                return false;
            }
        }
    }
}