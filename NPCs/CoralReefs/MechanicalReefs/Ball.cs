using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class Ball : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ball");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 50;
            NPC.damage = 0;
            NPC.defense = 3;

            NPC.width = 64;
            NPC.width = 64;

            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.npcSlots = 1f;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.lavaImmune = false;
            NPC.behindTiles = true;
            NPC.dontTakeDamage = true;
            NPC.friendly = true;
            NPC.value = Item.sellPrice(0, 0, 0, 75);
        }

        private static float X(float t,
float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 3) +
                x1 * 3 * t * Math.Pow(1 - t, 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow(1 - t, 3) +
                 y1 * 3 * t * Math.Pow(1 - t, 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }

        public void DrawHead(SpriteBatch spriteBatch, string headTexture, string glowMaskTexture, NPC head, Color drawColor, Vector2 ifYouReallyWantToo)
        {
            if (head != null && head.active && head.modNPC != null && head.modNPC is Ball)
            {
                Texture2D neckTex2D = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/MechanicalReefs/DreadmineChain");
                Vector2 neckOrigin = NPC.Center;
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
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2;
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
            NPC.lifeMax = (int)(NPC.lifeMax * 0.22f);
        }

        /* public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.);
        } */

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                Projectile.NewProjectile(NPC.Center + new Vector2(0, -100), Vector2.Zero, ModContent.ProjectileType<Dreadmine>(), 150, 0f, Main.myPlayer, NPC.whoAmI);
            }
            NPC.ai[0]++;
            double deg = (double)NPC.ai[1] + 10;
            double rad = deg * (Math.PI / 180) + Math.PI / 2;
            double dist = 180;

            NPC.ai[2] = NPC.Center.X - (float)(Math.Cos(rad) * dist) - NPC.width / 2;
            NPC.ai[3] = NPC.Center.Y - (float)(Math.Sin(rad) * dist) - NPC.height / 2;

            NPC.ai[1] = (float)Math.Cos(NPC.ai[0] * (Math.PI / 180)) * 10;

            NPC.velocity.Y = 2;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color DrawColor)
        {
            NPC.TargetClosest(true);
            //Player player = Main.player[npc.target]; // unused
            DrawColor = NPC.GetAlpha(DrawColor);
            DrawHead(spriteBatch, "NPCs/CoralReefs/MechanicalReefs/DreadmineChain", "NPCs/CoralReefs/MechanicalReefs/DreadmineChain", NPC, DrawColor, new Vector2(NPC.ai[2], NPC.ai[3]));
            Texture2D texture = Main.npcTexture[NPC.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0, 8), null, DrawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}