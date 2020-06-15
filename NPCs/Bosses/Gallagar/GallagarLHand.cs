using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.Bosses.Gallagar
{
    public class GallagarLHand : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gallagar's Hand");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 500;
            npc.defense = 13;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.width = 40;
            npc.height = 40;
            npc.damage = 18;
            npc.value = Item.sellPrice(0, 0, 0, 0);
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.noTileCollide = true;
        }
        private void Move(NPC npc, float sped, float TR, Vector2 addon)
        {
            Vector2 moveTo = npc.Center + addon;
            float speed = sped;
            Vector2 move = moveTo - this.npc.Center;
            float magnitude = move.Length(); //(float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TR;

            move = (this.npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            this.npc.velocity = move;
        }
        /*public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			int lifeMax = npc.lifeMax = (int)(npc.lifeMax * 0.80 * bossLifeScale);
			int defense = 15;
			int damage = 38;
			npc.defense = defense;
			npc.damage = damage;
			if (InteritosWorld.GenkaiMode)
			{
				npc.lifeMax = (int)(lifeMax * 0.70f * bossLifeScale);
				npc.defense = (int)(defense * 0.12f);
				npc.damage = (int)(damage * 0.22f);
			}
		}*/
        private int Ypos = 40;
        private int Xpos = 88;
        private int attackCounter = 300;
        private bool usefulFlag = true;
        private NPC OwnerNpc => Main.npc[(int)npc.ai[0]];
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (OwnerNpc.ai[0] % 400 == 0)
            {
                usefulFlag = true;
                npc.ai[1] = 0;
            }
            if (OwnerNpc.ai[1] == 2)
            {
                npc.ai[3] = 0;
                Ypos = -90;
                Xpos = 50;
                usefulFlag = true;
                double deg = (double)npc.ai[2];
                double rad = deg * (Math.PI / 180);
                npc.velocity.X += (float)(Math.Cos(rad) * 1.8f);
                npc.velocity.Y += (float)(Math.Sin(rad) * 1.8f);
                npc.ai[2] += 5;
            }
            else if (OwnerNpc.ai[1] == 3)
            {
                npc.ai[3] = 0;
                Ypos = -90;
                Xpos = 150;
                if (usefulFlag)
                {
                    npc.ai[1]++;
                    if (npc.ai[1] == 100)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 10, -(npc.Center.X - player.Center.X), -(npc.Center.Y - player.Center.Y), ModContent.ProjectileType<GBeamSpawn>(), npc.damage / 4, 1, Main.myPlayer, npc.spriteDirection == 1 ? 0 : 1, npc.whoAmI);
                        usefulFlag = false;
                        npc.ai[1] = 0;
                    }
                }
            }
            else if (OwnerNpc.ai[1] == 4)
            {
                Move(OwnerNpc, 33, 2, new Vector2(Xpos * -OwnerNpc.spriteDirection, Ypos));
                npc.ai[3]++;
                if (npc.ai[3] < 60)
                {
                    Ypos = -90;
                    Xpos = 100;
                }
                else
                {
                    Ypos = 60;
                    Xpos = 100;
                }
            }
            else
            {
                npc.ai[3] = 0;
                usefulFlag = true;
                Ypos = 40;
                Xpos = 88;
            }
            if (OwnerNpc.ai[1] != 4)
                Move(OwnerNpc, 27, 6, new Vector2(Xpos * -OwnerNpc.spriteDirection, Ypos));

            if (OwnerNpc.active && OwnerNpc.modNPC is Gallagar)
                npc.spriteDirection = OwnerNpc.spriteDirection;
            else
            {
                npc.active = false;
                return;
            }

            /* attackCounter++;
             if (attackCounter >= 600)
             {
                 int projectile = Projectile.NewProjectile(new Vector2((int)npc.Center.X, (int)npc.Center.Y), npc.DirectionTo(Main.player[npc.target].Center) * 8, ModContent.ProjectileType<HadesRing>(), 30, 0, Main.myPlayer);
                 attackCounter = 0;
             }*/

        }
        public override void FindFrame(int frameHeight) //Frame counter
        {
            if (npc.frameCounter++ > 4)
            {
                npc.frameCounter = 0;
                npc.frame.Y = npc.frame.Y + frameHeight;
            }
            if (npc.frame.Y >= frameHeight * 6)
            {
                npc.frame.Y = 0;
                return;
            }
        }
        private int frameNumber = 0;
    }
}
