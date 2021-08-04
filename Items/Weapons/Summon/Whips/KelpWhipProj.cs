using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Summon;

namespace EEMod.Items.Weapons.Summon.Whips
{
    public class KelpWhipProj : Whip
    {
        public override int segments => 24;
        public override float rangeMult => 0.8f;
        public override int summonTagDamage => 3;
        public override int summonTagCrit => 10;
        public override int buffGivenToPlayer => -1;
        public override int buffTime => 120;
        public override Color stringColor => Color.Gold;
        public override string texture => "EEMod/Items/Weapons/Summon/Whips/KelpWhipProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Whip");
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void NpcEffects(NPC target, int damage, float knockback, bool crit)
        {
            
        }
    }
}