using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.NPCs.CoralReefs
{
    public class Ball : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Calamity be like
            DisplayName.SetDefault("Ball");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.lifeMax = 50;
            npc.damage = 13;
            npc.defense = 3;

            npc.width = 64;
            npc.width = 64;

            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.npcSlots = 1f;
            npc.buffImmune[BuffID.Confused] = true;
            npc.lavaImmune = false;
            npc.behindTiles = true;
            npc.dontTakeDamage = true;
            npc.value = Item.sellPrice(0, 0, 0, 75);
        }

        private static float X(float t,
float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow((1 - t), 3) +
                x1 * 3 * t * Math.Pow((1 - t), 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }
        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow((1 - t), 3) +
                 y1 * 3 * t * Math.Pow((1 - t), 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }

        public void DrawHead(SpriteBatch spriteBatch, string headTexture, string glowMaskTexture, NPC head, Color drawColor, Vector2 ifYouReallyWantToo)
        {
            if (head != null && head.active && head.modNPC != null && head.modNPC is Ball)
            {
                Texture2D neckTex2D = TextureCache.Chain;
                Vector2 neckOrigin = npc.Center;
                Vector2 connector = ifYouReallyWantToo;
                float chainsPerUse = 0.05f;
                float POINT1X = (connector.X - neckOrigin.X) * 1 + neckOrigin.X;
                float POINT2X = (connector.X - neckOrigin.X) * 1 + neckOrigin.X;
                float POINT1Y = (connector.Y - neckOrigin.Y) * 0 + neckOrigin.Y;
                float POINT2Y = (connector.Y - neckOrigin.Y) * 0 + neckOrigin.Y;
                for (float i = 0; i <= 1; i += chainsPerUse)
                {
                    if (i % chainsPerUse * 2 == 0)
                    {

                    }
                    Vector2 distBetween;
                    float projTrueRotation;
                    distBetween = new Vector2(
                    X(i, neckOrigin.X, POINT1X, POINT2X, connector.X) -
                    X(i - chainsPerUse, neckOrigin.X, POINT1X, POINT2X, connector.X),
                    Y(i, neckOrigin.Y, POINT1Y, POINT2Y, connector.Y) -
                    Y(i - chainsPerUse, neckOrigin.Y, POINT1Y, POINT2Y, connector.Y));
                    projTrueRotation = distBetween.ToRotation() - (float)Math.PI / 2;
                    spriteBatch.Draw(neckTex2D, new Vector2(X(i, neckOrigin.X, POINT1X, POINT2X, connector.X) - Main.screenPosition.X, Y(i, neckOrigin.Y, POINT1Y, POINT2Y, connector.Y) - Main.screenPosition.Y),
                    new Rectangle(0, 0, neckTex2D.Width, neckTex2D.Height), drawColor, projTrueRotation,
                    new Vector2(neckTex2D.Width * 0.5f, neckTex2D.Height * 0.5f), 1, SpriteEffects.None, 0f);
                }
                //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.22f);
        }

        /* public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.);
        } */

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                Projectile.NewProjectile(npc.Center + new Vector2(0, -100), Vector2.Zero, ModContent.ProjectileType<Dreadmine>(), 150, 0f, Main.myPlayer, npc.whoAmI);
            }
            npc.ai[0]++;
            double deg = (double)npc.ai[1] + 10;
            double rad = deg * (Math.PI / 180) + Math.PI / 2;
            double dist = 180;


            npc.ai[2] = npc.Center.X - (float)(Math.Cos(rad) * dist) - npc.width / 2;
            npc.ai[3] = npc.Center.Y - (float)(Math.Sin(rad) * dist) - npc.height / 2;

            npc.ai[1] = ((float)Math.Cos(npc.ai[0] * (Math.PI / 180)) * 10);

            npc.velocity.Y = 2;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color DrawColor)
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            DrawColor = npc.GetAlpha(DrawColor);
            DrawHead(spriteBatch, "NPCs/CoralReefs/Chain", "NPCs/CoralReefs/Chain", npc, DrawColor, new Vector2(npc.ai[2], npc.ai[3]));
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, 8), null, DrawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
