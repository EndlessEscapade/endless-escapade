﻿using EEMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger
{
    public class Hydroshot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hydroshot");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 70;
            item.useTime = 24;
            item.shootSpeed = 4;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<HydroshotProj>();
        }
    }
}