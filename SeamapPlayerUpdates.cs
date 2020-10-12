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
using ReLogic.Graphics;
using EEMod.Seamap.SeamapAssets;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public List<SeagullsClass> seagulls = new List<SeagullsClass>();
        public float brightness;

        public void UpdateSea()
        {
            if (Main.dayTime)
            {
                if (Main.time <= 200)
                {
                    brightness += 0.0025f;
                }

                if (Main.time >= 52000 && brightness > 0.1f)
                {
                    brightness -= 0.0025f;
                }

                if (Main.time > 2000 && Main.time < 52000)
                {
                    brightness = 0.5f;
                }
            }
            else
            {
                brightness = 0.1f;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncBrightness, brightness);
            }

            if (quickOpeningFloat > 0.01f)
            {
                quickOpeningFloat -= quickOpeningFloat / 20f;
            }
            else
            {
                quickOpeningFloat = 0;
            }

            Filters.Scene["EEMod:SeaOpening"].GetShader().UseIntensity(quickOpeningFloat);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:SeaOpening"].IsActive())
            {
                Filters.Scene.Activate("EEMod:SeaOpening", player.Center).GetShader().UseIntensity(quickOpeningFloat);
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
            Filters.Scene[shad2].GetShader().UseOpacity(EEMod.instance.position.X);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Activate(shad2, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }

            markerPlacer++;

            if (markerPlacer == 1)
            {
                SeaObject.Add(new Island(new Vector2(500, 500), GetTexture("EEMod/Seamap/SeamapAssets/TropicalIsland"), 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-400, -400), GetTexture("EEMod/Seamap/SeamapAssets/VolcanoIsland"), 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-700, -300), GetTexture("EEMod/Seamap/SeamapAssets/TropicalIsland"), 16, 10, true));
                //SeaObject.Add(new Island(new Vector2(-500, -200), GetTexture("EEMod/Seamap/SeamapAssets/Lighthouse2"), 1, 0));
                SeaObject.Add(new Island(new Vector2(-400, -100), GetTexture("EEMod/Seamap/SeamapAssets/Rock1"), 16, 10));
                SeaObject.Add(new Island(new Vector2(-800, -150), GetTexture("EEMod/Seamap/SeamapAssets/Rock2"), 16, 10));
                SeaObject.Add(new Island(new Vector2(-200, -300), GetTexture("EEMod/Seamap/SeamapAssets/Rock3"), 16, 10));
                SeaObject.Add(new Island(new Vector2(-209, -55), GetTexture("EEMod/Seamap/SeamapAssets/MainIsland"), 1, 0, true));
                SeaObject.Add(new Island(new Vector2(-200, -600), GetTexture("EEMod/Seamap/SeamapAssets/CoralReefsEntrance"), 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-450, -650), GetTexture("EEMod/Seamap/SeamapAssets/MoyaiIsland"), 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-300, -250), GetTexture("EEMod/Seamap/SeamapAssets/Rock4"), 16, 10));

                if (SeaObjectFrames.Count != SeaObject.Count)
                {
                    SeaObjectFrames.Capacity = SeaObject.Count;
                    for (int i = 0; i < SeaObject.Count; i++)
                    {
                        SeaObjectFrames.Add(0);
                    }
                }

                if (!Islands.ContainsKey("VolcanoIsland"))
                {
                    Islands.Add("VolcanoIsland", SeaObject[1]);
                }

                if (!Islands.ContainsKey("Island"))
                {
                    Islands.Add("Island", SeaObject[2]);
                }

                if (!Islands.ContainsKey("Lighthouse"))
                {
                    Islands.Add("Lighthouse", SeaObject[3]);
                }

                if (!Islands.ContainsKey("Lighthouse2"))
                {
                    Islands.Add("Lighthouse2", SeaObject[4]);
                }

                if (!Islands.ContainsKey("MainIsland"))
                {
                    Islands.Add("MainIsland", SeaObject[8]);
                }

                if (!Islands.ContainsKey("CoralReefsEntrance"))
                {
                    Islands.Add("CoralReefsEntrance", SeaObject[9]);
                }

                if (!Islands.ContainsKey("UpperLand"))
                {
                    Islands.Add("UpperLand", SeaObject[10]);
                }

                for (int i = 0; i < 300; i++)
                {
                    int CloudChoose = Main.rand.Next(3);
                    Vector2 CloudPos = new Vector2(Main.rand.NextFloat(-200, Main.screenWidth * 0.7f), Main.rand.NextFloat(800, Main.screenHeight + 1000));
                    Vector2 dist = new Vector2(Main.screenWidth, Main.screenHeight + 1000) - CloudPos;

                    if (dist.Length() > 1140)
                    {
                        Texture2D cloudTexture;

                        switch (CloudChoose)
                        {
                            case 0:
                            case 1:
                                cloudTexture = GetTexture("EEMod/Seamap/SeamapAssets/DarkCloud" + (CloudChoose + 1));
                                break;

                            default:
                                cloudTexture = GetTexture("EEMod/Seamap/SeamapAssets/DarkCloud3");
                                break;
                        }

                        OceanMapElements.Add(new DarkCloud(CloudPos, cloudTexture, Main.rand.NextFloat(.6f, 1), Main.rand.NextFloat(60, 180)));
                    }
                }

                for (int i = 0; i < SeaObject.Count; i++)
                {
                    objectPos.Add(SeaObject[i].posToScreen);
                }

                //upgrade, pirates, radial
            }
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
            if (markerPlacer > 10)
            {
                if (Islands["VolcanoIsland"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.VolcanoIsland);

                        prevKey = KeyID.VolcanoIsland;
                    }
                }
                else if (Islands["Island"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island);

                        prevKey = KeyID.Island;
                    }
                }
                else if (Islands["MainIsland"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        ReturnHome();

                        prevKey = baseWorldName;
                    }
                }
                else if (Islands["CoralReefsEntrance"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        importantCutscene = true;
                    }
                }
                else if (Islands["UpperLand"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island2);

                        prevKey = KeyID.Island2;
                    }
                }
                else
                {
                    subTextAlpha -= 0.02f;

                    if (subTextAlpha <= 0)
                    {
                        subTextAlpha = 0;
                    }
                }
            }
            if (!arrowFlag)
            {
                arrowFlag = true;
            }

            foreach (Island island in Islands.Values)
            {
                if (island.isColliding)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                    {
                        subTextAlpha = 1;
                    }
                }
            }

            if (importantCutscene)
            {
                EEMod.Noise2D.Parameters["noiseTexture"].SetValue(EEMod.instance.GetTexture("noise"));
                Filters.Scene["EEMod:Noise2D"].GetShader().UseOpacity(cutSceneTriggerTimer / 180f);

                if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Noise2D"].IsActive())
                {
                    Filters.Scene.Activate("EEMod:Noise2D", player.Center).GetShader().UseOpacity(0);
                }

                cutSceneTriggerTimer++;

                if (cutSceneTriggerTimer > 180)
                {
                    Initialize();
                    Filters.Scene.Deactivate("EEMod:Noise2D");
                    SM.SaveAndQuit(KeyID.CoralReefs); // coral reefs

                    prevKey = KeyID.CoralReefs;
                }
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
                    Crate a = (Crate)Main.projectile[j].modProjectile;

                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && !a.sinking)
                    {
                        //Crate loot tables go here
                        if (Main.rand.NextBool())
                        {
                            player.QuickSpawnItem(ItemID.GoldBar, Main.rand.Next(4, 9));
                        }
                        else
                        {
                            player.QuickSpawnItem(ItemID.PlatinumBar, Main.rand.Next(4, 9));
                        }

                        if (Main.rand.NextBool())
                        {
                            player.QuickSpawnItem(ItemID.ApprenticeBait, Main.rand.Next(2, 4));
                        }
                        else
                        {
                            player.QuickSpawnItem(ItemID.JourneymanBait, 1);
                        }

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
            {
                NPC.NewNPC((int)Main.screenPosition.X + Main.screenWidth - 200, (int)Main.screenPosition.Y + Main.rand.Next(1000), NPCType<MerchantBoat>());
            }

            if (markerPlacer % 7200 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<RedDutchman>(), 0, 0f, Main.myPlayer, 0, 0);
            }

            if (markerPlacer % 800 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Crate>(), 0, 0f, Main.myPlayer, 0, 0);
            }

            if (markerPlacer % 150 == 0)
            {
                if (seagulls.Count < 500)
                {
                    GraphicObject.LazyAppendInBoids(ref seagulls, 5);
                }
                else
                {
                    seagulls.RemoveAt(0);
                }
            }

            if (markerPlacer % 20 == 0)
            {
                int CloudChoose = Main.rand.Next(5);
                IOceanMapElement cloud;

                switch (CloudChoose)
                {
                    case 0:
                    {
                        // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 1:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud6"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 144, 42, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud6>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 2:
                    {
                        // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 3:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud4"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 100, 48, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud4>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 4:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud5"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 96, 36, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud5>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                }
            }

            if (markerPlacer % 40 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + EEMod.instance.position, Vector2.Zero, ProjectileType<RedStrip>(), 0, 0f, Main.myPlayer, EEMod.instance.velocity.X, EEMod.instance.velocity.Y);
            }
        }
    }

    public partial class EEMod : Mod
    {
        public float[] anchorLerp = new float[12];
        public Texture2D texture;
        public Rectangle frame;
        public int frames;
        public static float ShipHelthMax = 7;
        public static float ShipHelth = 7;
        public Vector2 position;
        public Vector2 velocity;
        public static readonly Vector2 start = new Vector2(1700, 900);
        public int cannonDelay = 60;
        public Vector2 otherBoatPos;
        public Vector2 currentLightningPos;
        public float intenstityLightning;
        private void DrawSubText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.subTextAlpha;
            Color color = Color.White;
            if (Main.worldName == KeyID.Sea)
            {
                text = "Disembark?";
                color *= alpha;
            }
            if (text != null)
            {
                Vector2 textSize = Main.fontMouseText.MeasureString(text);
                float textPositionLeft = position.X - textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(textPositionLeft, position.Y + 20), color * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }

        public float flash = 0;
        public float markerPlacer = 0;

        public static bool IsPlayerLocalServerOwner(int whoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                RemoteClient client = Netplay.Clients[i];
                if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
                {
                    return true;
                }
            }
            return false;
        }
    }
}