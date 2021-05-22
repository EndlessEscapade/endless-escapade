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
            projectile.width = 0;
            projectile.height = 0;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.ownerHitCheck = true;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void NpcEffects(NPC target, int damage, float knockback, bool crit)
        {
            
        }
    }
}