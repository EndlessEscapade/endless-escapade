using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.ID;

namespace EEMod.Projectiles.Runes
{
    public class PermafrostRune : Rune
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Permafrost Rune");
            Main.projFrames[projectile.type] = 1;
        }

        //public override int RuneID => RuneID.PermafrostRune;
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 50;
        }

        public override void AI()
        {
            
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            
        }
    }
}