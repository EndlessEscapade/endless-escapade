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
    public class GuardBrute : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wizard's Guard");
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


        public Scrapwizard myWizard;

        public int currentAttack;

        public override void AI()
        {
            #region Initializing
            if (NPC.ai[0] == 0)
            {
                myWizard = Main.npc[NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Scrapwizard>(), ai0: NPC.whoAmI)].ModNPC as Scrapwizard;
            }
            #endregion

            NPC.ai[0]++; //Ticker
        }
    }
}