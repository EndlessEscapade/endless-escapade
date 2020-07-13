using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Pengun      //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class PengunProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PengunProjectile");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            aiType = ProjectileID.Bullet;
        }
    }
}