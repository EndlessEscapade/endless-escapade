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
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.05f));
            EEMod.Particles.Get("Main").SpawnParticles(projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), default, 180, 4, Color.GhostWhite, new CircularMotion(48 + (int)(Math.Sin(projectile.ai[0]) * 16), 48 + (int)(Math.Cos(projectile.ai[0]) * 16), 3, projectile));
        }
    }
}