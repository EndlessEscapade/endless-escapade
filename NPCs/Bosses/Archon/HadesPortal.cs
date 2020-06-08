using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.Bosses.Archon
{
    public class HadesPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hades Portal");
        }

        public override void SetDefaults()
        {
            //npc.aiStyle = 0;
            //npc.dontTakeDamage = true;
            //npc.damage = 0;
            //npc.lifeMax = 1;

            //npc.noGravity = true;
            projectile.width = 64;
            projectile.height = 48;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            {
                if (attackCounter > 0)
                    attackCounter--;
                if (lifeTime > 0)
                    lifeTime--;
                else
                {
                    // Main.npc[npc.type].StrikeNPC(1, 0f, 0);
                }

                // Player target = Main.player[npc.target];
                /* if (attackCounter <= 0 && target.WithinRange(npc.Center, 1000) && Collision.CanHit(npc.Center, 1, 1, target.Center, 1, 1))
                {
                    int projectile = Projectile.NewProjectile(new Vector2(npc.Center.X + (int)(Main.rand.Next(-32, 33), npc.Center.Y), new Vector2(0, 1), ModContent.ProjectileType<HadesRain>(), 20, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 1800;
                    attackCounter = Main.rand.Next(11, 31);
                    npc.netUpdate = true;
                } */
                projectile.Kill();

                Player target = Main.player[(int)projectile.ai[0]];

                if (attackCounter <= 0 && target.WithinRange(projectile.Center, 1000) && Collision.CanHit(projectile.Center, 1, 1, projectile.Center, 1, 1))
                {
                    int proj = Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-32, 33), projectile.Center.Y, 0, 1, ModContent.ProjectileType<HadesRain>(), 20, 0, Main.myPlayer);
                    Main.projectile[proj].timeLeft = 1800;
                    attackCounter = Main.rand.Next(11, 33);
                    projectile.netUpdate = true;
                }
            }
        }

        private int attackCounter = 30;
        private int lifeTime = 300;
    }
}