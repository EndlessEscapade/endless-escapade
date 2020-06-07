using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace InteritosMod.NPCs.Archon
{
    public class Magic2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hades Magic");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 0;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.width = 26;
            npc.height = 46;
            npc.lifeMax = 1;

            npc.noGravity = true;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            {
                if (attackCounter > 0)
                {
                    attackCounter--;
                }

                Player target = Main.player[npc.target];
                if (attackCounter <= 0 && target.WithinRange(npc.Center, 1000) && Collision.CanHit(npc.Center, 1, 1, target.Center, 1, 1))
                {
                    Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(npc.Center, direction * 10, ModContent.ProjectileType<HadesFireball>(), 20, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 1800;
                    attackCounter = Main.rand.Next(15, 41);
                    npc.netUpdate = true;
                }
            }
        }

        private int attackCounter = 30;

    }
}