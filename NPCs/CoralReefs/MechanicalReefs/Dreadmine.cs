using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class Dreadmine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreadmine");
            //  Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 40;
            projectile.penetrate = -1;

            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.damage = 200;
        }

        private NPC OwnerNpc => Main.npc[(int)projectile.ai[0]];

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            projectile.Center = new Vector2(OwnerNpc.ai[2], OwnerNpc.ai[3]);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 55);
            }
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
            projectile.Kill();
            OwnerNpc.StrikeNPC(55, 0, 0);
        }
    }
}