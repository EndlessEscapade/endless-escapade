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
using ReLogic.Graphics;
using EEMod.Seamap.SeamapAssets;
using EEMod.Autoloading;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public List<SeagullsClass> seagulls = new List<SeagullsClass>();
        public float brightness;
        public bool isStorming;

        public int timerForCutscene;
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subTextAlpha;
        public bool noU;
        public bool triggerSeaCutscene;
        public int cutSceneTriggerTimer;
        public float cutSceneTriggerTimer3;
        public int coralReefTrans;
        public int markerPlacer;
        public Vector2 position;
        public Vector2 velocity;
        public List<Vector2> objectPos = new List<Vector2>();
        public List<Island> SeaObject = new List<Island>();
        public List<int> SeaObjectFrames = new List<int>();
        public Dictionary<string, Island> Islands = new Dictionary<string, Island>();
        public string baseWorldName;

        [FieldInit]
        internal static List<IOceanMapElement> OceanMapElements = new List<IOceanMapElement>();


        public void UpdateSea()
        {
            #region Controlling island brightness
            if (Main.dayTime)
            {
                if (Main.time <= 200)
                    brightness += 0.0025f;

                if (Main.time >= 52000 && brightness > 0.1f)
                    brightness -= 0.0025f;

                if (Main.time > 2000 && Main.time < 52000)
                    brightness = 0.5f;
            }
            else
            {
                brightness = 0.1f;
            }

            if (Main.time % 1000 == 0)
            {
                if (Main.rand.NextBool(10)) isStorming = !isStorming;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncBrightness, brightness);
            }
            #endregion

            #region Opening cutscene for seamap
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
            #endregion

            markerPlacer++;

            #region Placing islands
            if (markerPlacer == 1)
            {
                SeaObject.Add(new Island(new Vector2(500, 500), GetTexture("EEMod/Seamap/SeamapAssets/TropicalIsland"), "TropicalIsland", 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-1200, -400), GetTexture("EEMod/Seamap/SeamapAssets/VolcanoIsland"), "VolcanoIsland", 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-700, -300), GetTexture("EEMod/Seamap/SeamapAssets/TropicalIsland"), "TropicalIsland2", 16, 10, true));
                //SeaObject.Add(new Island(new Vector2(-500, -200), GetTexture("EEMod/Seamap/SeamapAssets/Lighthouse2"), 1, 0));
                SeaObject.Add(new Island(new Vector2(-400, -100), GetTexture("EEMod/Seamap/SeamapAssets/Rock1"), null, 16, 10));
                SeaObject.Add(new Island(new Vector2(-800, -150), GetTexture("EEMod/Seamap/SeamapAssets/Rock2"), null, 16, 10));
                SeaObject.Add(new Island(new Vector2(-200, -300), GetTexture("EEMod/Seamap/SeamapAssets/Rock3"), null, 16, 10));
                SeaObject.Add(new Island(new Vector2(-209, -55), GetTexture("EEMod/Seamap/SeamapAssets/MainIsland"), "MainIsland", 1, 0, true));
                SeaObject.Add(new Island(new Vector2(-200, -600), GetTexture("EEMod/Seamap/SeamapAssets/CoralReefsEntrance"), "CoralReefsEntrance", 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-450, -650), GetTexture("EEMod/Seamap/SeamapAssets/MoyaiIsland"), "MoyaiIsland", 16, 10, true));
                SeaObject.Add(new Island(new Vector2(-300, -250), GetTexture("EEMod/Seamap/SeamapAssets/Rock4"), null, 16, 10));

                if (SeaObjectFrames.Count != SeaObject.Count)
                {
                    SeaObjectFrames.Capacity = SeaObject.Count;
                    for (int i = 0; i < SeaObject.Count; i++)
                    {
                        SeaObjectFrames.Add(0);
                    }
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
            #endregion

            #region If ship crashes
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
            #endregion

            #region Warping to islands
            if (markerPlacer > 10)
            {
                if (Islands["VolcanoIsland"].isColliding)
                {
                    /*if (EEMod.Inspect.JustPressed)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.VolcanoIsland);

                        prevKey = KeyID.VolcanoIsland;
                    }*/
                }
                else if (Islands["MainIsland"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        ReturnHome();
                        prevKey = KeyID.Sea;
                    }
                }
                else if (Islands["CoralReefsEntrance"].isColliding)
                {
                    if (EEMod.Inspect.JustPressed)
                    {
                        importantCutscene = true;
                        prevKey = KeyID.Sea;
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
            #endregion

            #region Warp cutscene
            if (importantCutscene)
            {
                EEMod.Noise2D.Parameters["noiseTexture"].SetValue(EEMod.instance.GetTexture("Noise/noise"));
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
                }
            }
            #endregion

            //THIS NEEDS CHANGING
            #region Updating entities
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
            #endregion

            #region Making actual player look normal
            player.position = player.oldPosition;
            player.invis = true;

            player.AddBuff(BuffID.Cursed, 100000);
            #endregion

            //THIS NEEDS CHANGING
            #region Spawning entities
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

            if (markerPlacer % 200 == 0)
            {
                if (seagulls.Count < 500)
                {
                    GraphicObject.LazyAppendInBoids(ref seagulls, Main.rand.Next(4, 8));
                }
                else
                {
                    seagulls.RemoveAt(0);
                }
            }
            #endregion

            //THIS NEEDS CHANGING
            #region Spawning clouds
            if (markerPlacer % 20 == 0)
            {
                int CloudChoose = Main.rand.Next(5);
                IOceanMapElement cloud;

                switch (CloudChoose)
                {
                    case 0:
                    {
                        break;
                    }
                    case 1:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud6"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 144, 42, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        break;
                    }
                    case 2:
                    {
                        break;
                    }
                    case 3:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud4"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 100, 48, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        break;
                    }
                    case 4:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud5"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 96, 36, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        break;
                    }
                }
            }
            #endregion

            #region Placing red marks on the map
            /*if (markerPlacer % 40 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + EEMod.instance.position, Vector2.Zero, ProjectileType<RedStrip>(), 0, 0f, Main.myPlayer, EEMod.instance.velocity.X, EEMod.instance.velocity.Y);
            }*/
            #endregion
        }

        internal static void UpdateOceanMapElements()
        {
            for (int i = 0; i < OceanMapElements.Count; i++)
            {
                OceanMapElements[i].Update();
            }
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            EEPlayer clone = clientPlayer as EEPlayer;

            if (clone.EEPosition != EEMod.instance.position)
            {
                var packet = mod.GetPacket();
                packet.Write(triggerSeaCutscene);
                packet.WriteVector2(EEPosition);
                packet.Send();
            }
        }

        public override void clientClone(ModPlayer clientClone)
        {

        }

        public Vector2 EEPosition;

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(triggerSeaCutscene);
            packet.WriteVector2(EEPosition);
            packet.Send(toWho, fromWho);
        }

        #region Shifting subworlds
        public void UpdateCutscenesAndTempShaders()
        {
            Filters.Scene[shad1].GetShader().UseOpacity(timerForCutscene);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad1].IsActive())
            {
                Filters.Scene.Activate(shad1, player.Center).GetShader().UseOpacity(timerForCutscene);
            }
            if (!godMode)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad1].IsActive())
                {
                    Filters.Scene.Deactivate(shad1);
                }
            }
            Filters.Scene[shad3].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad3].IsActive())
            {
                Filters.Scene.Activate(shad3, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad3].IsActive())
                {
                    Filters.Scene.Deactivate(shad3);
                }
            }
            /*if(Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:MyTestShader"].IsActive())
            {
                Filters.Scene.Activate("EEMod:MyTestShader", player.Center).GetShader().UseColor(Color.Red).UseTargetPosition(player.Center);
            }*/
            if (timerForCutscene >= 1400)
            {
                Initialize();
                prevKey = KeyID.BaseWorldName;
                SM.SaveAndQuit(KeyID.Pyramids); //pyramid
            }
            if (cutSceneTriggerTimer > 0)
            {
                if (cutSceneTriggerTimer >= 500)
                {
                    Initialize();
                    prevKey = KeyID.BaseWorldName;
                    SM.SaveAndQuit(KeyID.Sea); //sea
                }
            }
        }
        #endregion

        public struct Island
        {
            EEPlayer player => Main.LocalPlayer.GetModPlayer<EEPlayer>();
            public Island(Vector2 pos, Texture2D tex, string NameOfIsland, int frameCount = 1, int frameSpid = 2, bool canCollide = false, int startingFrame = 0)
            {
                posX = (int)pos.X;
                posY = (int)pos.Y;
                texture = tex;
                frames = frameCount;
                frameSpeed = frameSpid;
                currentFrame = startingFrame;
                this.canCollide = canCollide;
                if (NameOfIsland != null)
                {
                    if (!player.Islands.ContainsKey(NameOfIsland))
                    {
                        player.Islands.Add(NameOfIsland, this);
                    }
                }
            }

            private readonly int posX;
            private readonly int posY;
            public int frames;
            public int currentFrame;
            public int frameSpeed;
            public bool canCollide;

            public int posXToScreen
            {
                get => posX + (int)Main.screenPosition.X + Main.screenWidth;
            }

            public int posYToScreen
            {
                get => posY + (int)Main.screenPosition.Y + Main.screenHeight;
            }

            public Texture2D texture;
            public Vector2 posToScreen => new Vector2(posXToScreen - texture.Width / 2, posYToScreen - texture.Height / (2 * frames));
            public Rectangle hitBox => new Rectangle((int)posToScreen.X, (int)posToScreen.Y - texture.Height / (frames * 2), texture.Width, texture.Height / (frames));
            private Rectangle ShipHitBox => new Rectangle((int)Main.screenPosition.X + (int)EEMod.instance.position.X - 30, (int)Main.screenPosition.Y + (int)EEMod.instance.position.Y - 30, 30, 30);
            public bool isColliding => hitBox.Intersects(ShipHitBox) && canCollide;
        }

        public class DarkCloud : IOceanMapElement
        {
            public Vector2 pos;
            public float flash;

            public int posXToScreen
            {
                get => (int)(pos.X + Main.screenPosition.X);
            }

            public int posYToScreen
            {
                get => (int)(pos.Y + Main.screenPosition.Y);
            }

            public Texture2D texture;
            public float scale, alpha;
            private readonly EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            public DarkCloud(Vector2 pos, Texture2D tex, float scale, float alpha)
            {
                flash += 0.01f;
                this.pos = pos;
                texture = tex;
                this.scale = scale;
                this.alpha = alpha;
                flash = Main.rand.NextFloat(0, 4);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                flash += 0.003f;
                Vector2 p = new Vector2(posXToScreen + (float)Math.Sin(flash) * 10, posYToScreen - 1000).ForDraw();
                Color drawcolor = Lighting.GetColor(posXToScreen / 16, (posYToScreen - 1000) / 16) * modPlayer.seamapLightColor;
                drawcolor.A = (byte)alpha;
                if (modPlayer.quickOpeningFloat > 0.01f)
                {
                    float lerp = 1 - (modPlayer.quickOpeningFloat / 10f);
                    spriteBatch.Draw(texture, p, null, drawcolor * lerp, 0f, default, scale, SpriteEffects.None, 0f);
                    return;
                }
                spriteBatch.Draw(texture, p, null, drawcolor * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, default, scale, SpriteEffects.None, 0f);
            }

            public void Update()
            {

            }
        }

        public class MovingCloud : IOceanMapElement
        {
            public Vector2 pos;
            public float flash;

            public int posXToScreen
            {
                get => (int)(pos.X + Main.screenPosition.X);
            }

            public int posYToScreen
            {
                get => (int)(pos.Y + Main.screenPosition.Y);
            }

            public Texture2D texture;
            public float scale, alpha;
            private readonly EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            public MovingCloud(Vector2 pos, Texture2D tex, float scale, float alpha)
            {
                this.pos = pos;
                texture = tex;
                this.scale = scale;
                this.alpha = alpha;
                flash = Main.rand.NextFloat(0, 4);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                flash += 0.003f;
                Vector2 p = new Vector2(posXToScreen + (float)Math.Sin(flash) * 10, posYToScreen - 1000).ForDraw();
                Color drawcolor = Lighting.GetColor(posXToScreen / 16, (posYToScreen - 1000) / 16) * modPlayer.brightness * (modPlayer.isStorming ? 2 / 3f : 1);
                drawcolor.A = (byte)alpha;
                spriteBatch.Draw(texture, p, null, drawcolor * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, default, scale, SpriteEffects.None, 0f);
            }

            public void Update()
            {

            }
        }

        public class MCloud : IOceanMapElement
        {
            private Vector2 position;
            private readonly int width, height;
            private readonly float alpha, scale;
            private readonly Texture2D texture;
            private readonly EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            private Vector2 Center => new Vector2(position.X + width / 2f, position.Y + height / 2f);

            public MCloud(Texture2D texture, Vector2 position, int width, int height, float scale, float alpha)
            {
                //scale = projectile.ai[0];
                //alpha = (int)projectile.ai[1];
                this.scale = scale;
                this.alpha = alpha;
                this.texture = texture;
                this.position = position;
                this.width = width;
                this.height = height;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Vector2 newPos = Center + Main.screenPosition;
                Rectangle rect = new Rectangle(0, 0, width, height);
                Color lightColour = Lighting.GetColor((int)newPos.X / 16, (int)newPos.Y / 16) * modPlayer.brightness * (modPlayer.isStorming ? 2 / 3f : 1);
                spriteBatch.Draw(texture, Center, rect, lightColour * (alpha / 255f) * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, rect.Size() / 2, scale, SpriteEffects.None, 0f);
            }

            public void Update()
            {
                position.X -= 0.3f;
            }
        }

        internal interface IOceanMapElement
        {
            void Update();

            void Draw(SpriteBatch spriteBatch);
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

        #region Drawing "Disembark" text
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
        #endregion

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