using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.Projectiles;
using EEMod.Tiles;
using EEMod.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;
using static Terraria.ModLoader.ModContent;
using EEMod.Seamap.SeamapContent;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.NPCs.Aquamarine;
using Terraria.DataStructures;
using EEMod.NPCs.Friendly;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        private readonly List<float> _bubbleRoots = new List<float>();
        public List<BubbleClass> bubbles = new List<BubbleClass>();

        public float quickOpeningFloat = 5f;

        public bool jellyfishMigration;

        public void UpdatePyramids()
        {
            if (noU)
            {
                titleText -= 0.005f;
            }
            else
            {
                titleText += 0.005f;
            }

            if (titleText >= 1)
            {
                noU = true;
            }
            else
            {
                titleText = 0;
            }

            titleText2 = 1;

            if (!arrowFlag)
            {
                //Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                arrowFlag = true;
            }

            //DesArrowProjectile desArrowProj = (DesArrowProjectile)Main.projectile[Arrow].ModProjectile;

            if (Player.Center.X / 16 >= Main.spawnTileX - 5 && Player.Center.X / 16 <= Main.spawnTileX + 5 && Player.Center.Y / 16 >= Main.spawnTileY - 5 && Player.Center.Y / 16 <= Main.spawnTileY + 5)
            {
                if (Player.controlUp)
                {
                    ReturnHome();
                }

                ArrowsUIState.DesertArrowVisible = true;
            }
            else
            {
                // ArrowsUIState.DesertArrowVisible = false;
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene[SunThroughWallsShader].IsActive())
            {
                Filters.Scene.Deactivate(SunThroughWallsShader);
            }
        }

        int SpireCutscene;
        public void UpdateCR()
        {
            /*if (player.position.Y >= 800 * 16 && !player.accDivingHelm)
            {
                player.AddBuff(BuffType<WaterPressure>(), 60);
            }*/

            if (noU)
            {
                titleText -= 0.005f;
            }
            else
            {
                titleText += 0.005f;
            }

            if (titleText >= 1)
            {
                noU = true;
            }

            if (titleText <= 0)
            {
                titleText = 0;
            }

            seamapUpdateCount++;

            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, CoralReefs.SpirePosition * 16) < 700 * 700)
            {
                HasVisitedSpire = true;
            }

            if (HasVisitedSpire && SpireCutscene < 200)
            {
                FixateCameraOn(CoralReefs.SpirePosition * 16 + new Vector2(0, -13 * 16), 64f, false, true, 0);
                SpireCutscene++;
            }

            if (SpireCutscene == 200)
            {
                HasVisitedSpire = true;
                SpireCutscene++;
                TurnCameraFixationsOff();
            }

            if (!arrowFlag)
            {
                NPC.NewNPC((int)(CoralReefs.SpirePosition.X * 16), (int)(CoralReefs.SpirePosition.Y * 16), NPCType<AquamarineSpire>());
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, Main.myPlayer);
                }

                arrowFlag = true;
            }

            if (CoralReefs.CoralBoatPos == Vector2.Zero)
            {
                CoralReefs.CoralBoatPos = new Vector2(200, 48);
            }

            try
            {
                //Projectile oceanarrow = Main.projectile[Arrow2];

                if (Helpers.PointInRectangle(Player.Center / 16, CoralReefs.CoralBoatPos.X, CoralReefs.CoralBoatPos.Y + 12, 4, 4))
                {
                    if (Player.controlUp)
                    {
                        Initialize();

                        SeamapPlayerShip.localship.position = new Vector2(Main.screenWidth - 300, Main.screenHeight - 600);

                        SubworldManager.EnterSubworld<Sea>();
                    }

                    ArrowsUIState.OceanArrowVisible = true;
                }
                else
                {
                    // ArrowsUIState.OceanArrowVisible = false;
                }
            }
            catch { }

            if (Main.moonPhase == 4 && !Main.dayTime && !jellyfishMigration)
            {
                Main.NewText("Jellyfish are migrating on the surface!", new Color(50, 255, 130));
                jellyfishMigration = true;
            }
            if (jellyfishMigration && Main.dayTime)
            {
                jellyfishMigration = false;
            }
        }

        bool placedShipTether = false;
        bool firstLoad = true;

        public int tetherProj;
        public int sailProj;

        public void UpdateWorld()
        {
            if(!placedShipTether)
            {
                tetherProj = Projectile.NewProjectile(new ProjectileSource_BySourceId(ModContent.ProjectileType<TileExperimentation>()), 
                    shipCoords * 16, Vector2.Zero, ModContent.ProjectileType<TileExperimentation>(), 0, 0f);

                TileExperimentation tether = (Main.projectile[tetherProj].ModProjectile as TileExperimentation);

                tether.pos1 = (shipCoords * 16) + (new Vector2(43, 2) * 16) + new Vector2(8, 8);
                tether.pos2 = (shipCoords * 16) + (new Vector2(56, 9) * 16) + new Vector2(8, 8);

                sailProj = Projectile.NewProjectile(new ProjectileSource_BySourceId(ModContent.ProjectileType<TornSails>()), (shipCoords * 16) + new Vector2((26 * 16) + 8, 32), 
                    Vector2.Zero, ModContent.ProjectileType<TornSails>(), 0, 0);

                placedShipTether = true;
            }

            if(firstLoad)
            {
                NPC.NewNPC(((int)shipCoords.X + 108 + 7) * 16, ((int)shipCoords.Y - 8) * 16, ModContent.NPCType<Sailor>());
                firstLoad = false;
            }

            if (EEModConfigClient.Instance.ParticleEffects)
            {
                seamapUpdateCount++;
            }
            else
            {
                seamapUpdateCount = 0;
            }
            if (seamapUpdateCount == 1)
            {
                if (prevKey == KeyID.Sea)
                {
                    Player.Center = new Vector2(100 * 16, (TileCheckWater(100) - 22) * 16);
                }
            }
            baseWorldName = Main.ActiveWorldFileData.Name;
            if (Main.netMode != NetmodeID.Server && Filters.Scene[SunThroughWallsShader].IsActive())
            {
                Filters.Scene.Deactivate(SunThroughWallsShader);
            }
            SeamapPlayerShip.localship.position = SeamapPlayerShip.start;
            SeamapPlayerShip.localship.velocity = Vector2.Zero;
            titleText2 = 0;
            if (!arrowFlag)
            {
                arrowFlag = true;

                for (int i = 0; i < 200; i++)
                {
                    if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
        }
    }
}