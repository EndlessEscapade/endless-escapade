using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;

namespace EEMod.Items.Weapons.Summon
{
    public class BirbStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Birb Staff");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 13;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item8;
            //item.shoot = ModContent.ProjectileType<ARProj>();
        }
    }
}
