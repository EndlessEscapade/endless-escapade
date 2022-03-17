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

            NPC.lifeMax = 8000;
            NPC.defense = 10;

            NPC.width = 96;
            NPC.height = 96;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;

            NPC.noGravity = true;
        }


        public Scrapwizard myWizard;

        public Rectangle myRoom;

        public int currentAttack;

        public bool fightBegun;

        public float teleportFloat;

        public int frameY;

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (target.Center.X < NPC.Center.X) NPC.direction = -1;
            else NPC.direction = 1;

            NPC.spriteDirection = NPC.direction;

            #region Determining gravity
            NPC.velocity.Y += 0.8f; //Force of gravity
            #endregion

            #region Initializing
            if (!fightBegun)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[1] = 2001 * 16;
                    NPC.ai[2] = 191 * 16;

                    myRoom = new Rectangle((int)NPC.ai[1] + (3 * 16), (int)NPC.ai[2] + (7 * 16), 112 * 16, 33 * 16);

                    NPC.Center = myRoom.Center.ToVector2() + new Vector2(128, 128);

                    myWizard = Main.npc[NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Scrapwizard>(), ai0: NPC.whoAmI, ai1: 0)].ModNPC as Scrapwizard;

                    NPC.ai[0]++; //Ticker
                }
            }     
            #endregion
            else 
            {
                switch (myWizard.currentAttack)
                {
                    case 0: //teleports the brute behind the player and then does a rapid slam
                            //less than a second to teleport

                        if (myWizard.NPC.ai[1] < 40)
                        {
                            if (teleportFloat < 1) teleportFloat += 1 / 40f;
                        }

                        else if (myWizard.NPC.ai[1] == 40)
                        {
                            int y = 0;
                            for(int j = 0; j < 50; j++)
                            {
                                if (Main.tile[(int)(target.Center.X / 16f), (int)(target.Center.Y / 16f) + j].HasTile &&
                                    Main.tileSolid[Main.tile[(int)(target.Center.X / 16f), (int)(target.Center.Y / 16f) + j].TileType])
                                {
                                    y = j;

                                    break;
                                }
                            }

                            NPC.Center = new Vector2(target.Center.X + (84 * (Main.rand.NextBool() ? -1 : 1)), (((int)(target.Center.Y / 16f) + y) * 16) - 48);
                        }

                        //less than a second to telegraph
                        else if (myWizard.NPC.ai[1] < 80)
                        {
                            if (teleportFloat > 0) teleportFloat -= 1 / 40f;
                        }

                        //SLAM
                        else if (myWizard.NPC.ai[1] < 120)
                        {
                            if (myWizard.NPC.ai[1] % 5 == 0 && frameY < 3) frameY++;
                        }

                        break;
                    case 1: //teleports the brute across the arena from the player, and the brute charges across the arena,
                            //dragging its sword, leaving behind fire. at the end of its charge, does an uppercut.
                            //less than a second to teleport

                        if (myWizard.NPC.ai[1] < 40)
                        {
                            if (teleportFloat < 1) teleportFloat += 1 / 40f;
                        }

                        else if (myWizard.NPC.ai[1] == 40)
                        {
                            int x = myRoom.Center.X + (50 * 16 * (Main.rand.NextBool() ? -1 : 1));

                            int y = 0;
                            for (int j = 0; j < 50; j++)
                            {
                                if (Main.tile[(int)(x / 16f), (int)(target.Center.Y / 16f) + j].HasTile &&
                                    Main.tileSolid[Main.tile[(int)(x / 16f), (int)(target.Center.Y / 16f) + j].TileType])
                                {
                                    y = j;

                                    break;
                                }
                            }

                            NPC.Center = new Vector2(x, (((int)(target.Center.Y / 16f) + y) * 16) - 48);

                            NPC.ai[2] = NPC.Center.X;

                            //warp
                        }

                        //less than a second to telegraph
                        else if (myWizard.NPC.ai[1] < 119)
                        {
                            if(teleportFloat > 0) teleportFloat -= 1 / 40f;

                            //throw potion at some point
                        }

                        //charge!
                        else
                        {
                            NPC.spriteDirection = NPC.velocity.X < 0 ? -1 : 1;

                            NPC.velocity.X = -8 * (myRoom.Center.X < NPC.ai[2] ? 1 : -1);

                            frameY = 3;

                            if(myWizard.NPC.ai[1] % 2 == 0)
                            {
                                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(NPC.Center.X, myRoom.Y + (30 * 16)), Vector2.Zero, ModContent.ProjectileType<ShadowEmberTemp>(), 0, 0);
                            }
                        }

                        break;
                    case 2: //Starts swinging his sword in a slashing motion and moving towards the player.
                        NPC.velocity.X = 2 * (target.Center.X < NPC.Center.X ? -1 : 1);

                        frameY = (int)(2 + (Math.Sin(myWizard.NPC.ai[1] / 20f) * 1.5f));
                        break;
                    case 3: //brute jumps up, scrapwizard throws a shadowflame potion down, and the brute slams into it, shooting flame across the whole arena.
                        if (myWizard.NPC.ai[1] % 80 < 40)
                        {
                            if (teleportFloat < 1) teleportFloat += 1 / 40f;
                        }

                        else if (myWizard.NPC.ai[1] % 80 == 40)
                        {
                            NPC.Center = new Vector2(target.Center.X, myRoom.Y);
                        }

                        else //check instead if brute is aboveground
                        {
                            if (NPC.velocity.Y > 0) //in midair
                            {
                                if (teleportFloat > 0) teleportFloat -= 1 / 20f;
                            }
                        }

                        break;
                    case 4: //brute rapidly slices up and down towards the player, then slams his sword into the ground and gets stuck.
                        NPC.velocity.X = 4 * (target.Center.X < NPC.Center.X ? -1 : 1);

                        if (myWizard.NPC.ai[1] < 120)
                        {
                            if (myWizard.NPC.ai[1] % 16 < 8) frameY = 2;
                            else frameY = 3;
                        }
                        else if (myWizard.NPC.ai[1] < 130)
                        {
                            if (frameY > 1 && myWizard.NPC.ai[1] % 5 == 0) frameY--;
                        }
                        else if (myWizard.NPC.ai[1] == 130)
                        {
                            NPC.velocity.Y -= 24f;
                        }
                        else
                        {
                            if (NPC.velocity.Y > 0)
                            {
                                if (frameY < 3 && myWizard.NPC.ai[1] % 5 == 0) frameY++;
                            }
                        }
                        break;
                    case 5: //throws up a shadowflame potion, brute breaks it on its sword, then shoots a temporary flame column at the player
                        if (myWizard.NPC.ai[1] < 40)
                        {
                            if (teleportFloat < 1) teleportFloat += 1 / 40f;
                        }

                        if (myWizard.NPC.ai[1] == 40)
                        {
                            NPC.Center = myRoom.Center.ToVector2();
                        }

                        else if (myWizard.NPC.ai[1] < 80)
                        {
                            if (teleportFloat > 0) teleportFloat -= 1 / 40f;
                        }

                        else if (myWizard.NPC.ai[1] < 80 + 360)
                        {
                            if ((myWizard.NPC.ai[1] - 80) % 120 == 0)
                            {
                                if (mingus)
                                {
                                    for (int i = 0; i < 9; i++)
                                    {
                                        float xVal = (-60 + (i / 10f) * (60 - -60));

                                        Vector2 pos = myRoom.Center.ToVector2() + new Vector2((xVal * 16) + (6 * 16) + (6 * 16), 13 * 16);

                                        Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), pos, Vector2.Zero, ModContent.ProjectileType<FlameColumn>(), 0, 0);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        float xVal = (-60 + (i / 10f) * (60 - -60));

                                        Vector2 pos = myRoom.Center.ToVector2() + new Vector2((xVal * 16) + (3 * 16), 13 * 16);

                                        Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), pos, Vector2.Zero, ModContent.ProjectileType<FlameColumn>(), 0, 0);
                                    }
                                }


                                mingus = !mingus;
                            }
                        }


                        break;
                }
            }
        }

        public bool mingus;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            myWizard.InitShader();

            Rectangle rect = new Rectangle(0, frameY * 144, 144, 144);
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/GuardBrute").Value;

            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, rect, Color.White, NPC.rotation, NPC.spriteDirection == 1 ? new Vector2(48, 96) : new Vector2(96, 96), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }

        public override void OnKill()
        {
            myWizard.Trigger();

            base.OnKill();
        }
    }
}