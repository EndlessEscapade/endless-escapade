using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Melee.Swords;
using EEMod.Subworlds.GoblinFort;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class BruteHalberd : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brute Halberd");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.shootSpeed = 0;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;

            Item.shoot = ModContent.ProjectileType<GoblinBanner>();
        }
    }
}