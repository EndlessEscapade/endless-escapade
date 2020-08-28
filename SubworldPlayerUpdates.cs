using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles.OceanMap;
using EEMod.Projectiles.CoralReefs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using EEMod.NPCs.Bosses.Hydros;
using static Terraria.ModLoader.ModContent;
using EEMod.NPCs;
using EEMod.Tiles;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.NPCs.Friendly;
using EEMod.Items;
using EEMod.Buffs.Debuffs;
using EEMod.Buffs.Buffs;
using System.Windows.Forms;
using System.Drawing.Imaging;
using EEMod.Extensions;
using EEMod.Items.Fish;
using EEMod.ID;
using EEMod.Projectiles.Armor;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Walls;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public void UpdatePyramids()
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
                Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                arrowFlag = true;
            }
            DesArrowProjectile desArrowProj = Main.projectile[Arrow].modProjectile as DesArrowProjectile;
            if (player.Center.X / 16 >= Main.spawnTileX - 5 &&
                player.Center.X / 16 <= Main.spawnTileX + 5 &&
                player.Center.Y / 16 >= Main.spawnTileY - 5 &&
                player.Center.Y / 16 <= Main.spawnTileY + 5)
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
        public void UpdateSea()
        {
            if (!noU)
                titleText += 0.005f;
            if (titleText >= 1)
                noU = true;
            if (noU)
                titleText -= 0.005f;
            Filters.Scene[shad2].GetShader().UseOpacity(EEMod.instance.position.X);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Activate(shad2, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            markerPlacer++;
            if (markerPlacer == 1)
            {
                SeaObject.Add(new Island(new Vector2(500, 500), GetTexture("EEMod/Projectiles/OceanMap/Land")));
                SeaObject.Add(new Island(new Vector2(-400, -400), GetTexture("EEMod/Projectiles/OceanMap/VolcanoIsland"), true));
                SeaObject.Add(new Island(new Vector2(-700, -300), GetTexture("EEMod/Projectiles/OceanMap/Land"), true));
                SeaObject.Add(new Island(new Vector2(-500, -200), GetTexture("EEMod/Projectiles/OceanMap/Lighthouse")));
                SeaObject.Add(new Island(new Vector2(-1000, -400), GetTexture("EEMod/Projectiles/OceanMap/Lighthouse2")));
                SeaObject.Add(new Island(new Vector2(-300, -100), GetTexture("EEMod/Projectiles/OceanMap/Rock1")));
                SeaObject.Add(new Island(new Vector2(-800, -150), GetTexture("EEMod/Projectiles/OceanMap/Rock2")));
                SeaObject.Add(new Island(new Vector2(-200, -300), GetTexture("EEMod/Projectiles/OceanMap/Rock3")));
                SeaObject.Add(new Island(new Vector2(-100, -40), GetTexture("EEMod/Projectiles/OceanMap/MainIsland"), true));
                SeaObject.Add(new Island(new Vector2(-300, -600), GetTexture("EEMod/Projectiles/OceanMap/CoralReefsEntrance"), true));
                SeaObject.Add(new Island(new Vector2(-600, -800), GetTexture("EEMod/Projectiles/OceanMap/Land"), true));
                if (!Islands.ContainsKey("VolcanoIsland"))
                    Islands.Add("VolcanoIsland", SeaObject[1]);
                if (!Islands.ContainsKey("Island"))
                    Islands.Add("Island", SeaObject[2]);
                if (!Islands.ContainsKey("Lighthouse"))
                    Islands.Add("Lighthouse", SeaObject[3]);
                if (!Islands.ContainsKey("Lighthouse2"))
                    Islands.Add("Lighthouse2", SeaObject[4]);
                if (!Islands.ContainsKey("MainIsland"))
                    Islands.Add("MainIsland", SeaObject[8]);
                if (!Islands.ContainsKey("CoralReefsEntrance"))
                    Islands.Add("CoralReefsEntrance", SeaObject[9]);
                if (!Islands.ContainsKey("UpperLand"))
                    Islands.Add("UpperLand", SeaObject[10]);
                for (int i = 0; i < 400; i++)
                {
                    int CloudChoose = Main.rand.Next(3);
                    Vector2 CloudPos = new Vector2(Main.rand.NextFloat(Main.screenPosition.X - 200, Main.screenPosition.X + Main.screenWidth), Main.rand.NextFloat(Main.screenPosition.Y + 800, Main.screenPosition.Y + Main.screenHeight + 1000));
                    Vector2 dist = (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight + 1000)) - CloudPos;
                    if (dist.Length() > 1140)
                    {
                        switch (CloudChoose)
                        {
                            case 0:
                                {
                                    Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                    Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                    break;
                                }
                            case 1:
                                {
                                    Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud2>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                    break;
                                }
                            case 2:
                                {
                                    Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                    break;
                                }
                        }
                    }
                }

                for (int i = 0; i < SeaObject.Count; i++)
                    objectPos.Add(SeaObject[i].posToScreen);

                //upgrade, pirates, radial

            }

            Rectangle ShipHitBox = new Rectangle((int)Main.screenPosition.X + (int)EEMod.instance.position.X - 30, (int)Main.screenPosition.Y + (int)EEMod.instance.position.Y - 30 + 1000, 60, 60);
            isNearIsland = false;
            isNearIsland2 = false;
            isNearVolcano = false;
            isNearMainIsland = false;
            isNearCoralReefs = false;

            if (EEMod.ShipHelth <= 0)
            {
                if (prevKey == baseWorldName || prevKey == "Main")
                {
                    ReturnHome();
                }
                else
                {
                    Initialize();
                    arrowFlag = false;
                    SM.SaveAndQuit(prevKey);
                }
            }
            if (Islands["VolcanoIsland"].hitBox.Intersects(ShipHitBox))
            {
                isNearVolcano = true;
            }
            if (Islands["Island"].hitBox.Intersects(ShipHitBox))
            {
                isNearIsland = true;
            }
            if (Islands["MainIsland"].hitBox.Intersects(ShipHitBox))
            {
                isNearMainIsland = true;
            }
            if (Islands["CoralReefsEntrance"].hitBox.Intersects(ShipHitBox))
            {
                isNearCoralReefs = true;
            }
            if (Islands["UpperLand"].hitBox.Intersects(ShipHitBox))
            {
                isNearIsland2 = true;
            }
            if (!arrowFlag)
            {
                Anchors = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, Islands["Island"].posXToScreen, Islands["Island"].posYToScreen + 1000);
                AnchorsVolc = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, Islands["VolcanoIsland"].posXToScreen, Islands["VolcanoIsland"].posYToScreen - 50 + 1000);
                AnchorsMain = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, Islands["MainIsland"].posXToScreen, Islands["MainIsland"].posYToScreen - 50 + 1000);
                AnchorsCoral = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, Islands["CoralReefsEntrance"].posXToScreen, Islands["CoralReefsEntrance"].posYToScreen - 50 + 1000);
                arrowFlag = true;
            }
            if (isNearIsland)
            {
                subTextAlpha += 0.02f;
                if (subTextAlpha >= 1)
                    subTextAlpha = 1;
                (Main.projectile[Anchors].modProjectile as Anchor).visible = true;
                if (player.controlUp)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.Island);
                    prevKey = KeyID.Island;
                }
            }
            else
            {
                (Main.projectile[Anchors].modProjectile as Anchor).visible = false;
            }
            if (isNearIsland2)
            {
                subTextAlpha += 0.02f;
                if (subTextAlpha >= 1)
                    subTextAlpha = 1;
                (Main.projectile[Anchors].modProjectile as Anchor).visible = true;
                if (player.controlUp)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.Island2);
                    prevKey = KeyID.Island2;
                }
            }
            else
            {
                //  (Main.projectile[Anchors].modProjectile as Anchor).visible = false;
            }
            if (isNearVolcano)
            {
                subTextAlpha += 0.02f;
                if (subTextAlpha >= 1)
                    subTextAlpha = 1;
                (Main.projectile[AnchorsVolc].modProjectile as Anchor).visible = true;
                if (player.controlUp)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.VolcanoIsland);
                    prevKey = KeyID.VolcanoIsland;
                }
            }
            else
            {
                (Main.projectile[AnchorsVolc].modProjectile as Anchor).visible = false;
            }

            if (isNearMainIsland)
            {
                if (player.controlUp)
                {
                    ReturnHome();
                    prevKey = baseWorldName;
                }
                subTextAlpha += 0.02f;
                if (subTextAlpha >= 1)
                    subTextAlpha = 1;
                (Main.projectile[AnchorsMain].modProjectile as Anchor).visible = true;
            }
            else
            {
                (Main.projectile[AnchorsMain].modProjectile as Anchor).visible = false;
            }
            if (isNearCoralReefs && markerPlacer > 1)
            {
                subTextAlpha += 0.02f;
                if (subTextAlpha >= 1)
                    subTextAlpha = 1;
                (Main.projectile[AnchorsCoral].modProjectile as Anchor).visible = true;
                if (player.controlUp)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.CoralReefs); // coral reefs
                    prevKey = KeyID.CoralReefs;
                }
            }
            else
            {
                (Main.projectile[AnchorsCoral].modProjectile as Anchor).visible = false;
            }
            if (!isNearVolcano && !isNearIsland && !isNearCoralReefs && !isNearMainIsland)
            {
                subTextAlpha -= 0.02f;
                if (subTextAlpha <= 0)
                    subTextAlpha = 0;
            }

            for (int j = 0; j < 450; j++)
            {
                if (Main.projectile[j].type == ProjectileType<PirateShip>() || Main.projectile[j].type == ProjectileType<RedDutchman>() || Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                {
                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && Main.projectile[j].type != ProjectileType<EnemyCannonball>())
                    {
                        EEMod.ShipHelth -= 1;
                        EEMod.instance.velocity += Main.projectile[j].velocity * 20;
                    }
                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 30 && Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                    {
                        EEMod.ShipHelth -= 1;
                        EEMod.instance.velocity += Main.projectile[j].velocity;
                    }
                }
                if (Main.projectile[j].type == ProjectileType<Crate>())
                {
                    Crate a = Main.projectile[j].modProjectile as Crate;
                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && !a.sinking)
                    {
                        //Crate loot tables go here
                        if (Main.rand.NextBool())
                            player.QuickSpawnItem(ItemID.GoldBar, Main.rand.Next(4, 9));
                        else
                            player.QuickSpawnItem(ItemID.PlatinumBar, Main.rand.Next(4, 9));

                        if (Main.rand.NextBool())
                            player.QuickSpawnItem(ItemID.ApprenticeBait, Main.rand.Next(2, 4));
                        else
                            player.QuickSpawnItem(ItemID.JourneymanBait, 1);

                        player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(0, 2));
                        player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(0, 100));
                        player.QuickSpawnItem(ItemID.CopperCoin, Main.rand.Next(0, 100));


                        a.sinking = true;
                        a.Sink();
                    }
                }
            }
            player.position = player.oldPosition;
            player.invis = true;
            player.AddBuff(BuffID.Cursed, 100000);
            if (markerPlacer % 600 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
                Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
            }
            if (markerPlacer % 2400 == 0)
                NPC.NewNPC((int)Main.screenPosition.X + Main.screenWidth - 200, (int)Main.screenPosition.Y + Main.rand.Next(1000), NPCType<MerchantBoat>());

            if (markerPlacer % 7200 == 0)
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<RedDutchman>(), 0, 0f, Main.myPlayer, 0, 0);

            if (markerPlacer % 800 == 0)
                Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Crate>(), 0, 0f, Main.myPlayer, 0, 0);

            if (markerPlacer % 20 == 0)
            {
                int CloudChoose = Main.rand.Next(5);
                switch (CloudChoose)
                {
                    case 0:
                        {
                            // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                            break;
                        }
                    case 1:
                        {
                            Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud6>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                            break;
                        }
                    case 2:
                        {
                            // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                            break;
                        }
                    case 3:
                        {
                            Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud4>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                            break;
                        }
                    case 4:
                        {
                            Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud5>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                            break;
                        }
                }
            }

            if (markerPlacer % 40 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + EEMod.instance.position, Vector2.Zero, ProjectileType<RedStrip>(), 0, 0f, Main.myPlayer, EEMod.instance.velocity.X, EEMod.instance.velocity.Y);
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
            if (!noU)
                titleText += 0.005f;
            if (titleText >= 1)
                noU = true;
            if (noU)
                titleText -= 0.005f;
            if (titleText <= 0)
                titleText = 0;
            markerPlacer++;
            if (markerPlacer % 40 == 0)
            {
                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<CoralBubble>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
            }

            if (!arrowFlag)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, Main.myPlayer);
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
                NPC.NewNPC((Main.maxTilesX / 2) * 16, 75 * 16, NPCType<AtlantisCore>());
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
            VolcanoArrowProj voclanoarrow = Main.projectile[Arrow2].modProjectile as VolcanoArrowProj;
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
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<Particle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
                }
            }
        }
        public void UpdateWorld()
        {
            int lastNoOfShipTiles = missingShipTiles.Count;
            try
            {
                ShipComplete();
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
                    int proj = Projectile.NewProjectile(tile * 16 + new Vector2((8) + (-3 * 16), (8) + (-6 * 16)), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                    WhiteBlock newProj = Main.projectile[proj].modProjectile as WhiteBlock;
                    newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                }
            }
            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = mod.GetPacket();
                netMessage.Write(shipComplete);
                netMessage.Send();
            }
            if (!importantCutscene)
            {
                //SM.SaveAndQuit(KeyID.Cutscene1);
                importantCutscene = true;
            }
            if (EEModConfigClient.Instance.ParticleEffects)
            {
                markerPlacer++;
            }
            else
            {
                markerPlacer = 0;
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
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<BetterLighting>(), 0, 0f, Main.myPlayer, 0, player.whoAmI);
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
                    int proj = Projectile.NewProjectile(tile * 16 + new Vector2((8) + (-3 * 16), (8) + (-6 * 16)), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                    WhiteBlock newProj = Main.projectile[proj].modProjectile as WhiteBlock;
                    newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                }
            }
            if (EntracesPosses.Count > 0)
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
            }
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
