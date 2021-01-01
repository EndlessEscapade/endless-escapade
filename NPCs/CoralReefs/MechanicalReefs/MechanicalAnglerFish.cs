using EEMod.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class MechanicalAnglerFish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Angler Fish");
            Main.npcFrameCount[npc.type] = 3;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameNumber * frameHeight;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;
            npc.damage = 50;

            npc.width = 46;
            npc.height = 48;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            if (frameNumber == 2)
            {
                if (npc.ai[3] == 0)
                {
                    npc.ai[3] = Projectile.NewProjectile(npc.Center + new Vector2(-10, 0), Vector2.Zero, ModContent.ProjectileType<MechanicalLure>(), npc.damage, 0f, Owner: npc.whoAmI);
                }

                npc.velocity = Vector2.Zero;

                if (Math.Abs(target.position.X - npc.position.X) > 320)
                {
                    Main.projectile[(int)npc.ai[3]].Kill();
                    frameNumber = 0;
                }
                npc.rotation = 0;
                if (Main.projectile[(int)npc.ai[3]].ai[0] == 1)
                {
                    Main.NewText(1 / Vector2.Distance(npc.Center, Main.projectile[(int)npc.ai[3]].Center));
                    Lighting.AddLight(npc.Center, 10 / Vector2.Distance(npc.Center, Main.projectile[(int)npc.ai[3]].Center), 0.1f, 0.1f);
                }
                npc.spriteDirection = 1;
            }
            else
            {
                if (Math.Abs(target.position.X - npc.position.X) < 160)
                {
                    npc.ai[2]++;
                    if (npc.ai[2] >= 15 && frameNumber <= 2)
                    {
                        frameNumber++;
                        npc.ai[2] = 0;
                    }
                }
                npc.velocity = Vector2.Normalize(target.position - npc.position) * 4;

                if (target.position.X > npc.position.X)
                {
                    npc.spriteDirection = -1;
                }
                else
                {
                    npc.spriteDirection = 1;
                }

                npc.rotation = npc.velocity.X / 32;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color DrawColor)
        {
            if (npc.ai[3] != 0)
            {
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                Texture2D LureChain = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/Enemy/MechanicalLureChain");
                float distance = Vector2.Distance(npc.Center, Main.projectile[(int)npc.ai[3]].position) / LureChain.Height;
                Vector2 pos = npc.position - Main.screenPosition + new Vector2(x: (npc.width / 2) - (LureChain.Width / 2) - 10, y: npc.height / 2);
                for (int i = 0; i < distance; i++)
                {
                    Main.spriteBatch.Draw(LureChain, pos + new Vector2(x: 0, y: i * LureChain.Height), Color.White);
                }
            }
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, 40), npc.frame, DrawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void NPCLoot()
        {
            Main.projectile[(int)npc.ai[3]].Kill();
        }
    }
}