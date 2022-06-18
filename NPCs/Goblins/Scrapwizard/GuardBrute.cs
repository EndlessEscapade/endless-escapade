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
    [AutoloadHead]
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

            NPC.lifeMax = 8000;
            NPC.defense = 10;

            NPC.width = 108;
            NPC.height = 108;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;

            NPC.noGravity = true;

            NPC.knockBackResist = 0f;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public Scrapwizard myWizard;

        public Rectangle myRoom;

        public int currentAttack = 0;

        public bool fightBegun = false;

        public float teleportFloat;

        public int frameY;

        public override void AI()
        {
            NPC.active = true;

            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (target.Center.X < NPC.Center.X) NPC.direction = -1;
            else NPC.direction = 1;

            NPC.spriteDirection = NPC.direction;

            NPC.velocity.Y += 0.6f; //Force of gravity

            #region Initializing
            if (!fightBegun)
            {
                if (NPC.ai[0] == 0)
                {
                    if (NPC.ai[1] == 0)
                    {
                        NPC.ai[1] = 744 * 16 - 384;
                        NPC.ai[2] = 118 * 16 - 464;
                    }

                    myRoom = new Rectangle((int)NPC.ai[1] + 384, (int)NPC.ai[2] + 464, 112 * 16, 36 * 16);

                    NPC.Center = myRoom.Center.ToVector2() + new Vector2(96, 128);

                    myWizard = Main.npc[NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Scrapwizard>(), ai0: NPC.whoAmI, ai1: 0)].ModNPC as Scrapwizard;

                    myWizard.myGuard = this;
                    myWizard.myRoom = myRoom;
                    myWizard.NPC.Center = new Vector2(myRoom.X + (56 * 16), myRoom.Y + (20 * 16));

                    NPC.ai[0]++; //Ticker
                }
            }     
            #endregion
            else 
            {
                switch (myWizard.currentAttack)
                {

                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            myWizard.InitShader(spriteBatch);

            Rectangle rect = new Rectangle(0, frameY * 108, 108, 108);
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/GuardBrute").Value;

            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, rect, Color.White, NPC.rotation, NPC.spriteDirection == 1 ? new Vector2(54, 54) : new Vector2(54, 54), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }

        public override void OnKill()
        {
            myWizard.attackTimer = 0;
            myWizard.fightPhase = 2;

            base.OnKill();
        }
    }
}