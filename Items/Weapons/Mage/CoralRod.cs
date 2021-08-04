using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Weapons.Mage;

namespace EEMod.Items.Weapons.Mage
{
    public class CoralRod : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Rod");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.crit = 4;
            Item.noMelee = true;
            Item.magic = true;
            //item.shoot = ModContent.ProjectileType<Snowball>();
            Item.shootSpeed = 16f;
            Item.mana = 2;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.useStyle = ItemUseStyleID.HoldingOut;
        }
    }
}