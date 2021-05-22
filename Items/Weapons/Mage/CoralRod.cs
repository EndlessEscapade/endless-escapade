using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Weapons.Mage;

namespace EEMod.Items.Weapons.Mage
{
    public class CoralRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Rod");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 8;
            item.width = 32;
            item.height = 32;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 0;
            item.rare = ItemRarityID.Blue;
            item.autoReuse = true;
            item.crit = 4;
            item.noMelee = true;
            item.magic = true;
            //item.shoot = ModContent.ProjectileType<Snowball>();
            item.shootSpeed = 16f;
            item.mana = 2;
            item.UseSound = SoundID.DD2_MonkStaffSwing;
            item.useStyle = ItemUseStyleID.HoldingOut;
        }
    }
}