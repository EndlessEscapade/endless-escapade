using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class Dreadmine : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreadmine");
            //  Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 40;
            Projectile.penetrate = -1;

            // Projectile.tileCollide = false;
            // Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.damage = 200;
        }

        private NPC OwnerNpc => Main.npc[(int)Projectile.ai[0]];

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            Projectile.Center = new Vector2(OwnerNpc.ai[2], OwnerNpc.ai[3]);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Pixie);
            }
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
            Projectile.Kill();
            OwnerNpc.StrikeNPC(55, 0, 0);
        }
    }
}