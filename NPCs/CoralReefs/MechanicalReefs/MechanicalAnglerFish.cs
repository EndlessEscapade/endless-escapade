using EEMod.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class MechanicalAnglerFish : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Angler Fish");
            Main.npcFrameCount[NPC.type] = 3;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameNumber * frameHeight;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;
            NPC.damage = 50;

            NPC.width = 46;
            NPC.height = 48;

            NPC.noGravity = true;

            NPC.buffImmune[BuffID.Confused] = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (frameNumber == 2)
            {
                if (NPC.ai[3] == 0)
                {
                    NPC.ai[3] = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), NPC.Center + new Vector2(-10, 0), Vector2.Zero, ModContent.ProjectileType<MechanicalLure>(), NPC.damage, 0f, Owner: NPC.whoAmI);
                }

                NPC.velocity = Vector2.Zero;

                if (Math.Abs(target.position.X - NPC.position.X) > 320)
                {
                    Main.projectile[(int)NPC.ai[3]].Kill();
                    frameNumber = 0;
                }
                NPC.rotation = 0;
                if (Main.projectile[(int)NPC.ai[3]].ai[0] == 1)
                {
                    Main.NewText(1 / Vector2.Distance(NPC.Center, Main.projectile[(int)NPC.ai[3]].Center));
                    Lighting.AddLight(NPC.Center, 10 / Vector2.Distance(NPC.Center, Main.projectile[(int)NPC.ai[3]].Center), 0.1f, 0.1f);
                }
                NPC.spriteDirection = 1;
            }
            else
            {
                if (Math.Abs(target.position.X - NPC.position.X) < 160)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] >= 15 && frameNumber <= 2)
                    {
                        frameNumber++;
                        NPC.ai[2] = 0;
                    }
                }
                NPC.velocity = Vector2.Normalize(target.position - NPC.position) * 4;

                if (target.position.X > NPC.position.X)
                {
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }

                NPC.rotation = NPC.velocity.X / 32;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color DrawColor)
        {
            if (NPC.ai[3] != 0)
            {
                NPC.TargetClosest();
                Player player = Main.player[NPC.target];
                Texture2D LureChain = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Projectiles/Enemy/MechanicalLureChain").Value;
                float distance = Vector2.Distance(NPC.Center, Main.projectile[(int)NPC.ai[3]].position) / LureChain.Height;
                Vector2 pos = NPC.position - Main.screenPosition + new Vector2(x: (NPC.width / 2) - (LureChain.Width / 2) - 10, y: NPC.height / 2);
                for (int i = 0; i < distance; i++)
                {
                    Main.spriteBatch.Draw(LureChain, pos + new Vector2(x: 0, y: i * LureChain.Height), Color.White);
                }
            }
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0, 40), NPC.frame, DrawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void NPCLoot()
        {
            Main.projectile[(int)NPC.ai[3]].Kill();
        }
    }
}