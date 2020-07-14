using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class Tentacle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
        }

        public override void SetDefaults()
        {
            npc.width = 10;
            npc.height = 78;
            npc.damage = 20;
            npc.aiStyle = -1;
            npc.lifeMax = 1000;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
        }
        Vector2 startingPosition;
        Vector2 distance;
        bool isGrabbing;
        bool isRetrating = false;
        bool yeet;
        public override bool CheckActive()
        {
            return false;
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            Rectangle npcHitBox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
            Rectangle playerHitBox = new Rectangle((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
            if (isGrabbing && !isRetrating)
            {
                (Main.npc[(int)npc.ai[2]].modNPC as KrakenHead).GETHIMBOIS = true;
                if (!yeet)
                {
                    Main.npc[(int)npc.ai[2]].ai[0] = 0;
                    yeet = true;
                }
                Main.player[npc.target].Center = npc.Center;
                npc.velocity *= 0.94f;
            }
            else
            {
                if (npc.ai[0] == 0)
                {
                    startingPosition = npc.Center;
                }
                npc.ai[0]++;
                if (distance.X > 800)
                {
                    npc.ai[1] = 1;
                }
                if (npc.ai[1] == 0)
                    npc.velocity.X = npc.ai[0] / 5;
                else
                {
                    npc.velocity.X = -5;
                    if (distance.X < 0)
                    {
                        npc.life = 0;
                    }
                }
            }
            if (npc.life < 1000)
            {
                isRetrating = true;
            }
            if (Main.npc[(int)npc.ai[2]].ai[0] >= 278)
            {
                isRetrating = true;
            }
            if (isRetrating)
            {
                npc.velocity.X = -5;
            }

            distance = (npc.Center - startingPosition);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            isGrabbing = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.Tentacle;
            Main.spriteBatch.Draw(texture, npc.Center + new Vector2(npc.width / 2, 0) - Main.screenPosition - distance / 2, new Rectangle(texture.Width - (int)distance.X, 0, (int)distance.X, texture.Height), Color.White, npc.rotation, new Rectangle(texture.Width - (int)distance.X, 0, (int)distance.X, texture.Height).Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
     }
}
