using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InteritosMod.Projectiles.Mage;
using Microsoft.Xna.Framework;

namespace InteritosMod.Items.Weapons.Mage
{
    public class Stalagtite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stalagtite");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.autoReuse = true;
            item.noMelee = true;
            item.magic = true;

            item.mana = 12;
            item.useTime = 12;
            item.useAnimation = 12;
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(0, 0, 80);
            item.width = 40;
            item.height = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.shootSpeed = 9f;
            item.knockBack = 3.5f;

            item.UseSound = SoundID.Item88;
            //Missing projectile
        }
    }
}