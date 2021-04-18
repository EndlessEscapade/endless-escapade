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

        public override int ThisRuneID => (int)RuneID.IceRune;
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 50;
        }

        public override void CustomAI()
        {
            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.03f));
            EEMod.MainParticles.SpawnParticles(projectile.Center, Vector2.Zero, default, 180, 4, Color.Lerp(Color.SkyBlue, Color.White, Main.rand.NextFloat(0f, 1f)), new CircularMotion(32 + (int)(Math.Sin(projectile.ai[0] / 20) * 16), 32 + (int)(Math.Cos(projectile.ai[0] / 20) * 16), 0.25f, projectile), new AfterImageTrail(0.95f));
        }

        public override void CustomPostDraw()
        {
            Main.spriteBatch.Draw(mod.GetTexture("Projectiles/Runes/PermafrostRuneGlow"), projectile.position - Main.screenPosition, Color.White * ((int)(Math.Sin(projectile.ai[0]) / 2) + 0.5f)); //FIX THIS DRAW
        }
    }
}