using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.IntWorld;
using EEMod.NPCs.Bosses.Archon;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using EEMod.NPCs.Bosses.Akumo;
using System.Threading;
using Terraria.World.Generation;
using Terraria.Utilities;
using Terraria.IO;
using Terraria.Localization;
using System.Collections.Generic;
using EEMod.Projectiles;

namespace EEMod
{
    public class EEPlayer : ModPlayer
    {
        public bool FlameSpirit;
        public bool magmaRune;
        public bool dalantiniumHood;
        public bool hydriteVisage;
        public bool ZoneCoralReefs;
		private int opac;
        public override void UpdateBiomes()
		{
			ZoneCoralReefs = EEWorld.CoralReefsTiles > 200;
			if(ZoneCoralReefs)
			{
				opac++;
				if (opac > 100)
					opac = 100;
				Filters.Scene.Activate("InteritosMod:CR").GetShader().UseOpacity(opac);
			}
			else
			{
				opac--;
				if (opac < 0)
					opac = 0;
				Filters.Scene.Deactivate("InteritosMod:CR");
			}
		}

        public override bool CustomBiomesMatch(Player other)
        {
            EEPlayer modOther = other.GetModPlayer<EEPlayer>();
            return ZoneCoralReefs == modOther.ZoneCoralReefs;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            EEPlayer modOther = other.GetModPlayer<EEPlayer>();
            modOther.ZoneCoralReefs = ZoneCoralReefs;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneCoralReefs;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneCoralReefs = flags[0];
        }

        public bool radiationResist = false;
        public bool alchemistFire = false;
        public bool alchemistElectric = false;
        public bool alchemistNuclear = false;
        public static bool godMode = false;
        public bool quartzCrystal = false;
        public bool isQuartzRangedOn = false;
        public bool isQuartzSummonOn = false;
        public bool isQuartzMeleeOn = false;
        public bool isQuartzChestOn = false;
        public int timerForCutscene;
        public static string key1 = "Pyramid";
        public static string key2 = "Sea";
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public bool noU;
        public static bool triggerSeaCutscene;
        public static int cutSceneTriggerTimer;
        public static int cutSceneTriggerTimer2;
        public static float cutSceneTriggerTimer3;
        public static int markerPlacer;
        public Vector2 position;
        public Vector2 velocity;
        public static List<Vector2> objectPos = new List<Vector2>();
        public static bool isNearIsland;
        public static bool isNearVolcano;
        public override void Initialize()
        {
            isSaving = false;
            godMode = false;
            timerForCutscene = 0;
            markerPlacer = 0;
            arrowFlag = false;
            noU = false;
            triggerSeaCutscene = false;
            cutSceneTriggerTimer = 0;
            cutSceneTriggerTimer2 = 1000;
            position = player.Center;
        }

        public override void ResetEffects()
        {
            //Minions

            //Pets

            //Acessories, Weapons, Armors, etc
            isQuartzChestOn = false;
            isQuartzRangedOn = false;
            isQuartzMeleeOn = false;
            isQuartzSummonOn = false;
            radiationResist = false;
            alchemistFire = false;
            alchemistElectric = false;
            alchemistNuclear = false;
            ResetMinionEffect();
            isSaving = false;
        }
        int Arrow;
        int Arrow2;
        float positionOfGlacX;
        float positionOfGlacY;

