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
                SeaObject.Add(new Island(new Vector2(500, 500), GetTexture("EEMod/SeamapAssets/Land")));
                SeaObject.Add(new Island(new Vector2(-400, -400), GetTexture("EEMod/SeamapAssets/VolcanoIsland"), true));
                SeaObject.Add(new Island(new Vector2(-700, -300), GetTexture("EEMod/SeamapAssets/Land"), true));
                SeaObject.Add(new Island(new Vector2(-500, -200), GetTexture("EEMod/SeamapAssets/Lighthouse")));
                SeaObject.Add(new Island(new Vector2(-1000, -400), GetTexture("EEMod/SeamapAssets/Lighthouse2")));
                SeaObject.Add(new Island(new Vector2(-300, -100), GetTexture("EEMod/SeamapAssets/Rock1")));
                SeaObject.Add(new Island(new Vector2(-800, -150), GetTexture("EEMod/SeamapAssets/Rock2")));
                SeaObject.Add(new Island(new Vector2(-200, -300), GetTexture("EEMod/SeamapAssets/Rock3")));
                SeaObject.Add(new Island(new Vector2(-100, -40), GetTexture("EEMod/SeamapAssets/MainIsland"), true));
                SeaObject.Add(new Island(new Vector2(-300, -600), GetTexture("EEMod/SeamapAssets/CoralReefsEntrance"), true));
                SeaObject.Add(new Island(new Vector2(-600, -800), GetTexture("EEMod/SeamapAssets/Land"), true));
                SeaObject.Add(new Island(new Vector2(-300, -250), GetTexture("EEMod/SeamapAssets/Rock2")));

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

                for (int i = 0; i < 400; i++)
                {
                    int CloudChoose = Main.rand.Next(3);
                    Vector2 CloudPos = new Vector2(Main.rand.NextFloat(-200, Main.screenWidth), Main.rand.NextFloat(800, Main.screenHeight + 1000));
                    Vector2 dist = new Vector2(Main.screenWidth, Main.screenHeight + 1000) - CloudPos;

                    if (dist.Length() > 1140)
                    {
                        Texture2D cloudTexture;

                        switch (CloudChoose)
                        {
                            case 0:
                            case 1:
                                cloudTexture = GetTexture("EEMod/SeamapAssets/DarkCloud" + (CloudChoose + 1));
                                break;

                            default:
                                cloudTexture = GetTexture("EEMod/SeamapAssets/DarkCloud3");
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
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.VolcanoIsland);

                        prevKey = KeyID.VolcanoIsland;
                    }
                }
                else if (Islands["Island"].isColliding)
                {
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island);

                        prevKey = KeyID.Island;
                    }
                }
                else if (Islands["MainIsland"].isColliding)
                {
                    if (player.controlUp)
                    {
                        ReturnHome();

                        prevKey = baseWorldName;
                    }
                }
                else if (Islands["CoralReefsEntrance"].isColliding)
                {
                    if (player.controlUp)
                    {
                        importantCutscene = true;
                    }
                }
                else if (Islands["UpperLand"].isColliding)
                {
                    if (player.controlUp)
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
                        cloud = new MCloud(GetTexture("EEMod/SeamapAssets/Cloud6"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 144, 42, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
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
                        cloud = new MCloud(GetTexture("EEMod/SeamapAssets/Cloud4"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 100, 48, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud4>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 4:
                    {
                        cloud = new MCloud(GetTexture("EEMod/SeamapAssets/Cloud5"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 96, 36, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
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
        float[] anchorLerp = new float[12];
        public void UpdateIslands()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            for (int i = 0; i < modPlayer.SeaObject.Count; i++)
            {
                EEPlayer.Island current = modPlayer.SeaObject[i];
                Vector2 currentPos = current.posToScreen.ForDraw();
                Color drawColour = Lighting.GetColor((int)(current.posToScreen.X / 16f), (int)(current.posToScreen.Y / 16f));
                if (current.isColliding)
                {
                    if (anchorLerp[i] < 1)
                        anchorLerp[i] += 0.02f;
                }
                else
                {
                    if (anchorLerp[i] > 0)
                        anchorLerp[i] -= 0.02f;
                }
                Main.spriteBatch.Draw(GetTexture("SeamapAssets/Anchor"), currentPos + new Vector2(0, (float)Math.Sin(markerPlacer / 20f)) * 4 + new Vector2(current.texture.Width / 2f - EEMod.instance.GetTexture("SeamapAssets/Anchor").Width / 2f, -80), drawColour * anchorLerp[i]);
                if (modPlayer.quickOpeningFloat > 0.01f)
                {
                    float lerp = 1 - (modPlayer.quickOpeningFloat / 10f);
                    if (i > 4 && i < 8 || i == 11)
                    {
                        float score = currentPos.X + currentPos.Y;
                        Main.spriteBatch.Draw(current.texture, currentPos + new Vector2(0, (float)Math.Sin(score + markerPlacer / 40f)) * 4, drawColour * lerp);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(current.texture, currentPos, drawColour * lerp);
                    }
                }
                else
                {
                    if (i > 4 && i < 8 || i == 11)
                    {
                        float score = currentPos.X + currentPos.Y;
                        Main.spriteBatch.Draw(current.texture, currentPos + new Vector2(0, (float)Math.Sin(score + markerPlacer / 40f)) * 4, drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                    }
                    else
                    {
                        Main.spriteBatch.Draw(current.texture, currentPos, drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                    }
                }
            }
            var OceanElements = EEPlayer.OceanMapElements;
            for (int i = 0; i < OceanElements.Count; i++)
            {
                var element = OceanElements[i];
                element.Draw(Main.spriteBatch);
            }
            for (int i = 0; i < modPlayer.seagulls.Count; i++)
            {
                var element = modPlayer.seagulls[i];
                element.frameCounter++;
                element.Position += new Vector2(0, -0.5f);
                element.Draw(EEMod.instance.GetTexture("SeamapAssets/Seagulls"), 9, 5);
            }
        }

        private Texture2D texture;
        private Rectangle frame;
        private int frames;
        public static float ShipHelthMax = 7;
        public static float ShipHelth = 7;
        public Vector2 position;
        public Vector2 velocity;
        public static readonly Vector2 start = new Vector2(1700, 900);

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

        private float flash = 0;
        private float markerPlacer = 0;

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

        private int cannonDelay = 60;
        public Vector2 otherBoatPos;
        public Vector2 currentLightningPos;
        float intenstityLightning;
        private void DrawShip()
        {
            markerPlacer++;
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            position.X = MathHelper.Clamp(position.X, Main.screenWidth * 0.6f, Main.screenWidth);
            position.Y = MathHelper.Clamp(position.Y, 0, Main.screenHeight);
            if (!Main.gamePaused)
            {
                position += velocity;
                if (player.controlJump)
                {
                    velocity.Y -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlDown)
                {
                    velocity.Y += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlRight)
                {
                    velocity.X += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlLeft)
                {
                    velocity.X -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlUseItem && cannonDelay <= 0 && eePlayer.cannonballType != 0)
                {
                    switch (eePlayer.cannonballType)
                    {
                        case 1:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize(position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyCannonball>(), 0, 0);
                            break;

                        case 2:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize(position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyExplosiveCannonball>(), 0, 0);
                            break;

                        case 3:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize(position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyHallowedCannonball>(), 0, 0);
                            break;

                        case 4:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize(position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyChlorophyteCannonball>(), 0, 0);
                            break;

                        case 5:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize(position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyLuminiteCannonball>(), 0, 0);
                            break;
                    }
                    Main.PlaySound(SoundID.Item61);
                    cannonDelay = 60;
                }
                cannonDelay--;
            }
            velocity.X = Helpers.Clamp(velocity.X, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            velocity.Y = Helpers.Clamp(velocity.Y, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            texture = EEMod.instance.GetTexture("SeamapAssets/ShipMount");

            frames = 12;
            int frameNum = 0;
            if (Main.netMode == NetmodeID.SinglePlayer || ((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 0))
            {
                if (eePlayer.boatSpeed == 3)
                {
                    frameNum = 1;
                }

                if (eePlayer.boatSpeed == 1)
                {
                    frameNum = 0;
                }
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                switch (player.team)
                {
                    case 1:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 3;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 2;
                        break;
                    case 2:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 9;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 8;
                        break;
                    case 3:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 5;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 4;
                        break;
                    case 4:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 7;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 6;
                        break;
                    case 5:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 11;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 10;
                        break;
                }
            }

            if (!Main.gamePaused)
            {
                velocity *= 0.98f;
            }
            for (int i = 0; i < eePlayer.objectPos.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }

                if (i == 1)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                }

                if (i == 2)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }

                if (i == 4)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                }

                if (i == 7)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }

                if (i == 0)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }
            }
            //Lighting.AddLight(eePlayer.objectPos[1], 0.9f, 0.9f, 0.9f);
            if (Main.rand.NextBool(100) && !Main.dayTime)
            {
                currentLightningPos = Main.screenPosition + new Vector2(Main.rand.Next(500), Main.rand.Next(1000));
                intenstityLightning = Main.rand.NextFloat(.1f, .2f);
            }
            if (intenstityLightning > 0)
            {
                float rand = Main.rand.NextFloat(.2f, 5f);
                intenstityLightning -= 0.008f;
                float light = rand * intenstityLightning;
                Lighting.AddLight(currentLightningPos, light, light, light);
            }
            Texture2D texture3 = EEMod.instance.GetTexture("SeamapAssets/ShipHelthSheet");
            Lighting.AddLight(Main.screenPosition + position, .1f, .1f, .1f);
            //float quotient = ShipHelth / ShipHelthMax; // unused
            Rectangle rect = new Rectangle(0, (int)(texture3.Height / 8 * ShipHelth), texture3.Width, texture3.Height / 8);
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 175, 50), rect, Color.White, 0, rect.Size() / 2, 1, SpriteEffects.None, 0);
            for (int i = 0; i < Main.ActivePlayersCount; i++)
            {
                if (i == 0)
                {
                    Color drawColour = Lighting.GetColor((int)((Main.screenPosition.X + position.X) / 16f), (int)((Main.screenPosition.Y + position.Y) / 16f));
                    Main.spriteBatch.Draw(texture, position, new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), velocity.X / 10, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        EEServerVariableCache.SyncBoatPos(position, velocity.X);
                    }
                    for (int j = 0; j < 255; j++)
                    {
                        if (Main.player[j].active && j != Main.myPlayer)
                        {
                            Color drawColour = Lighting.GetColor((int)(EEServerVariableCache.OtherBoatPos[j].X / 16f), (int)(EEServerVariableCache.OtherBoatPos[j].Y / 16f));
                            Main.spriteBatch.Draw(texture, EEServerVariableCache.OtherBoatPos[j], new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), EEServerVariableCache.OtherRot[j] / 10f, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, EEServerVariableCache.OtherRot[j] < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        }
                    }

                }
            }
            flash += 0.01f;
            if (flash == 2)
            {
                flash = 10;
            }
        }
    }
}