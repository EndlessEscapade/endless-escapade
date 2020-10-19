using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.Friendly;
using EEMod.Projectiles;
using EEMod.Tiles;
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
using EEMod.SeamapAssets;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        private readonly List<float> _bubbleRoots = new List<float>();
        public List<BubbleClass> bubbles = new List<BubbleClass>();

        public float quickOpeningFloat = 5f;

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
                Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                arrowFlag = true;
            }

            DesArrowProjectile desArrowProj = (DesArrowProjectile)Main.projectile[Arrow].modProjectile;

            if (player.Center.X / 16 >= Main.spawnTileX - 5 && player.Center.X / 16 <= Main.spawnTileX + 5 && player.Center.Y / 16 >= Main.spawnTileY - 5 && player.Center.Y / 16 <= Main.spawnTileY + 5)
            {
                if (player.controlUp)
                {
                    ReturnHome();
                }

                desArrowProj.visible = true;
            }
            else
            {
                desArrowProj.visible = false;
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Deactivate(shad2);
            }
        }

        public void UpdateCR()
        {
            if (player.position.Y >= 800 * 16 && !player.accDivingHelm)
            {
                player.AddBuff(BuffType<WaterPressure>(), 60);
            }

            if (HydrosCheck())
            {
                NPC.NewNPC((int)position.X * 16, (int)position.Y * 16 - 400, NPCType<Hydros>());

                EEWorld.EEWorld.instance.minionsKilled = 0;
            }

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

            markerPlacer++;

            if (markerPlacer % 80 == 0)
            {
                _bubbleRoots.Add(Main.rand.Next(Main.screenWidth) + player.Center.X - Main.screenWidth / 2);
            }

            if (markerPlacer % 120 == 0)
            {
                for (int i = 0; i < _bubbleRoots.Count; i++)
                {
                    Vector2 BubblePos = new Vector2(_bubbleRoots[i], player.Center.Y + 600);
                    BubbleClass bubble = new BubbleClass
                    {
                        scale = Main.rand.NextFloat(0.5f, 1f),
                        alpha = Main.rand.NextFloat(.2f, .8f),
                        Position = BubblePos,
                        flash = Main.rand.NextFloat(0, 100),
                        Velocity = new Vector2(Main.rand.NextFloat(0.5f, 1), 0)
                    };

                    if (bubbles.Count < 500)
                    {
                        bubbles.Add(bubble);
                    }
                }

                if (bubbles.Count > 500)
                {
                    bubbles.RemoveAt(0);
                }

                //Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<CoralBubble>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
            }

            foreach (BubbleClass bubble in bubbles)
            {
                bubble.flash++;
                bubble.Position -= new Vector2((float)Math.Sin(bubble.flash / (bubble.Velocity.X * 30)), bubble.Velocity.X);
            }

            if (!arrowFlag)
            {
                for (int i = 0; i < EESubWorlds.OrbPositions.Count; i++)
                {
                    NPC.NewNPC((int)EESubWorlds.OrbPositions[i].X * 16, (int)EESubWorlds.OrbPositions[i].Y * 16, NPCType<SpikyOrb>());
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, Main.myPlayer);
                }

                player.ClearBuff(BuffID.Cursed);
                arrowFlag = true;
            }

            if (EESubWorlds.CoralBoatPos == Vector2.Zero)
            {
                EESubWorlds.CoralBoatPos = new Vector2(200, 48);
            }

            try
            {
                Projectile oceanarrow = Main.projectile[Arrow2];

                if (Helpers.PointInRectangle(player.Center / 16, EESubWorlds.CoralBoatPos.X, EESubWorlds.CoralBoatPos.Y + 12, 4, 4))
                {
                    if (player.controlUp)
                    {
                        Initialize();

                        EEMod.instance.position = new Vector2(Main.screenWidth - 300, Main.screenHeight - 600);

                        SM.SaveAndQuit(KeyID.Sea);
                    }

                    oceanarrow.ai[1] = 1;
                }
                else
                {
                    oceanarrow.ai[1] = 0;
                }
            }
            catch
            {
            }
        }

        public void UpdateIsland()
        {
            player.ClearBuff(BuffID.Cursed);

            if (!arrowFlag)
            {
                NPC.NewNPC(Main.maxTilesX / 2 * 16, 75 * 16, NPCType<AtlantisCore>());

                arrowFlag = true;
            }
        }

        public void UpdateVolcano()
        {
            firstFrameVolcano = true;

            player.ClearBuff(BuffID.Cursed);

            if (!arrowFlag)
            {
                Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                NPC.NewNPC(600 * 16, 594 * 16, NPCType<VolcanoSmoke>());

                arrowFlag = true;
            }

            if (SubWorldSpecificVolcanoInsidePos == Vector2.Zero)
            {
                SubWorldSpecificVolcanoInsidePos = new Vector2(198, 198);
            }

            VolcanoArrowProj voclanoarrow = (VolcanoArrowProj)Main.projectile[Arrow2].modProjectile;

            if (Helpers.PointInRectangle(player.Center / 16, SubWorldSpecificVolcanoInsidePos.X - 4, SubWorldSpecificVolcanoInsidePos.Y - 4, 8, 8))
            {
                if (player.controlUp)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.VolcanoInside);
                }

                voclanoarrow.visible = true;
            }
            else
            {
                voclanoarrow.visible = false;
            }
        }

        public void UpdateInnerVolcano()
        {
            player.ClearBuff(BuffID.Cursed);

            if (firstFrameVolcano)
            {
                NPC.NewNPC(200, TileCheck(200, TileType<MagmastoneTile>()), NPCType<Akumo>());

                firstFrameVolcano = false;
            }
        }

        public void UpdateCutscene()
        {
            markerPlacer++;

            if (markerPlacer == 5)
            {
                player.AddBuff(BuffID.Cursed, 100000);
                NPC.NewNPC(193 * 16, (120 - 30) * 16, NPCType<SansSlime>());
                NPC.NewNPC(207 * 16, (120 - 30) * 16, NPCType<GreenSlimeGoBurr>());
            }

            if (markerPlacer > 120 * 8)
            {
                if (markerPlacer == 5 && EEModConfigClient.Instance.ParticleEffects)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<EEParticle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
                }
            }
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

                    if (Main.LocalPlayer.Center.X < (DefShipPosX + ShipTiles.GetLength(1)) * 16 && Main.LocalPlayer.Center.Y < (DefShipPosY + ShipTiles.GetLength(0)) * 16)
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
                markerPlacer++;
            }
            else
            {
                markerPlacer = 0;
            }
            if (markerPlacer == 1)
            {
                if (prevKey == KeyID.Sea)
                {
                    player.Center = new Vector2(100 * 16, (TileCheckWater(100) - 22) * 16);
                    player.ClearBuff(BuffID.Cursed);
                }
            }
            if (markerPlacer == 10 && EEModConfigClient.Instance.ParticleEffects)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<Particle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), player.whoAmI);
            }
            baseWorldName = Main.ActiveWorldFileData.Name;
            if (Main.netMode != NetmodeID.Server && Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Deactivate(shad2);
            }
            EEMod.instance.position = EEMod.start;
            EEMod.instance.velocity = Vector2.Zero;
            titleText2 = 0;
            if (!arrowFlag)
            {
                if (EEModConfigClient.Instance.BetterLighting)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<BetterLighting>(), 0, 0f, Main.myPlayer, 0, player.whoAmI);
                }

                player.ClearBuff(BuffID.Cursed);

                Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, player.whoAmI);

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

            Vector2 revisedRee = new Vector2(ree.X == 0 ? 100 : ree.X,
                                             ree.Y == 0 ? TileCheckWater(100) - 22 : ree.Y);
            if (Helpers.PointInRectangle(player.Center / 16, revisedRee.X, revisedRee.Y + 12, 4, 4) && shipComplete)
            {
                if (player.controlUp)
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
            }
        }
    }
}