        public override void ModifyScreenPosition()
        {
            base.ModifyScreenPosition();
            if (Main.ActiveWorldFileData.Name == key2)
            {
                Main.screenPosition += new Vector2(0, 1000);
                //Main.GameZoomTarget += markerPlacer/1000;
            }

            if (triggerSeaCutscene && cutSceneTriggerTimer <= 1000)
            {
                Main.screenPosition.X -= cutSceneTriggerTimer;
            }
            if (cutSceneTriggerTimer >= 1000)
            {
                Main.screenPosition.X -= cutSceneTriggerTimer2;
            }
        }
        public override void UpdateBiomeVisuals()
        {
            player.ManageSpecialBiomeVisuals("InteritosMod:Akumo", NPC.AnyNPCs(ModContent.NPCType<Akumo>()));
            if (triggerSeaCutscene && cutSceneTriggerTimer <= 1000)
            {
                cutSceneTriggerTimer += 6;
                player.position = player.oldPosition;
            }
            if (cutSceneTriggerTimer >= 1000)
            {
                cutSceneTriggerTimer += 2;
            }
            if (godMode)
            {
                timerForCutscene += 20;
            }
            string key = "Prophecy:Ripple";
            string key4 = "Prophecy:SunThroughWalls";
            string key3 = "Prophecy:SeaTrans";
            if (Main.ActiveWorldFileData.Name == key1)
            {
                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;

                if (titleText <= 0)
                    titleText = 0;

                titleText2 = 1;
                if (!arrowFlag)
                {
                    Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("DesArrow"), 0, 0, player.whoAmI);
                    arrowFlag = true;
                }
                if (player.Center.X / 16 >= Main.spawnTileX - 5 &&
                    player.Center.X / 16 <= Main.spawnTileX + 5 &&
                    player.Center.Y / 16 >= Main.spawnTileY - 5 &&
                    player.Center.Y / 16 <= Main.spawnTileY + 5)
                {
                    (Main.projectile[Arrow].modProjectile as DesertArrowProjectile).visible = true;
                }
                else
                {
                    (Main.projectile[Arrow].modProjectile as DesertArrowProjectile).visible = false;
                }
                Filters.Scene.Deactivate(key4);
            }
            else if (Main.ActiveWorldFileData.Name == key2)
            {
                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;
                Filters.Scene[key4].GetShader().UseOpacity(EEMod.position.X);
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[key4].IsActive())
                {
                    Filters.Scene.Activate(key4, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
                }
                markerPlacer++;
                float pos1X = Main.screenPosition.X + Main.screenWidth - 900;
                float pos1Y = Main.screenPosition.Y + Main.screenHeight - 100 + 1000;
                float pos2X = Main.screenPosition.X + Main.screenWidth - 400;
                float pos2Y = Main.screenPosition.Y + Main.screenHeight - 400 + 1000;
                float pos3X = Main.screenPosition.X + Main.screenWidth - 700;
                float pos3Y = Main.screenPosition.Y + Main.screenHeight - 300 + 1000;
                float pos4X = Main.screenPosition.X + Main.screenWidth - 500;
                float pos4Y = Main.screenPosition.Y + Main.screenHeight - 200 + 1000;
                float pos5X = Main.screenPosition.X + Main.screenWidth - 1000;
                float pos5Y = Main.screenPosition.Y + Main.screenHeight - 400 + 1000;
                float pos6X = Main.screenPosition.X + Main.screenWidth - 300;
                float pos6Y = Main.screenPosition.Y + Main.screenHeight - 100 + 1000;
                float pos7X = Main.screenPosition.X + Main.screenWidth - 800;
                float pos7Y = Main.screenPosition.Y + Main.screenHeight - 150 + 1000;
                float pos8X = Main.screenPosition.X + Main.screenWidth - 200;
                float pos8Y = Main.screenPosition.Y + Main.screenHeight - 300 + 1000;
                Rectangle rectangle1 = new Rectangle((int)pos3X - 56, (int)pos3Y - 32, 118, 64);
                Rectangle rectangle2 = new Rectangle((int)pos2X - 56, (int)pos2Y - 32, 118, 64);
                Rectangle rectangle3 = new Rectangle((int)Main.screenPosition.X + (int)EEMod.position.X - 30, (int)Main.screenPosition.Y + (int)EEMod.position.Y - 30 + 1000, 60, 60);
                isNearIsland = false;
                isNearVolcano = false;
                if (rectangle1.Intersects(rectangle3))
                {
                    isNearIsland = true;
                }
                if (rectangle2.Intersects(rectangle3))
                {
                    isNearVolcano = true;
                }
                for (int j = 0; j < 200; j++)
                {
                    if (Main.projectile[j].type == mod.ProjectileType("PirateShip"))
                    {
                        Main.NewText((Main.projectile[j].Center - EEMod.position - Main.screenPosition).Length());
                        if ((Main.projectile[j].Center - EEMod.position - Main.screenPosition).Length() < 40)
                        {
                            EEMod.velocity += Main.projectile[j].velocity * 20;
                        }
                    }
                }
                if (markerPlacer == 1)
                {
                    //Projectile.NewProjectile(new Vector2(pos3X, pos3X), Vector2.Zero, mod.ProjectileType("Land"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos2X, pos2Y), Vector2.Zero, mod.ProjectileType("VolcanoIsland"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos3X, pos3Y), Vector2.Zero, mod.ProjectileType("Land"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos4X, pos4Y), Vector2.Zero, mod.ProjectileType("Lighthouse"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos5X, pos5Y), Vector2.Zero, mod.ProjectileType("Lighthouse2"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos6X, pos6Y), Vector2.Zero, mod.ProjectileType("Rock1"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos7X, pos7Y), Vector2.Zero, mod.ProjectileType("Rock2"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos8X, pos8Y), Vector2.Zero, mod.ProjectileType("Rock3"), 0, 0f, Main.myPlayer, 0, 0);
                    objectPos.Add(new Vector2(pos1X, pos1Y));
                    objectPos.Add(new Vector2(pos2X, pos2Y));
                    objectPos.Add(new Vector2(pos3X, pos3Y));
                    objectPos.Add(new Vector2(pos4X, pos4Y));
                    objectPos.Add(new Vector2(pos5X, pos5Y));
                    objectPos.Add(new Vector2(pos6X, pos6Y));
                    objectPos.Add(new Vector2(pos7X, pos7Y));
                    objectPos.Add(new Vector2(pos8X, pos8Y));
                    //upgrade, pirates, radial
                    for (int i = 0; i < 2; i++)
                    {
                        int GlacierChoose = Main.rand.Next(3);
                        float positionOfGlacXLast = positionOfGlacX;
                        float positionOfGlacYLast = positionOfGlacY;
                        positionOfGlacX = Main.rand.NextFloat(Main.screenPosition.X, Main.screenPosition.X + Main.screenWidth);
                        positionOfGlacY = Main.rand.NextFloat(Main.screenPosition.Y + 1000, Main.screenPosition.Y + Main.screenHeight + 1000);
                        Vector2 dist = new Vector2(positionOfGlacY - positionOfGlacYLast, positionOfGlacXLast - positionOfGlacX);
                        if (dist.Length() > 300)
                        {
                            switch (GlacierChoose)
                            {
                                case 0:
                                    {
                                        // Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, mod.ProjectileType("Glacier"), 0, 0f, Main.myPlayer, Prophecy.velocity.X, Prophecy.velocity.Y);
                                        break;
                                    }
                                case 1:
                                    {
                                        // Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, mod.ProjectileType("Glacier2"), 0, 0f, Main.myPlayer, Prophecy.velocity.X, Prophecy.velocity.Y);
                                        break;
                                    }
                                case 2:
                                    {
                                        //  Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, mod.ProjectileType("Glacier3"), 0, 0f, Main.myPlayer, Prophecy.velocity.X, Prophecy.velocity.Y);
                                        break;
                                    }
                            }
                        }
                    }
                }
                player.position = player.oldPosition;
                player.invis = true;
                player.AddBuff(BuffID.Cursed, 100000);
                if (markerPlacer % 600 == 0)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("PirateShip"), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("PirateShip"), 0, 0f, Main.myPlayer, 0, 0);
                }
                if (markerPlacer % 20 == 0)
                {
                    int CloudChoose = Main.rand.Next(5);
                    switch (CloudChoose)
                    {
                        case 0:
                            {
                                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("Cloud1"), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 1:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("Cloud6"), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 2:
                            {
                                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("Cloud3"), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 3:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("Cloud4"), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 4:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, mod.ProjectileType("Cloud5"), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                    }
                }

                if (markerPlacer % 40 == 0)
                {
                    Projectile.NewProjectile(Main.screenPosition + EEMod.position, Vector2.Zero, mod.ProjectileType("RedStrip"), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                }
            }
            else
            {
                Filters.Scene.Deactivate(key4);
                EEMod.position = EEMod.start;
                EEMod.velocity = Vector2.Zero;
                titleText2 = 0;
                if (!arrowFlag)
                {
                    Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("DesArrow"), 0, 0, player.whoAmI);
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("OceanArrow"), 0, 0, player.whoAmI);
                    arrowFlag = true;
                }
                if (player.Center.X / 16 >= (EEWorld.yes.X + 12) - 2 &&
                    player.Center.X / 16 <= (EEWorld.yes.X + 12) + 2 &&
                    player.Center.Y / 16 >= (EEWorld.yes.Y + 7) - 2 &&
                    player.Center.Y / 16 <= (EEWorld.yes.Y + 7) + 2)
                {
                    if (player.controlUp)
                    {
                        godMode = true;
                    }
                    (Main.projectile[Arrow].modProjectile as DesertArrowProjectile).visible = true;
                }
                else
                {
                    (Main.projectile[Arrow].modProjectile as DesertArrowProjectile).visible = false;
                }
                if (player.Center.X / 16 >= (EEWorld.ree.X + 2) - 2 &&
                    player.Center.X / 16 <= (EEWorld.ree.X + 2) + 2 &&
                    player.Center.Y / 16 >= (EEWorld.ree.Y + 14) - 2 &&
                    player.Center.Y / 16 <= (EEWorld.ree.Y + 14) + 2)
                {
                    if (player.controlUp)
                    {
                        triggerSeaCutscene = true;
                    }
                    (Main.projectile[Arrow2].modProjectile as OceanArrowProjectile).visible = true;

                }
                else
                {
                    (Main.projectile[Arrow2].modProjectile as OceanArrowProjectile).visible = false;
                }
            }
            Filters.Scene[key].GetShader().UseOpacity(timerForCutscene);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[key].IsActive())
            {
                Filters.Scene.Activate(key, player.Center).GetShader().UseOpacity(timerForCutscene);
            }
            if (!godMode)
            {
                Filters.Scene.Deactivate(key);
            }
            Filters.Scene[key3].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[key].IsActive())
            {
                Filters.Scene.Activate(key3, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                Filters.Scene.Deactivate(key3);
            }
            Action newWorld = EnterSub1;
            Action newWorld2 = EnterSub2;
            if (timerForCutscene >= 1400)
            {
                isSaving = false;
                godMode = false;
                timerForCutscene = 0;
                arrowFlag = false;
                noU = false;
                triggerSeaCutscene = false;
                cutSceneTriggerTimer = 0;
                cutSceneTriggerTimer2 = 1000;
                WorldGen.SaveAndQuit(newWorld);
            }
            if (cutSceneTriggerTimer >= 1000)
            {
                cutSceneTriggerTimer2 -= 5;
                if (cutSceneTriggerTimer >= 2020)
                {
                    markerPlacer = 0;
                    EEMod.position = new Vector2(1000, 1000);
                    isSaving = false;
                    godMode = false;
                    timerForCutscene = 0;
                    arrowFlag = false;
                    noU = false;
                    triggerSeaCutscene = false;
                    cutSceneTriggerTimer = 0;
                    cutSceneTriggerTimer2 = 1000;
                    WorldGen.SaveAndQuit(newWorld2);
                }
            }
        }
        internal static void PreSaveAndQuit()
        {

            Mod[] mods = ModLoader.Mods;
            for (int i = 0; i < mods.Length; i++)
            {
                mods[i].PreSaveAndQuit();
            }
        }
        public static void SaveAndQuit(Action callback = null)
        {
            Main.PlaySound(SoundID.MenuClose);
            PreSaveAndQuit();
            ThreadPool.QueueUserWorkItem(SaveAndQuitCallBack, callback);
        }
        public static void SaveAndQuitCallBack(object threadContext)
        {
            isSaving = true;
            try
            {
                Main.PlaySound(SoundID.Waterfall, -1, -1, 0);
                Main.PlaySound(SoundID.Lavafall, -1, -1, 0);
            }
            catch
            {
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.CacheSaveTime();
            }
            Main.invasionProgress = 0;
            Main.invasionProgressDisplayLeft = 0;
            Main.invasionProgressAlpha = 0f;
            Main.menuMode = 10;
            Main.gameMenu = true;
            Main.StopTrackedSounds();
            Terraria.Graphics.Capture.CaptureInterface.ResetFocus();
            Main.ActivePlayerFileData.StopPlayTimer();
            Player.SavePlayer(Main.ActivePlayerFileData);
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.saveWorld();
                Main.PlaySound(SoundID.MenuOpen);
            }
            else
            {
                Netplay.disconnect = true;
                Main.netMode = NetmodeID.SinglePlayer;
            }
            Main.fastForwardTime = false;
            Main.UpdateSundial();
            Main.menuMode = 0;
            if (threadContext != null)
            {
                ((Action)threadContext)();
            }
        }

        public static void Do_worldGenCallBack(object threadContext)
        {
            Main.PlaySound(SoundID.MenuOpen);
            WorldGen.clearWorld();
            EEMod.GenerateWorld(Main.ActiveWorldFileData.Seed, threadContext as GenerationProgress);
            WorldFile.saveWorld(Main.ActiveWorldFileData.IsCloudSave, resetTime: true);
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"C:\Users\tafid\Documents\My Games\Terraria\ModLoader\Worlds\{key1}.wld", false);
            WorldGen.playWorld();
        }
        public static void Do_worldGenCallBack2(object threadContext)
        {
            Main.PlaySound(SoundID.MenuOpen);
            WorldGen.clearWorld();
            EEMod.GenerateWorld2(Main.ActiveWorldFileData.Seed, threadContext as GenerationProgress);
            WorldFile.saveWorld(Main.ActiveWorldFileData.IsCloudSave, resetTime: true);
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"C:\Users\tafid\Documents\My Games\Terraria\ModLoader\Worlds\{key2}.wld", false);
            WorldGen.playWorld();
        }
        public override void UpdateBadLifeRegen()
        {
            if (player.position.Y < Main.rockLayer + 80f + 640f)
            {
                _ = EESub.Enter(EESub.mySubworldID) ?? false;
            }
        }
        public static void WorldGenCallBack(object threadContext)
        {
            try
            {
                Do_worldGenCallBack(threadContext);
            }
            catch (Exception ex)
            {
                Logging.Terraria.Error((object)Language.GetTextValue("tModLoader.WorldGenError"), ex);
            }
        }
        public static void CreateNewWorld(string text, GenerationProgress progress = null)
        {
            Main.rand = new UnifiedRandom(Main.ActiveWorldFileData.Seed);
            if (text == key1)
                ThreadPool.QueueUserWorkItem(Do_worldGenCallBack, progress);
            if (text == key2)
                ThreadPool.QueueUserWorkItem(Do_worldGenCallBack2, progress);
        }
        private void OnWorldNamed(string text, GenerationProgress progress)
        {
            if (text == key1)
            {
                string path = $@"C:\Users\tafid\Documents\My Games\Terraria\ModLoader\Worlds\{key1}.wld";
                if (!File.Exists(path))
                {
                    Main.worldName = text.Trim();
                    CreateNewWorld(key1, progress);
                }
                Main.ActiveWorldFileData = Terraria.IO.WorldFile.GetAllMetadata(path, false);
                WorldGen.playWorld();
            }
            if (text == key2)
            {
                string path = $@"C:\Users\tafid\Documents\My Games\Terraria\ModLoader\Worlds\{key2}.wld";
                if (!File.Exists(path))
                {
                    Main.worldName = text.Trim();
                    CreateNewWorld(key2, progress);
                }
                Main.ActiveWorldFileData = Terraria.IO.WorldFile.GetAllMetadata(path, false);
                WorldGen.playWorld();
            }
        }
        public void EnterSub1()
        {
            GenerationProgress progress = new GenerationProgress();
            OnWorldNamed(key1, progress);
        }
        public void EnterSub2()
        {
            GenerationProgress progress = new GenerationProgress();
            OnWorldNamed(key2, progress);
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {

            if (godMode)
            {

                int getRand = Main.rand.Next(5);
                int healSet = damage / 9;
                if (healSet > 5)
                {
                    healSet = 5;
                }
                if (healSet < 1)
                {
                    healSet = 1;
                }

                if (getRand == 1)
                {
                    player.statLife += healSet;
                    player.HealEffect(healSet);
                }
            }
            if (isQuartzRangedOn && item.ranged)
            {
                if (crit)
                    target.AddBuff(BuffID.CursedInferno, 120);
            }
            if (isQuartzSummonOn && item.summon)
            {
                if (Main.rand.Next(10) < 3)
                    target.AddBuff(BuffID.OnFire, 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {

            if (isQuartzRangedOn && proj.ranged)
            {
                if (crit)
                    target.AddBuff(BuffID.CursedInferno, 120);
            }
            if (isQuartzSummonOn && proj.minion)
            {
                if (Main.rand.Next(10) < 3)
                    target.AddBuff(BuffID.OnFire, 180);
            }
        }
        private void ResetMinionEffect()
        {
            quartzCrystal = false;
        }
    }
}
