using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Mage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class AncientBubbleStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble Staff");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 13;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 20;
            item.height = 20;
            item.mana = 12;
            item.shootSpeed = 8f;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<AncientBubbleSmall>();
        }
    }
}