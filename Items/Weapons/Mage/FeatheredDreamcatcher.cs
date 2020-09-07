using EEMod.Projectiles.Mage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class FeatheredDreamcatcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Dreamcatcher");
        }

        public override void SetDefaults()
        {
            item.magic = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 110;
            item.useTime = 20;
            item.useAnimation = 20;
            item.width = 20;
            item.height = 20;
            item.shootSpeed = 0;
            item.rare = ItemRarityID.Yellow;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
            item.mana = 7;
            item.shoot = ModContent.ProjectileType<FeatheredDreamcatcherProjectile>();
        }
    }
}