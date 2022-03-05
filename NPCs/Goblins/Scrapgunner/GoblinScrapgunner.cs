using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapgunner
{
    public class GoblinScrapgunner : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Scrapgunner");
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

        public bool aggro;
        public override void AI()
        {
            NPC.velocity.X *= 0.95f;

            NPC.spriteDirection = Main.LocalPlayer.Center.X < NPC.Center.X ? 1 : -1;

            staffCenter = NPC.Center + new Vector2(12 * NPC.spriteDirection, 14);

            NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            if (Vector2.DistanceSquared(player.Center, NPC.Center) <= 16 * 16 * 24 * 24 || NPC.life < NPC.lifeMax)
            {
                aggro = true;
            }

            if (aggro)
            {
                NPC.ai[0]++;

                if (NPC.ai[0] % 120 == 0)
                {
                    if (Main.rand.NextBool(5))
                    {
                        rocketJumping = true;

                        NPC.velocity += new Vector2(0, -12f);
                    }
                    else
                    {
                        ShootBall(NPC.Center, Vector2.Zero);
                    }
                }

                if (oldVel.Y < -0.02f && NPC.velocity.Y >= -0.02f && NPC.velocity.Y < 0 && rocketJumping)
                {
                    ShootBall(NPC.Center, Vector2.Zero);
                    rocketJumping = false;
                    NPC.ai[0] = 0;
                }
            }

            oldVel = NPC.velocity;
        }

        float staffRot;
        Vector2 staffCenter;
        public Vector2 oldVel;
        public bool rocketJumping;

        public void ShootBall(Vector2 pos, Vector2 addVel)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(pos, 0, 0, DustID.Smoke);
                Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                Main.dust[dust].noGravity = true;
            }

            NPC.velocity += (((Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center) + addVel) * 3).X < 0 ? new Vector2(0.5f, 0) : new Vector2(-0.5f, 0));

            int newBolt = NPC.NewNPC(new Terraria.DataStructures.EntitySource_Parent(NPC), (int)pos.X, (int)pos.Y, ModContent.NPCType<Scrapball>(), 20, 2, Main.myPlayer);

            Main.npc[newBolt].velocity = ((Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center) + addVel) * 3);

            SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color defaultDrawColor = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));

            Texture2D ShamanGlow = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/Scrapgunner/GoblinScrapgunner").Value;

            Main.spriteBatch.Draw(ShamanGlow, NPC.Center - Main.screenPosition + new Vector2(0, 4), null, defaultDrawColor, NPC.rotation, ShamanGlow.Bounds.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End(); Main.spriteBatch.Begin();

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color defaultDrawColor = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));

            Texture2D ShamanStaff = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/Scrapgunner/ScrapgunnerGun").Value;

            Main.spriteBatch.Draw(ShamanStaff, staffCenter - Main.screenPosition, null, defaultDrawColor, staffRot, (ShamanStaff.Bounds.Size() / 2f) + new Vector2(1, 4), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);


            //shaman's hand
            Texture2D ShamanHand = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/Scrapgunner/ScrapgunnerHand").Value;

            Main.spriteBatch.Draw(ShamanHand, NPC.Center - Main.screenPosition + new Vector2(11 * NPC.spriteDirection, 4 + 11), null, defaultDrawColor, 0f, new Vector2(11, 39), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }

    public class Scrapball : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrapball");
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;

            NPC.alpha = 0;

            NPC.friendly = false;
            NPC.scale = 1f;

            NPC.aiStyle = -1;
            NPC.lifeMax = 1;

            NPC.noGravity = false;

            NPC.defense = 0;

            NPC.noTileCollide = true;

            NPC.DeathSound = SoundID.Item10;

            NPC.damage = 20;
        }

        public override void AI()
        {
            
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void OnKill()
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, 0, 0, DustID.Smoke);
                Main.dust[dust].velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));
            }
        }
    }
}