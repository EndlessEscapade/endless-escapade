using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Summon;
using Terraria.DataStructures;

namespace EEMod.Items.Weapons.Summon.Whips
{
    public class ArclashProj : Whip
    {
        public override int segments => 16;
        public override float rangeMult => 1f;
        public override int summonTagDamage => 3;
        public override int summonTagCrit => 10;
        public override int buffGivenToPlayer => -1;
        public override int buffTime => 120;
        public override Color stringColor => Color.Gold;
        public override string texture => "EEMod/Items/Weapons/Summon/Whips/ArclashProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arclash");
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            // Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void NpcEffects(NPC target, int damage, float knockback, bool crit)
        {
            if(Main.rand.NextBool(5))
            {
                //Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<ArclashTarget>(), 0, 0f);
            }
        }
    }
}