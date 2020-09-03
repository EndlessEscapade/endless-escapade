using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using EEMod;

namespace EEMod.NPCs.funny
{
    public class Perceptionator : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perceptionator");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 5)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 4)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.friendly = false;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 75000;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.height = 199;
            npc.width = 114;
            npc.noGravity = true;
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.rotation = (target.Center - npc.Center).ToRotation() - MathHelper.PiOver2;

            if (npc.ai[3] == 0)
                npc.ai[3] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Jawbrawn>());


        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color DrawColor)
        {
            npc.TargetClosest();
            //Player player = Main.player[npc.target];

            Texture2D LureChain = TextureCache.FleshChain;
            float distance = Vector2.Distance(npc.Center, Main.npc[(int)npc.ai[3]].Center) / LureChain.Height;
            for (int i = 0; i < distance; i++)
            {
                Vector2 position = npc.Center - Main.screenPosition + new Vector2((npc.width / 2) - (LureChain.Width / 2), (npc.height / 2)) + Vector2.Lerp(npc.Center, Main.npc[(int)npc.ai[3]].Center, i / Vector2.Distance(npc.Center, Main.npc[(int)npc.ai[3]].Center));
                Vector2 origin = new Vector2(LureChain.Width * 0.5f, LureChain.Height * 0.5f);
                //Main.spriteBatch.Draw(LureChain, , npc.frame, Color.White, Vector2.Normalize(Main.npc[(int)npc.ai[3]].Center - npc.Center).ToRotation(), origin, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(LureChain, position, LureChain.Bounds, DrawColor, npc.rotation, origin, 1, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
