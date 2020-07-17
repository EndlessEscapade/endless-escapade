using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles;

namespace EEMod.Items.Weapons.Mage
{
    public class InkFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Flask");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.magic = true;
            item.width = 20;
            item.height = 20;
            item.mana = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 30;
            item.useAnimation = 30;
            item.value = Item.sellPrice(0, 20, 0);
            item.rare = ItemRarityID.Purple;
            item.shoot = ModContent.ProjectileType<InkFlaskProjectile>();
            item.shootSpeed = 15f;
            item.damage = 160;
            item.knockBack = 1;
            item.autoReuse = true;
        }
    }
}