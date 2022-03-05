using EEMod.Extensions;
using EEMod.NPCs.Goblins.Bard;
using EEMod.NPCs.Goblins.Berserker;
using EEMod.NPCs.Goblins.Shaman;
using EEMod.Prim;
using EEMod.Tiles.Furniture.Chests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins
{
    public class GoblinGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            int goblinsCount = 0;

            for (int index = 0; index < Main.maxNPCs; index++)
            {
                if (((Main.npc[index].type == ModContent.NPCType<GoblinShaman>() ||
                      Main.npc[index].type == ModContent.NPCType<CymbalBard>() ||
                      Main.npc[index].type == ModContent.NPCType<PanfluteBard>() ||
                      Main.npc[index].type == ModContent.NPCType<PercussionBard>() ||
                      Main.npc[index].type == ModContent.NPCType<GoblinBerserker>()
                    ) && Main.npc[index].active))
                {
                    goblinsCount++;
                }
            }

            if (npc.type == ModContent.NPCType<GoblinShaman>() ||
               npc.type == ModContent.NPCType<CymbalBard>() ||
               npc.type == ModContent.NPCType<PanfluteBard>() ||
               npc.type == ModContent.NPCType<PercussionBard>() ||
               npc.type == ModContent.NPCType<GoblinBerserker>())
            {
                int proj = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(npc), npc.Center, Vector2.Zero, ModContent.ProjectileType<GoblinDeathBolt>(), 0, 0);

                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[proj], Color.Violet, 6));

                Vector2 tilePos = Vector2.Zero;

                Main.NewText(goblinsCount);

                if (goblinsCount <= 1)
                {
                    Main.projectile[proj].ai[1] = 1;
                }

                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for(int j = 0; j < Main.maxTilesY; j++)
                    {
                        if(Framing.GetTileSafely(i, j).TileType == ModContent.TileType<ShadowflameHexChestTile>())
                        {
                            tilePos = new Vector2((i * 16) + 16, (j * 16) + 16);

                            (Main.projectile[proj].ModProjectile as GoblinDeathBolt).Target = tilePos;

                            return;
                        }
                    }
                }
            }
        }
    }

    public class GoblinDeathBolt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.alpha = 0;

            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 10000;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public int specialTicker;
        public Vector2 storedScreenPos;
        public override void AI()
        {
            if(Projectile.ai[1] == 0) Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Target - Projectile.Center) * 10, 0.02f);
            else Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Target - Projectile.Center) * 10/* + new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f) * 2f, 0).RotatedBy(Vector2.Normalize(Target - Projectile.Center).ToRotation() + 1.57f)*/, 0.02f);


            if (Projectile.ai[1] == 1 || Projectile.ai[1] == 2)
            {
                Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 4f, false, true, 0);
            }

            if(Projectile.ai[1] == 2)
            {
                specialTicker--;

                if(specialTicker <= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, Projectile.Center);

                    for (int i = 0; i < 20; i++)
                    {
                        int dust = Dust.NewDust(Target + new Vector2(0, -36), 0, 0, DustID.CrystalSerpent_Pink);

                        Main.dust[dust].velocity = new Vector2(0, 4f).RotatedByRandom(6.28f);
                        //Main.dust[dust].noGravity = true;
                    }

                    Projectile.ai[1] = 3;
                    specialTicker = 60;
                }
            }

            if (Projectile.ai[1] == 3)
            {
                specialTicker--;

                Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Vector2.Lerp(Main.LocalPlayer.Center, Projectile.Center, specialTicker / 60f), 4f, false, true, 0);

                if(specialTicker == 0)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();

                    Projectile.Kill();
                }
            }

            //Main.NewText(Projectile.ai[1]);

            if (Vector2.DistanceSquared(Target, Projectile.Center) <= 8 * 8)
            {
                Projectile.velocity = Vector2.Zero;

                if (Projectile.ai[1] == 0)
                {
                    Projectile.Kill();
                }
                else if(Projectile.ai[1] == 1)
                {
                    Projectile.Center = Target;

                    Projectile.ai[1] = 2;

                    specialTicker = 180;

                    for (int i = 0; i < 10; i++)
                    {
                        int dust = Dust.NewDust(Projectile.position, 0, 0, DustID.CrystalSerpent_Pink);
                        Main.dust[dust].velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));
                    }
                }
            }
        }

        public Vector2 Target;

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
            //Texture2D mask = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Extra_49").Value;
            Texture2D bolt = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Goblins/Shaman/ShadowflameHexBolt").Value;

            if(Projectile.ai[1] < 2) Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition - Projectile.velocity, Color.Violet, 0.5f, 0f);

            if(Projectile.ai[1] == 1)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/Chests/ShadowflameHexChestLock").Value,
                    new Vector2(Target.X, Target.Y - 36) - Main.screenPosition, null, Color.White, 0f, new Vector2(10, 12), 1f, SpriteEffects.None, 0f);
            }

            if (Projectile.ai[1] == 2)
            {
                /*Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.4f * ((180 - specialTicker) / 180f), (Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f));

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.4f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 1f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.5f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 2f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.25f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 2.5f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.5f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 2.75f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.4f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 3.5f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.55f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 4f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.35f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 4.25f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.5f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 5.25f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet, 2f * 0.45f * ((180 - specialTicker) / 180f), ((Main.GameUpdateCount / 50f) * ((180 - specialTicker) / 45f)) + 5.75f);*/

                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/Chests/ShadowflameHexChestLock").Value,
                    new Vector2(Target.X, Target.Y - 36) - Main.screenPosition +
                    new Vector2(Main.rand.Next((int)(-(180 - specialTicker) / 45f), (int)((180 - specialTicker) / 45f) + 1), Main.rand.Next((int)(-(180 - specialTicker) / 45f), (int)((180 - specialTicker) / 45f) + 1)),
                    null, Color.White, 0f, new Vector2(10, 12), 1f, SpriteEffects.None, 0f);
            }

            /*if (Projectile.ai[1] == 3)
            {
                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.4f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f));

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.4f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 1f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.5f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 2f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.25f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 2.5f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.5f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 2.75f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.4f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 3.5f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.55f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 4f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.35f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 4.25f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.5f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 5.25f);

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSlit").Value, Target - Main.screenPosition + new Vector2(0, -36), Color.Violet * (specialTicker / 60f), 2f * 0.45f * (specialTicker / 60f), (Main.GameUpdateCount * 4f / 60f) + 5.75f);
            }*/


            //Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition, Color.DarkViolet, 0.15f, 0f);

            //Main.spriteBatch.Draw(bolt, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(6, 6), Projectile.scale, SpriteEffects.None, 0);

            //lightColor = Color.White;

            if (Projectile.ai[1] == 2)
            {

            }

            return false;
        }
    }
}