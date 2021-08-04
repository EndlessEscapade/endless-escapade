using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    /// <summary>
    /// Projectiles that are not meant to interact with anything, they're just visuals
    /// </summary>
    public abstract class VisualProjectile : EEProjectile
    {
        public override bool? CanCutTiles() => false;

        public override bool CanDamage() => false;

        public override bool? CanHitNPC(NPC target) => false;

        public override bool CanHitPlayer(Player target) => false;

        public override bool CanHitPvp(Player target) => false;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => false;
    }
}