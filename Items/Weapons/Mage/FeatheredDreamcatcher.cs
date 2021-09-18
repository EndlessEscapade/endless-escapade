using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class FeatheredDreamcatcher : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Dreamcatcher");
        }

        public override void SetDefaults()
        {
            // Item.magic = false;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 110;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.width = 20;
            Item.height = 20;
            Item.shootSpeed = 0;
            Item.rare = ItemRarityID.Yellow;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.mana = 7;
            Item.shoot = ModContent.ProjectileType<FeatheredDreamcatcherProjectile>();
        }
    }
}