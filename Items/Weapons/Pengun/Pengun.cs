using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Pengun
{
    public class Pengun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pengun");
            Tooltip.SetDefault("You're a monster.");
        }

        public override void SetDefaults()
        {
            item.damage = 48;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 2;
            item.useAnimation = 2;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 16;
            item.value = 1;
            item.rare = 11;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 16f;
            item.shoot = mod.ProjectileType("PengunProjectile");
        }
    }
}