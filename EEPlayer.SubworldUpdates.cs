using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.NPCs.CoralReefs;
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

            //DesArrowProjectile desArrowProj = (DesArrowProjectile)Main.projectile[Arrow].modProjectile;

            if (player.Center.X / 16 >= Main.spawnTileX - 5 && player.Center.X / 16 <= Main.spawnTileX + 5 && player.Center.Y / 16 >= Main.spawnTileY - 5 && player.Center.Y / 16 <= Main.spawnTileY + 5)
            {
                if (player.controlUp)
                {
                    ReturnHome();
                }

                ArrowsUIState.DesertArrowVisible = true;
            }
            else
            {
                ArrowsUIState.DesertArrowVisible = false;
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
                for (int i = 0; i < CoralReefs.OrbPositions.Count; i++)
                {
                    NPC.NewNPC((int)CoralReefs.OrbPositions[i].X * 16, (int)CoralReefs.OrbPositions[i].Y * 16, NPCType<SpikyOrb>());
                }
                NPC.NewNPC((int)CoralReefs.SpirePosition.X * 16 + 160 - 8, (int)CoralReefs.SpirePosition.Y * 16, NPCType<AquamarineSpire>());
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

                if (Helpers.PointInRectangle(player.Center / 16, CoralReefs.CoralBoatPos.X, CoralReefs.CoralBoatPos.Y + 12, 4, 4))
                {
                    if (player.controlUp)
                    {
                        Initialize();

                        SeamapPlayerShip.localship.position = new Vector2(Main.screenWidth - 300, Main.screenHeight - 600);

                        SubworldManager.EnterSubworld<Sea>();
                    }

                    ArrowsUIState.OceanArrowVisible = true;
                }
                else
                {
                    ArrowsUIState.OceanArrowVisible = false;
                }
            }
            catch { }

            if (Main.moonType == 4 && !Main.dayTime && !jellyfishMigration)
            {
                Main.NewText("Jellyfish are migrating on the surface!", new Color(50, 255, 130));
                jellyfishMigration = true;
            }
            if (jellyfishMigration && Main.dayTime)
            {
                jellyfishMigration = false;
            }
        }

        public void UpdateIsland()
        {
            if (!arrowFlag)
            {
                NPC.NewNPC(Main.maxTilesX / 2 * 16, 75 * 16, NPCType<AtlantisCore>());

                arrowFlag = true;
            }
        }

        public void UpdateVolcano()
        {
            firstFrameVolcano = true;

            if (!arrowFlag)
            {
                //Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                //Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                NPC.NewNPC(600 * 16, 594 * 16, NPCType<VolcanoSmoke>());

                arrowFlag = true;
            }

            if (SubWorldSpecificVolcanoInsidePos == Vector2.Zero)
            {
                SubWorldSpecificVolcanoInsidePos = new Vector2(198, 198);
            }

            //VolcanoArrowProj voclanoarrow = (VolcanoArrowProj)Main.projectile[Arrow2].modProjectile;

            if (Helpers.PointInRectangle(player.Center / 16, SubWorldSpecificVolcanoInsidePos.X - 4, SubWorldSpecificVolcanoInsidePos.Y - 4, 8, 8))
            {
                if (player.controlUp)
                {
                    Initialize();
                    SubworldManager.EnterSubworld<CoralReefs>();
                }

                //voclanoarrow.visible = true;
                ArrowsUIState.DesertArrowVisible = true;
            }
            else
            {
                //voclanoarrow.visible = false;
                ArrowsUIState.DesertArrowVisible = false;
            }
        }

        public void UpdateInnerVolcano()
        {
            /*player.ClearBuff(BuffID.Cursed);

            if (firstFrameVolcano)
            {
                NPC.NewNPC(200, TileCheck(200, TileType<MagmastoneTile>()), NPCType<Akumo>());

                firstFrameVolcano = false;
            }*/
        }

        public void UpdateCutscene()
        {
            seamapUpdateCount++;

            if (seamapUpdateCount == 5)
            {
                player.AddBuff(BuffID.Cursed, 100000);
                NPC.NewNPC(193 * 16, (120 - 30) * 16, NPCType<SansSlime>());
                NPC.NewNPC(207 * 16, (120 - 30) * 16, NPCType<GreenSlimeGoBurr>());
            }

            /*if (markerPlacer > 120 * 8)
            {
                if (markerPlacer == 5 && EEModConfigClient.Instance.ParticleEffects)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<EEParticle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
                }
            }*/
        }

        public void UpdateWorld()
        {
            /*object tempLights = typeof(Lighting).GetField("tempLights", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            try
            {
                Main.NewText(tempLights.get);
                //Point16[] arrayOfAllKeys = buffer.Keys.ToArray();
                // foreach (Point16 p in arrayOfAllKeys)
                // {
                //    Main.NewText(p);
                // }
            }
            catch
            {
                Main.NewText("few");
            }*/
            if (missingShipTiles != null)
            {
                int lastNoOfShipTiles = missingShipTiles.Count;

                try
                {
                    int DefShipPosX = 100;
                    int DefShipPosY = TileCheckWater(100) - 22;

                    if (Main.LocalPlayer.Center.X < (DefShipPosX + ShipTiles.GetLength(1)) * 16 && Main.LocalPlayer.Center.Y < (DefShipPosY + ShipTiles.GetLength(0)) * 16 && Main.GameUpdateCount % 100 == 0)
                    {
                        ShipComplete();
                    }
                }
                catch
                {
                }

                if (missingShipTiles.Count != lastNoOfShipTiles)
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                        {
                            Main.projectile[i].Kill();
                        }
                    }

                    foreach (Vector2 tile in missingShipTiles)
                    {
                        int proj = Projectile.NewProjectile(tile * 16 + new Vector2(8 + (-3 * 16), 8 + (-6 * 16)), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                        WhiteBlock newProj = Main.projectile[proj].modProjectile as WhiteBlock;
                        newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                    }
                }
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
                    player.Center = new Vector2(100 * 16, (TileCheckWater(100) - 22) * 16);
                }
            }
            /*if (markerPlacer == 10 && EEModConfigClient.Instance.ParticleEffects)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<EEParticle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), player.whoAmI);
            }*/
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
                /*if (EEModConfigClient.Instance.BetterLighting)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<BetterLighting>(), 0, 0f, Main.myPlayer, 0, player.whoAmI);
                }*/

                //Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                //Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, player.whoAmI);

                arrowFlag = true;

                for (int i = 0; i < 200; i++)
                {
                    if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                    {
                        Main.projectile[i].Kill();
                    }
                }

                foreach (Vector2 tile in missingShipTiles)
                {
                    int proj = Projectile.NewProjectile(tile * 16 + new Vector2(8 + (-3 * 16), 8 + (-6 * 16)), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                    WhiteBlock newProj = (WhiteBlock)Main.projectile[proj].modProjectile;

                    newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                }
            }
            /* if (EntracesPosses.Count > 0)
             {
                 if (Main.projectile[Arrow].modProjectile is DesArrowProjectile arrow)
                 {
                     Vector2 entrace = EntracesPosses[0];
                     if (Helpers.PointInRectangle(player.Center / 16, entrace.X + 10, entrace.Y + 5, 4, 4))
                     {
                         if (player.controlUp)
                         {
                             godMode = true;
                         }
                         arrow.visible = true;
                     }
                     else
                     {
                         arrow.visible = false;
                     }
                 }
             }*/

            /*if (Helpers.PointInRectangle(player.Center / 16, revisedRee.X, revisedRee.Y + 12, 4, 4) && shipComplete)
            {
                if (EEMod.Inspect.Current)
                {
                    triggerSeaCutscene = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = mod.GetPacket();
                        netMessage.Write(triggerSeaCutscene);
                        netMessage.Send();
                    }
                }

                Main.projectile[Arrow2].ai[1] = 1;
            }
            else
            {
                Main.projectile[Arrow2].ai[1] = 0;
            }*/
        }
    }
}