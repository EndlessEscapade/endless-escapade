using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class Scrapwizard : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Scrapwizard");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 44;
            NPC.height = 56;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;
        }


        public GuardBrute myGuard;

        public int attackDelay;

        public int currentAttack;

        public override void AI()
        {
            #region Initializing
            if (NPC.ai[1] == 0)
            {
                myGuard = Main.npc[(int)NPC.ai[0]].ModNPC as GuardBrute;
            }
            #endregion

            NPC.ai[1]++; //Ticker

            if (attackDelay <= 0) PickAttack();

            switch(currentAttack)
            {
                case 0:
                    break;
            }
        }

        public void PickAttack()
        {
            currentAttack = 0;
            myGuard.currentAttack = 0;

            attackDelay = 60;
        }
    }
}