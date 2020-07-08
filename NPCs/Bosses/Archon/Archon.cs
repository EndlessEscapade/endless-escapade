using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
// using static Terraria.ModLoader.ModContent;

namespace EEMod.NPCs.Bosses.Archon
{
    [AutoloadBossHead]
    internal class Archon : ModNPC
    {
        public override void SetDefaults()
        {
            music = MusicID.Boss3;

            npc.aiStyle = 0;
            npc.lifeMax = 4000;
            npc.defense = 10;
            npc.damage = 40;
            npc.width = 154;
            npc.height = 156;
            npc.boss = true;
            npc.HitSound = new LegacySoundStyle(3, 23, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(3, 55, Terraria.Audio.SoundType.Sound);

            npc.noGravity = true;
        }

        public override bool CheckActive()
        {
            return Helpers.AnyPlayerAlive;
        }

        private int attackCounter = 120;
        private int attackCounter2 = 130;
        private int attackCounter3 = 300;
        private int attackCounter4 = 150;
        private float attackCounterMult = 1;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
            writer.Write(attackCounter2);
            writer.Write(attackCounter3);
            writer.Write(attackCounter4);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
            attackCounter2 = reader.ReadInt32();
            attackCounter3 = reader.ReadInt32();
            attackCounter4 = reader.ReadInt32();
        }

        private bool secondPhase = false;
        private bool thirdPhase = false;

        private int meditationCounter = 0;

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            {
                npc.TargetClosest();
                Player target = Main.player[npc.target];

                //Idle
                if (target.WithinRange(npc.Center, 6400))
                {
                    npc.velocity = Vector2.Normalize(target.Center - npc.Center) * 2;
                    npc.netUpdate = true;
                }
                if (attackCounter4 > 0)
                {
                    attackCounter4--;
                }
                if (attackCounter4 <= 0 && target.WithinRange(npc.Center, 1000) && Collision.CanHit(npc.Center, 1, 1, target.Center, 1, 1))
                {
                    int projectile = Projectile.NewProjectile(target.Center, new Vector2(0, 0), ModContent.ProjectileType<HadesRing>(), 0, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 120;
                    attackCounter4 = (int)(Main.rand.Next(200, 601) * attackCounterMult);
                    npc.netUpdate = true;
                }




                //Meditation
                meditationCounter++;
                if (meditationCounter >= 420)
                {
                    npc.life++;
                }




                //Attack 2
                if (attackCounter2 > 0)
                {
                    attackCounter2--;
                }
                if (attackCounter2 <= 0 && target.WithinRange(npc.Center, 5000) && secondPhase)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 direction = new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));

                        Projectile.NewProjectile(new Vector2((int)npc.Center.X, (int)npc.Center.Y), direction * 8, ModContent.ProjectileType<HadesFireball>(), 30 * (int)(1 / attackCounterMult), 0, Main.myPlayer);
                        attackCounter2 = (int)(Main.rand.Next(200, 351) * attackCounterMult);
                        npc.netUpdate = true;
                    }
                }




                //Phase 2
                if (npc.life <= npc.lifeMax / 3 && !secondPhase)
                {
                    Main.NewText("You underestimate his power.", new Color(81, 0, 73));
                    npc.damage *= 2;
                    npc.defense *= (int)1.5f;
                    attackCounterMult = 0.75f;
                    npc.life = (int)npc.lifeMax * 2 / 3;

                    magic = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 128, ModContent.NPCType<Magic2>(), 60 * (int)(1 / attackCounterMult), 0, Main.myPlayer);
                    npc.alpha = 127;
                    secondPhase = true;
                }
                if (secondPhase)
                {
                    Vector2 pos = npc.Center + new Vector2(144, 0).RotatedBy(MathHelper.ToRadians(rotation));
                    Main.npc[magic].Center = pos;
                    rotation += 3;
                }




                //Phase 3
                if (npc.life <= npc.lifeMax / 3 && secondPhase && !thirdPhase)
                {
                    Main.NewText("The power of my master will tear you apart.", new Color(81, 0, 73));
                    npc.defense *= (int)1.5f;
                    attackCounterMult = 0.5f;
                    npc.life = (int)npc.lifeMax * 1 / 2;

                    magic2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 128, ModContent.NPCType<Magic2>(), 60 * (int)(1 / attackCounterMult), 0, Main.myPlayer);
                    rotation2 = rotation + 180;
                    npc.alpha = 193;
                    thirdPhase = true;
                }
                if (thirdPhase)
                {
                    Vector2 pos = npc.Center + new Vector2(144, 0).RotatedBy(MathHelper.ToRadians(rotation2));
                    Main.npc[magic2].Center = pos;
                    rotation2 += 3;
                }




                //Preventing player from running away
                if (!target.WithinRange(npc.Center, 2400))
                {
                    Main.NewText("Running is futile.", new Color(81, 0, 73));
                    npc.Teleport(new Vector2(target.Center.X, target.Center.Y - 256));
                }
            }
        }

        public int magic = 0;
        public int rotation = 0;
        public int magic2 = 0;
        public int rotation2 = 0;

        public override bool CheckDead()
        {
            // new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f)
            int goreIndex = Gore.NewGore(npc.Center - new Vector2(24), default(Vector2), mod.GetGoreSlot("Gores/ArchonGore"), 1f);
            Main.gore[goreIndex].scale = 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
            //foreach (NPC npc in Main.npc.Where(n => n.modNPC is Magic2))
            //{
            //    npc.StrikeNPC(1, 0f, 0);
            //}
            //foreach (NPC npc in Main.npc.Where(n => n.modNPC is MagicMinion))
            //{
            //    npc.StrikeNPC(1, 0f, 0);
            //}
            Main.npc[magic].StrikeNPC(1, 0f, 0);
            Main.npc[magic2].StrikeNPC(1, 0f, 0);
            Main.NewText("This isn't the last you'll see of me...", new Color(81, 0, 73));
            Main.NewText("I'll serve my master until the end...", new Color(81, 0, 73));
            return true;
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            meditationCounter = 0;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            meditationCounter = 0;
        }
    }
}