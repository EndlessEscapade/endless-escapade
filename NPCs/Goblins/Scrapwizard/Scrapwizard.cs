using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.GoblinFort;
using System.Collections.Generic;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class Scrapwizard : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Scrapwizard");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 3300;
            NPC.defense = 10;

            NPC.width = 32;
            NPC.height = 32;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;

            NPC.noGravity = true;
        }

        public GuardBrute myGuard;

        public Rectangle myRoom;

        public int attackDelay;

        public int currentAttack;

        public bool bruteDead;

        public bool fightBegun;

        public bool mountedOnBrute;

        public float initialPosX;

        public List<Projectile> chandeliers;

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (target.Center.X < NPC.Center.X) NPC.direction = -1;
            else NPC.direction = 1;

            NPC.spriteDirection = NPC.direction;

            if (!bruteDead) //Phase 1
            {
                NPC.dontTakeDamage = true;

                #region Determining gravity
                if (mountedOnBrute)
                {
                    NPC.Center = myGuard.NPC.Center + new Vector2(-40 * myGuard.NPC.spriteDirection, -32);
                    NPC.rotation = 0f;
                }
                else
                {
                    NPC.velocity.Y += 0.48f; //Force of gravity
                }
                #endregion
                #region Starting the fight
                if (!fightBegun)
                {
                    #region Initializing
                    if (NPC.ai[1] == 0)
                    {
                        myGuard = Main.npc[(int)NPC.ai[0]].ModNPC as GuardBrute;

                        myRoom = myGuard.myRoom;

                        NPC.Center = new Vector2(myRoom.X + (56 * 16), myRoom.Y + (20 * 16));

                        NPC.ai[1]++; //Ticker
                    }
                    #endregion

                    if (NPC.ai[1] > 1)
                    {
                        NPC.ai[1]++;

                        NPC.rotation += NPC.velocity.X / 4f;
                    }

                    if (Vector2.DistanceSquared(target.Center, NPC.Center) <= 320 * 320 && NPC.ai[1] == 1)
                    {
                        SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblinlaugh2"));

                        NPC.velocity.X = (myGuard.NPC.Center.X - (NPC.Center.X - (-40 * myGuard.NPC.direction))) / 60f;

                        NPC.velocity.Y = -15f;
                        NPC.ai[1]++;
                    }

                    if(NPC.ai[1] >= 60)
                    {
                        NPC.ai[1] = 0;
                        fightBegun = true;
                        mountedOnBrute = true;
                        NPC.velocity = Vector2.Zero;
                        currentAttack = 0;
                        myGuard.fightBegun = true;

                        chandeliers = new List<Projectile>();

                        for(int i = 0; i < Main.maxProjectiles; i++)
                        {
                            if(Main.projectile[i].type == ModContent.ProjectileType<GoblinChandelierLight>())
                            {
                                chandeliers.Add(Main.projectile[i]);
                            }
                        }

                        SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingrr"));
                    }
                }
                #endregion

                else
                {
                    NPC.ai[1]++;

                    switch (currentAttack)
                    {
                        case 0: //teleports the brute behind the player and then does a rapid slam
                            //less than a second to teleport

                            if(NPC.ai[1] < 40)
                            {

                            }

                            else if(NPC.ai[1] == 40)
                            {

                            }

                            //less than a second to telegraph
                            else if (NPC.ai[1] < 80)
                            {

                            }

                            else if (NPC.ai[1] == 80)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingrunt2"));
                            }

                            //SLAM
                            else if (NPC.ai[1] < 120)
                            {

                            }

                            //next attack
                            else
                            {
                                currentAttack = Main.rand.Next(6);
                                NPC.ai[1] = 0;

                                myGuard.frameY = 0;
                            }
                            break;
                        case 1: //teleports the brute across the arena from the player, and the brute charges across the arena,
                                //dragging its sword, leaving behind fire. at the end of its charge, does an uppercut.
                            //less than a second to teleport
                            if (NPC.ai[1] < 40)
                            {

                            }

                            else if (NPC.ai[1] == 40)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingrunt1"));
                            }

                            //less than a second to telegraph
                            else if (NPC.ai[1] < 122)
                            {
                                //throw potion at some point
                            }

                            else if (NPC.ai[1] == 122)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingo"));
                            }

                            //charge!
                            else
                            {
                                if ((myGuard.NPC.velocity.X < 0 && myGuard.NPC.Center.X < myRoom.Center.X - Math.Abs(myRoom.Center.X - myGuard.NPC.ai[2])) ||
                                    (myGuard.NPC.velocity.X > 0 && myGuard.NPC.Center.X > myRoom.Center.X + Math.Abs(myRoom.Center.X - myGuard.NPC.ai[2])))
                                {
                                    myGuard.NPC.velocity = Vector2.Zero;
                                    myGuard.frameY = 0;

                                    currentAttack = Main.rand.Next(6);
                                    NPC.ai[1] = 0;
                                }
                            }

                            break;
                        case 2: //Starts swinging his sword in a slashing motion while scrapwizard throws concoctions and moving towards the player.
                            //starts throwing while brute starts swinging
                            if(NPC.ai[1] % 30 == 0)
                            {
                                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), NPC.Center, (Vector2.Normalize(target.Center - NPC.Center) * 6f) + myGuard.NPC.velocity, ModContent.ProjectileType<ShadowflameJar>(), 0, 0);
                            }

                            if(NPC.ai[1] > 180)
                            {
                                myGuard.NPC.velocity = Vector2.Zero;
                                myGuard.frameY = 0;

                                currentAttack = Main.rand.Next(6);
                                NPC.ai[1] = 0;
                            }

                            //stop and next attack
                            break;
                        case 3: //teleports brute upwards, throws potion down, scatters flames, repeat 3 times.
                            //less than a second to teleport
                            if (NPC.ai[1] % 80 < 40)
                            {

                            }

                            else if (NPC.ai[1] % 80 == 40)
                            {
                                //warp
                            }

                            else
                            {
                                if(myGuard.NPC.velocity.Y <= 0 && NPC.ai[1] >= 80)
                                {
                                    myGuard.NPC.velocity = Vector2.Zero;

                                    currentAttack = Main.rand.Next(6);
                                    NPC.ai[1] = 0;
                                }
                            }

                            //throws potion down and explodes into flames

                            //slams down and reteleports, repeat twice more

                            //next attack
                            break;
                        case 4: //brute rapidly slices up and down towards the player, then slams his sword into the ground and gets stuck.
                            if(NPC.ai[1] > 200)
                            {
                                myGuard.NPC.velocity = Vector2.Zero;

                                currentAttack = Main.rand.Next(6);
                                NPC.ai[1] = 0;

                                myGuard.frameY = 0;
                            }
                            break;
                        case 5: //throws up a shadowflame potion, brute breaks it on its sword, then shoots a temporary flame column at the player
                            if (NPC.ai[1] > 80 + 360)
                            {
                                myGuard.NPC.velocity = Vector2.Zero;

                                currentAttack = Main.rand.Next(6);
                                NPC.ai[1] = 0;

                                myGuard.frameY = 0;
                            }
                            break;
                    }
                }
            }
            else //Phase 2
            {
                #region Initializing
                if (!fightBegun)
                {
                    NPC.velocity.Y += 0.48f;

                    if (NPC.ai[1] < 40)
                    {
                        if (teleportFloat < 1) teleportFloat += 1 / 40f;
                    }
                    else if (NPC.ai[1] == 40)
                    {
                        NPC.Center = myRoom.Center.ToVector2() + new Vector2(0, 13 * 16);
                    }
                    else if (NPC.ai[1] < 80)
                    {
                        if (teleportFloat > 0) teleportFloat -= 1 / 40f;
                    }
                    else if (NPC.ai[1] < 100) { }
                    else if (NPC.ai[1] == 100)
                    {
                        //TODO
                        //Jump up to chandeliers instead of just starting the fight

                        float dist = 100000000;
                        Projectile myProj = null;
                        for(int i = 0; i < chandeliers.Count; i++)
                        {
                            if(Vector2.Distance(NPC.Center, chandeliers[i].Center) < dist)
                            {
                                dist = Vector2.Distance(NPC.Center, chandeliers[i].Center);
                                myProj = chandeliers[i];
                            }
                        }

                        NPC.ai[2] = myProj.whoAmI;

                        NPC.velocity = new Vector2((myProj.Center.X - NPC.Center.X) / 60f, -22.2f);

                        SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblincry1"));
                    }
                    else if (NPC.ai[1] < 160)
                    {
                        NPC.rotation -= 0.25f;
                    }
                    else if (NPC.ai[1] >= 160)
                    {
                        fightBegun = true;
                        NPC.ai[1] = 0;
                        NPC.ai[3] = 0;
                        currentAttack = 0;

                        NPC.velocity.Y = 0;
                        NPC.velocity.X = 0;

                        NPC.rotation = 0f;
                    }

                    NPC.ai[1]++;
                }
                #endregion
                else
                {
                    NPC.dontTakeDamage = false;

                    NPC.knockBackResist = 0f;

                    //NPC.velocity.Y += 0.48f; //Force of gravity

                    switch (currentAttack)
                    {
                        case 0: //turns all the chandelier flames but the one he's on into shadowflame, telegraph beams
                            break;
                        case 1: //animates a set of armor to briefly attack
                            break;
                        case 2: //throws down a few shadowflame potions that bounces twice and then explodes
                            break;
                        case 3: //casts a magic spell with a twirl of the wand and shoots bursts of three magic bolts
                            break;
                        case 4: //combines the chandelier flames into one big fireball each and casts them down with a meteor fashion towards the player
                            break;
                        case 5: //lengthens a chandelier and leans down to throw more shadowflame molotovs at the player
                            break;
                    }

                    if (NPC.ai[3] % 310 == 0)
                    {
                        List<Projectile> potentialChandeliers = new List<Projectile>();

                        foreach (Projectile chandelier in chandeliers)
                        {
                            if (Vector2.Distance(chandelier.Center, NPC.Center) < 20 * 16 && chandelier != Main.projectile[(int)NPC.ai[2]])
                            {
                                potentialChandeliers.Add(chandelier);
                            }
                        }

                        potChandelier = potentialChandeliers[Main.rand.Next(0, potentialChandeliers.Count)];

                        if(potChandelier.Center.X < Main.projectile[(int)NPC.ai[2]].Center.X)
                        {
                            nextToTheLeft = true;
                        }
                        else
                        {
                            nextToTheLeft = false;
                        }

                        if (NPC.velocity.X < 0)
                        {
                            NPC.ai[3] = 0;
                        }
                        else
                        {
                            NPC.ai[3] = 45;
                        }
                    }
                    else if (NPC.ai[3] % 310 < 180) //Actively swinging on a chandelier
                    {
                        GoblinChandelierLight skrunkle = (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight);

                        skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                        NPC.Center = skrunkle.anchorPos16 + 
                            (Vector2.UnitY * skrunkle.chainLength / 1.5f)
                            .RotatedBy(skrunkle.axisRotation);

                        NPC.rotation = skrunkle.axisRotation;

                        //HEY GOOD AFTERNOON :)
                        //make swing time dependent on a random bool and being ready to swing to another chandelier instead of being on a preset timer - but have a cap
                    }
                    else if (NPC.ai[3] % 310 < 270) //Picking the next chandelier to swing to, and starting the jump
                    {
                        if ((nextToTheLeft && NPC.ai[3] == 180) || (!nextToTheLeft && NPC.ai[3] == 225))
                        {
                            //Ready to swing
                            NPC.ai[2] = potChandelier.whoAmI;

                            NPC.ai[3] = 270;

                            NPC.velocity.X = (Main.projectile[(int)NPC.ai[2]].Center.X - NPC.Center.X) / 40f;
                            NPC.velocity.Y = -2f;
                        }
                        else
                        {
                            GoblinChandelierLight skrunkle = (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight);

                            skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                            NPC.Center = skrunkle.anchorPos16 +
                                (Vector2.UnitY * MathHelper.Clamp(Vector2.Distance(skrunkle.anchorPos16, NPC.Center), 0f, skrunkle.chainLength / 1.5f))
                                .RotatedBy(skrunkle.axisRotation);

                            NPC.rotation = skrunkle.axisRotation;
                        }
                    }
                    else if (NPC.ai[3] % 310 < 310) //In midair
                    {
                        NPC.velocity.Y += 2 / 20f;
                        NPC.rotation += NPC.velocity.X / 10f;
                        NPC.spriteDirection = (NPC.velocity.X < 0 ? -1 : 1);
                    }

                    foreach (Projectile chandelier in chandeliers)
                    {
                        if (chandelier != Main.projectile[(int)NPC.ai[2]])
                        {
                            (chandelier.ModProjectile as GoblinChandelierLight).axisRotation += ((0f - (chandelier.ModProjectile as GoblinChandelierLight).axisRotation) / 10f);
                        }
                    }

                    NPC.ai[3]++;
                    NPC.ai[1]++;
                }
            }
        }

        public float teleportFloat;

        public bool threeSwing;

        public Projectile potChandelier;

        public float swingDir;

        public bool nextToTheLeft;
        
        public void Trigger()
        {
            NPC.ai[1] = 0;
            bruteDead = true;
            fightBegun = false;
        }

        public void InitShader()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(Main.graphics.GraphicsDevice.Viewport.Width / 2, Main.graphics.GraphicsDevice.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            EEMod.ShadowWarp.Parameters["noise"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/Noise/noise").Value);
            EEMod.ShadowWarp.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);
            EEMod.ShadowWarp.Parameters["lerpVal"].SetValue(1 - MathHelper.Clamp((bruteDead ? teleportFloat : myGuard.teleportFloat), 0f, 1f));
            EEMod.ShadowWarp.Parameters["baseColor"].SetValue(new Vector4(Color.White.R, Color.White.G, Color.White.B, Color.White.A) / 255f);

            EEMod.ShadowWarp.CurrentTechnique.Passes[0].Apply();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            InitShader();

            Rectangle rect = new Rectangle(0, 0, 32, 32);

            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/Scrapwizard").Value;

            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, rect, Color.White, NPC.rotation, new Vector2(16, 16), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            
            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            InitShader();

            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardStaff").Value;

            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition + new Vector2((NPC.spriteDirection == 1 ? -6 : 6), 0), null, Color.White, NPC.rotation, tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            //spriteBatch.Draw(tex, new Rectangle((int)myRoom.X - (int)Main.screenPosition.X, (int)myRoom.Y - (int)Main.screenPosition.Y, myRoom.Width, myRoom.Height), null, Color.White, 0f, Vector2.Zero, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);


            Texture2D tex2 = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardArm").Value;

            spriteBatch.Draw(tex2, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, tex2.Size() / 2f, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

        }
    }
}