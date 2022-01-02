using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.GoblinShaman
{
    public class GoblinShaman : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Shaman");
            //Main.npcFrameCount[npc.type] = 6;
        }

        /*public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 6)
            {
                npc.frame.Y = 0;
                return;
            }
        }*/

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

        public override void AI()
        {
            NPC.velocity.X *= 0.9f;

            //swing staff around in circle, plant it down, do aura thing
            //swing staff forward, summon three bolts three times, and fire at player

            NPC.spriteDirection = Main.LocalPlayer.Center.X < NPC.Center.X ? 1 : -1;

            staffCenter = NPC.Center + new Vector2(12 * NPC.spriteDirection, 14) + ((NPC.ai[0] % 180 >= 60 && NPC.ai[0] % 180 < 120) ? new Vector2(0, (float)Math.Sin(((NPC.ai[0] % 60) / 20f) * MathHelper.Pi * 2) * -3).RotatedBy(staffRot) : Vector2.Zero);
            
            if (NPC.ai[0] == 0)
            {
                StaffBolt = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), staffCenter + new Vector2(0, -22), Vector2.Zero, ModContent.ProjectileType<StaffBolt>(), 0, 0, ai0: 1f);

                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(StaffBolt, Color.Violet, 4));
            }

            if(NPC.ai[1] == 0) NPC.ai[0]++;

            if (NPC.ai[1] > 0)
            {
                NPC.ai[1]++;

                for (int i = 0; i < 3; i++)
                {
                    int dust = Dust.NewDust(NPC.Center, 0, 0, DustID.CrystalSerpent_Pink);
                    Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                    Main.dust[dust].noGravity = true;
                }
            }

            if (NPC.ai[1] != 60) StaffBolt.Center = staffCenter + new Vector2(0, -22).RotatedBy(staffRot);

            if (NPC.ai[0] == 540 && NPC.ai[1] == 0)
            {
                SoundEngine.PlaySound(SoundID.Zombie, NPC.Center, style: 61);

                NPC.ai[1] = 1;
            }

            if (NPC.ai[1] == 59)
            {
                StaffBolt.Kill();
                StaffBolt.active = false;
                StaffBolt = null;
            }

            if (NPC.ai[1] == 60)
            {
                Vector2 newPos = Vector2.Zero;

                while (newPos == Vector2.Zero)
                {
                    int tileTry = (int)(Main.LocalPlayer.Center.X / 16f) + Main.rand.Next(30, 50) * (Main.rand.NextBool() ? 1 : -1);

                    for (int i = (int)(Main.LocalPlayer.Center.Y / 16f) - 50; i < (int)(Main.LocalPlayer.Center.Y / 16f) + 50; i++)
                    {
                        if ((Main.tile[tileTry, i].IsActive && Main.tileSolid[Main.tile[tileTry, i].type]) && 
                            (!Main.tile[tileTry, i - 1].IsActive || !Main.tileSolid[Main.tile[tileTry, i - 1].type]) && 
                            (!Main.tile[tileTry, i - 2].IsActive || !Main.tileSolid[Main.tile[tileTry, i - 2].type]) && 
                            (!Main.tile[tileTry, i - 3].IsActive || !Main.tileSolid[Main.tile[tileTry, i - 3].type]))
                        {
                            newPos = new Vector2((tileTry * 16) + 8, (i * 16) - 28);
                        }
                    }
                }

                NPC.Center = newPos;

                NPC.spriteDirection = Main.LocalPlayer.Center.X < NPC.Center.X ? -1 : 1;
                staffCenter = NPC.Center + new Vector2(-6 * NPC.spriteDirection, 6);

                StaffBolt = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), staffCenter + new Vector2(1, -22), Vector2.Zero, ModContent.ProjectileType<StaffBolt>(), 0, 0, ai0: 1f);

                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(StaffBolt, Color.Violet, 4));
            }

            if (NPC.ai[1] >= 120)
            {
                for (int i = 0; i < 20; i++)
                {
                    int dust = Dust.NewDust(NPC.Center, 0, 0, DustID.CrystalSerpent_Pink);
                    Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                    Main.dust[dust].noGravity = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.Center, 0, 0, DustID.CrystalSerpent_Pink);
                    Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6));
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = 2f;
                }

                NPC.ai[1] = 0;
                NPC.ai[0] = 1;
            }

            if (NPC.ai[1] == 0)
            {
                if (NPC.ai[0] % 180 >= 60 && NPC.ai[0] % 180 < 120)
                {
                    staffRot = Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center).ToRotation() + ((float)Math.Cos((((NPC.ai[0] % 180) / 60f) * Math.PI)) * 0.75f) + MathHelper.PiOver2;

                    if (NPC.ai[0] % 20 == 10) ShootBolt(staffCenter + new Vector2(0, -22).RotatedBy(staffRot), Vector2.Normalize(new Vector2(0, -22).RotatedBy(staffRot)));
                }
                else if (NPC.ai[0] % 180 >= 120)
                {
                    staffRot -= (NPC.spriteDirection == 1 ? (staffRot / 10f) : ((staffRot - 6.28f) / 10f));
                }
                else
                {
                    staffRot += (((Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center).ToRotation() + 0.75f) - staffRot) / 10f);
                }
            }
        }

        public void ShootBolt(Vector2 pos, Vector2 addVel)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(pos, 0, 0, DustID.CrystalSerpent_Pink);
                Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                Main.dust[dust].noGravity = true;
            }

            NPC.velocity += (((Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center) + addVel) * 3).X < 0 ? new Vector2(0.5f, 0) : new Vector2(-0.5f, 0));

            Projectile newBolt = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), pos, ((Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center) + addVel) * 3), ModContent.ProjectileType<ShadowflameHexBolt>(), 20, 2, Main.myPlayer);

            PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(newBolt, Color.Violet, 16));

            SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
        }

        Projectile StaffBolt;
        float staffRot;
        Vector2 staffCenter;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Color defaultDrawColor = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));

            EEMod.ShadowWarp.Parameters["noise"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/Noise/noise").Value);
            EEMod.ShadowWarp.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);
            EEMod.ShadowWarp.Parameters["lerpVal"].SetValue((float)Math.Cos((NPC.ai[1] / 120f) * MathHelper.TwoPi).PositiveSin());
            EEMod.ShadowWarp.Parameters["baseColor"].SetValue(new Vector4(defaultDrawColor.R, defaultDrawColor.G, defaultDrawColor.B, defaultDrawColor.A) / 255f);

            EEMod.ShadowWarp.CurrentTechnique.Passes[0].Apply();

            Texture2D ShamanGlow = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/GoblinShaman/GoblinShaman").Value;

            Main.spriteBatch.Draw(ShamanGlow, NPC.Center - Main.screenPosition + new Vector2(0, 4), null, defaultDrawColor, NPC.rotation, ShamanGlow.Bounds.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End(); Main.spriteBatch.Begin();

            if (StaffBolt != null) StaffBolt.ai[0] = (float)Math.Cos((NPC.ai[1] / 120f) * MathHelper.TwoPi).PositiveSin();

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Color defaultDrawColor = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));

            EEMod.ShadowWarp.Parameters["noise"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/Noise/noise").Value);
            EEMod.ShadowWarp.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);
            EEMod.ShadowWarp.Parameters["lerpVal"].SetValue((float)Math.Cos((NPC.ai[1] / 120f) * MathHelper.TwoPi).PositiveSin());
            EEMod.ShadowWarp.Parameters["baseColor"].SetValue(new Vector4(defaultDrawColor.R, defaultDrawColor.G, defaultDrawColor.B, defaultDrawColor.A) / 255f);

            EEMod.ShadowWarp.CurrentTechnique.Passes[0].Apply();



            //staff drawing
            Texture2D ShamanStaff = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/GoblinShaman/ShamanStaff").Value;
            //Texture2D StaffGlow = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/GoblinShaman/StaffGlow").Value;

            Main.spriteBatch.Draw(ShamanStaff, staffCenter - Main.screenPosition, null, defaultDrawColor, staffRot, (ShamanStaff.Bounds.Size() / 2f) + new Vector2(1, 4), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            //Main.spriteBatch.Draw(StaffGlow, NPC.Center - Main.screenPosition + new Vector2(-6 * NPC.spriteDirection, 0), null, Color.White, staffRot, StaffGlow.Bounds.Size() / 2f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);


            //shaman's hand
            //Texture2D ShamanHand = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/GoblinShaman/ShamanHand").Value;

            //Main.spriteBatch.Draw(ShamanHand, NPC.Center - Main.screenPosition + new Vector2(11 * NPC.spriteDirection, 4 + 11), null, defaultDrawColor, staffRot, new Vector2(11, 39), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }

        public override void OnKill()
        {
            if (StaffBolt != null)
            {
                StaffBolt.Kill();
                StaffBolt.active = false;
                StaffBolt = null;
            }
        }
    }

    public class ShadowflameHexBolt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Bolt");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.alpha = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.LocalPlayer.Center - Projectile.Center) * 6, 0.02f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, 0, 0, DustID.CrystalSerpent_Pink);
                Main.dust[dust].velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Texture2D mask = EEMod.Instance.Assets.Request<Texture2D>("Textures/Extra_49").Value;
            Texture2D bolt = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/GoblinShaman/ShadowflameHexBolt").Value;

            Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition, Color.Violet, 0.75f, 0f);

            //Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition, Color.DarkViolet, 0.15f, 0f);

            //Main.spriteBatch.Draw(bolt, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(6, 6), Projectile.scale, SpriteEffects.None, 0);

            //lightColor = Color.White;

            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.Kill();
            return false;
        }
    }

    public class StaffBolt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.alpha = 0;
            Projectile.timeLeft = 10000000;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D bolt = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/GoblinShaman/StaffBolt").Value;

            Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition, Color.Violet * Projectile.ai[0], 1f, 0f);

            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}