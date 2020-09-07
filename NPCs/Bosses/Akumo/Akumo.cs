using EEMod.Compatibility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Akumo
{
    public class Akumo : ModNPC
    {
        private int wingsPer = 6;

        private float wingspeed
        {
            get => wingsPer;
            set => wingsPer = (int)Math.Round(1 / (float)value * 10);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akumo");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.lavaImmune = true;
            npc.friendly = false;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.lifeMax = 50000;
            npc.defense = 40;
            npc.damage = 95;
            npc.value = Item.buyPrice(0, 8, 0, 0);
            npc.noTileCollide = true;
            npc.width = 250;
            npc.height = 230;
            drawOffsetY = 40;

            npc.npcSlots = 24f;
            npc.knockBackResist = 0f;

            musicPriority = MusicPriority.BossMedium;

            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }

            music = Compatibilities.EEMusic?.GetSoundSlot(SoundType.Music, "Sounds/Music/Precursors") ?? MusicID.Boss3;
        }

        private int frameUpdate;

        public override void FindFrame(int frameHeight)
        {
            frameUpdate++;
            if (frameUpdate == wingsPer)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                frameUpdate = 0;
            }
            if (npc.frame.Y == frameHeight * 2)
            {
                Main.PlaySound(SoundID.Item32);
            }
            if (npc.frame.Y >= frameHeight * 8)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 75000;
            npc.damage = 110;
            npc.defense = 55;
        }

        public override void NPCLoot()
        {
            EEWorld.EEWorld.downedAkumo = true;
            //EEMod.ServBoolUpdate();
        }

        private Vector2 addOn;

        private void switchPos(int mode)
        {
            switch (mode)
            {
                case 0:
                {
                    addOn = new Vector2(-400, -400);
                    break;
                }
                case 1:
                {
                    addOn = new Vector2(0, -400);
                    break;
                }
                case 2:
                {
                    addOn = new Vector2(400, -400);
                    break;
                }
            }
        }

        private bool isDashing = false;
        private bool isVortexing = false;
        private bool isCircling = false;
        //private bool isFeathering = false; // unused atm
        private readonly int nextAttackTime = 240;
        private readonly int spawnOffset = 1000;
        private int lengthOfAttack = 120;
        private readonly int endurance = 5;
        private readonly int lengthOfBreak = 600;
        private readonly int lengthOfAttack1 = 120;
        private readonly int lengthOfAttack2 = 340;
        private readonly int lengthOfAttack3 = 400;
        //	private readonly int lengthOfAttack3 = 340;

        public override void AI()
        {
            if (npc.ai[2] == 0 || npc.ai[2] == 2)
            {
                lengthOfAttack = lengthOfAttack1;
            }
            else if (npc.ai[2] == 3)
            {
                lengthOfAttack = lengthOfAttack3;
            }
            else
            {
                lengthOfAttack = lengthOfAttack2;
            }

            npc.TargetClosest(true);
            if (scree)
            {
                Scree();
            }

            Player player = Main.player[npc.target];
            if (npc.ai[0] == 0)
            {
                alpha = 1;
                npc.position = new Vector2(player.Center.X, player.Center.Y - spawnOffset);
            }
            if (npc.velocity.X > 0)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }

            isDashing = false;
            isVortexing = false;
            isCircling = false;
            npc.rotation = npc.velocity.X / 40f;
            DespawnHandler();
            npc.ai[0]++;
            if (npc.ai[0] >= nextAttackTime && npc.ai[0] <= nextAttackTime + lengthOfAttack + 1)
            {
                switch (npc.ai[2])
                {
                    case 0:
                        Dash();
                        break;

                    case 1:
                        Vortex();
                        break;

                    case 2:
                        Feathers();
                        break;

                    case 3:
                        Ascend();
                        break;
                }
            }

            if (!isDashing && !isVortexing && npc.ai[2] != -1 && !isCircling)
            {
                switchPos(1);
                Move(player, 8, 40);
                wingspeed = 2;
            }

            if (npc.ai[0] == nextAttackTime + lengthOfAttack + 2)
            {
                for (int i = 0; i < 20; i++)
                {
                    npc.ai[2] = Main.rand.Next(4);
                }

                frameUpdate = 0;
                alpha = 1;
                scale = 0;
                npc.ai[0] = 1;
                npc.ai[1] = 0;
                npc.ai[3]++;
                npc.netUpdate = true;
            }
            if (npc.ai[3] > endurance)
            {
                npc.ai[3]++;
                npc.ai[2] = -1;
            }
            if (npc.ai[3] == lengthOfBreak)
            {
                npc.ai[3] = 0;
                npc.ai[2] = Main.rand.Next(2);
                npc.ai[1] = 0;
                npc.ai[0] = 1;
                frameUpdate = 0;
                alpha = 1;
                scale = 0;
                npc.netUpdate = true;
            }
            if (npc.ai[2] == -1)
            {
                npc.velocity *= 0.994f;
                if (npc.ai[0] == nextAttackTime + lengthOfAttack + 2)
                {
                    frameUpdate = 0;
                    wingspeed = 0.5f;
                }
            }
        }

        private int akumoDirectionDescision;

        private void Dash()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            isDashing = true;
            if (npc.ai[0] == nextAttackTime)
            {
                akumoDirectionDescision = Main.rand.Next(3);
            }

            switchPos(akumoDirectionDescision);
            if (npc.ai[0] < nextAttackTime + 80)
            {
                Move(player, 30, 40);
            }

            if (npc.ai[0] == nextAttackTime + 40)
            {
                addOn *= 1.5f;
            }

            if (npc.ai[0] >= nextAttackTime + 80)
            {
                if (npc.ai[1] == 0)
                {
                    npc.velocity = new Vector2((1 - akumoDirectionDescision) * (npc.ai[0] - (nextAttackTime + 80)) / 2, (npc.ai[0] - (nextAttackTime + 80)) / 2);
                }

                if (npc.ai[1] == 1)
                {
                    npc.velocity *= 0.975f;
                    if (npc.ai[0] < nextAttackTime + 90)
                    {
                        addOn = Vector2.Zero;
                        Move(player, 110, 20);
                    }
                }
            }
            if (npc.ai[0] >= nextAttackTime + lengthOfAttack && npc.ai[1] == 0)
            {
                npc.ai[0] = nextAttackTime - 1;
                npc.ai[1]++;
            }
        }

        private void Feathers()
        {
            npc.velocity *= 0.98f;
            //isFeathering = true;
            Player player = Main.player[npc.target];
            npc.velocity.X = 0;
            npc.velocity.Y = 0;
            float Speed = 3f;
            Vector2 vector8 = npc.Center;
            int damage = 10;
            int type = ModContent.ProjectileType<AkumoFeather>();
            float DisX = Main.rand.NextFloat(-1000f, 1000f);
            float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
            if (npc.ai[0] % 5 == 0)
            {
                Projectile.NewProjectile(vector8.X + DisX, vector8.Y - 1200, (float)(Math.Cos(rotation) * Speed * -1), (float)(Math.Sin(rotation) * Speed * -1), type, damage, 0f, 0);
            }
        }

        private void Vortex()
        {
            npc.velocity *= 0.98f;
            isVortexing = true;
            if (npc.ai[0] == nextAttackTime)
            {
                frameUpdate = 0;
                wingspeed = 5;
            }
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 dist = new Vector2(npc.Center.X - player.Center.X, npc.Center.Y - player.Center.Y);
            int speedReduce = 480 - (int)npc.ai[0] + 800;
            dist /= speedReduce;
            int maxDistance = 1000;
            Rectangle rectangle1 = player.Hitbox;
            Rectangle rectangle2 = new Rectangle((int)npc.position.X - maxDistance, (int)npc.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);

            if (rectangle1.Intersects(rectangle2))
            {
                player.velocity += dist;
            }
            for (int i = 0; i < 20; i++)
            {
                double deg = (double)npc.ai[1] + (i * 72);
                double rad = deg * (Math.PI / 180);

                int num7 = Dust.NewDust(npc.Center, npc.width, npc.height, 63, 0f, 0f, 480 - (int)npc.ai[0], new Color(255, 255, 255, 255), (npc.ai[0] / 255) - 1);
                Main.dust[num7].position.X = npc.Center.X - (int)(Math.Cos(rad) * (i * 30 * (1 - npc.ai[0] % 30 / 30)));
                Main.dust[num7].position.Y = npc.Center.Y - (int)(Math.Sin(rad) * (i * 30 * (1 - npc.ai[0] % 30 / 30)));
                Main.dust[num7].noGravity = true;

                npc.ai[1] += 0.5f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num7].scale *= npc.ai[0] / 255;
                    }
                    npc.netUpdate = true;
                }
            }
        }

        private void Move(Player player, float sped, float TR)
        {
            Vector2 moveTo = player.Center + addOn;
            float speed = sped;
            Vector2 move = moveTo - npc.Center;
            float magnitude = move.Length(); // (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TR;

            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            npc.velocity = move;
        }

        private void Scree()
        {
            if (alpha <= 0)
            {
                alpha = 1;
            }

            alpha -= 0.08f;
            scale = 1.8f - (alpha * 1.8f);
        }

        private void Ascend()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (npc.ai[0] > nextAttackTime && npc.ai[0] <= nextAttackTime + 120)
            {
                for (int i = 0; i < 5; i++)
                {
                    int num7 = Dust.NewDust(npc.Center, 1, 1, DustID.Fire, 0f, 0f, 0, default, 4 - (npc.ai[0] - 240) / 90);
                    Main.dust[num7].position = npc.Center;
                    Main.dust[num7].noGravity = true;
                }
                isCircling = true;
                npc.velocity *= 0.98f;
                double deg = ((double)npc.ai[0] - nextAttackTime) * 5;
                double rad = deg * (Math.PI / 180);
                npc.velocity.X += (float)(Math.Cos(rad) * 1.4f);
                npc.velocity.Y += (float)(Math.Sin(rad) * 1.4f);
            }
            if (npc.ai[0] >= nextAttackTime + 120 && npc.ai[0] <= nextAttackTime + 200)
            {
                for (int i = 0; i < 100; i++)
                {
                    int num7 = Dust.NewDust(npc.Center, 1, 1, DustID.Fire, 0f, 0f, 0, default, 2);
                    Main.dust[num7].position.X = player.Center.X - 1000 + (i * 20);
                    Main.dust[num7].position.Y = player.Center.Y - 700;
                    Main.dust[num7].noGravity = false;
                }
                for (int i = 0; i < 2; i++)
                {
                    int num7 = Dust.NewDust(npc.Center, 1, 1, DustID.Fire, 0f, 0f, 0, default, (npc.ai[0] - 240) / 90);
                    Main.dust[num7].position = npc.Center;
                    Main.dust[num7].noGravity = true;
                }
                isCircling = true;
                npc.velocity.Y -= 1;
            }
            if (npc.ai[0] >= nextAttackTime + 200)
            {
                if (npc.ai[0] < nextAttackTime + 230)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        int num7 = Dust.NewDust(npc.Center, 1, 1, DustID.Fire, 0f, 0f, 0, default, 2);
                        Main.dust[num7].position.X = player.Center.X - 1000 + (i * 20);
                        Main.dust[num7].position.Y = player.Center.Y - 700;
                        Main.dust[num7].noGravity = false;
                    }
                }
                if (npc.ai[0] == nextAttackTime + 200 && npc.ai[0] <= nextAttackTime + 360)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Scree").WithVolume(.7f).WithPitchVariance(4));
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/BadScree").WithVolume(.8f).WithPitchVariance(3));
                }
                if (npc.ai[0] == nextAttackTime + 230)
                {
                    Scree();
                }
                addOn = new Vector2(player.velocity.X, 600);
                isCircling = true;
                Move(player, 80, 60);
            }
        }

        private void DespawnHandler()
        {
            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                npc.dontTakeDamage = true;
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                }
                return;
            }
        }

        private float alpha, scale;
        private bool scree => npc.ai[0] < 60 && npc.ai[0] > 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.Akumo;
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            //Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin();
            if (scree)
            {
                if (npc.ai[0] == 1)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Scree").WithVolume(.7f).WithPitchVariance(4));
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/BadScree").WithVolume(.8f).WithPitchVariance(1.5f));
                }
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, drawColor * alpha, npc.rotation, npc.frame.Size() / 2, scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            if (isDashing && npc.ai[0] >= nextAttackTime + 80)
            {
                AfterImage.DrawAfterimage(spriteBatch, texture, 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            if (isCircling)
            {
                AfterImage.DrawAfterimage(spriteBatch, texture, 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
                if (npc.ai[0] >= nextAttackTime + 200)
                {
                    AfterImage.DrawAfterimage(spriteBatch, texture, 0, npc, 2, 1f, 6, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
                }
            }
            return true;
        }
    }
}