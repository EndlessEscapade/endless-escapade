using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;
using Microsoft.Xna.Framework;
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

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -2);
        }

        int flameColor = 0;
        public override bool AltFunctionUse(Player player)
        {
            if (flameColor < 3)
            {
                Main.NewText("ae");
                flameColor++;
            }
            if (flameColor == 3)
            {
                Main.NewText("ae");
                flameColor = 0;
            }
            Main.NewText(flameColor);
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 0)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<FlamingPumpkinProjectile>(), item.damage, item.knockBack, ai0: flameColor);
                Main.NewText(flameColor);
            }
            return false;
        }
    }
}