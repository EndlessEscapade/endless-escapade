using EEMod.Autoloading;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs.CoralReefs;
using EEMod.Skies;
using EEMod.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;
using EEMod.Tiles.Furniture;
using EEMod.SeamapAssets;
using EEMod.Seamap.SeamapContent;
using EEMod.MachineLearning;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static EEMod instance;

        public override void PostSetupContent()
        {
        }

        public static Texture2D ScTex;

        public static double worldSurface;

        public static double worldSurfaceLow;

        public static double worldSurfaceHigh;

        public static double rockLayer;

        public static double rockLayerLow;

        public static double rockLayerHigh;

        public static int _lastSeed;
        public static ParticleZoneHandler Particles;
        public Handwriting HandwritingCNN;
        internal delegate void UIUpdateDelegate(GameTime gameTime);
        internal delegate void UIModifyLayersDelegate(List<GameInterfaceLayer> layers, int mouseTextIndex, GameTime lastUpdateUIGameTime);
        internal static event UIUpdateDelegate OnUpdateUI;
        internal static event UIModifyLayersDelegate OnModifyInterfaceLayers;

        public static void GenerateWorld(string key, int seed, GenerationProgress customProgressObject = null)
        {
            typeof(EESubWorlds).GetMethod(key).Invoke(null, new object[] { seed, customProgressObject });
        }

        public static Effect NoiseSurfacing;

        public void DrawZipline()
        {
            Vector2 PylonBegin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
            Vector2 PylonEnd = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(GetTexture("EEMod/Items/ZipCarrier2"), Main.LocalPlayer.position.ForDraw() + new Vector2(0, 6), new Rectangle(0, 0, 2, 16), Color.White, 0, new Vector2(2, 16) / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(GetTexture("EEMod/Items/ZipCarrier"), Main.LocalPlayer.position.ForDraw(), new Rectangle(0, 0, 18, 8), Color.White, (PylonEnd - PylonBegin).ToRotation(), new Vector2(18, 8) / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.End();
        }

        public void TestParticleSystem()
        {
            
        }

        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            HandwritingCNN = null;
            Noise2D = null;
            RuneActivator = null;
            Inspect = null;
            RuneSpecial = null;
            simpleGame = null;
            ActivateVerletEngine = null;
            Train = null;
            NoiseSurfacing = null;
            White = null;
            UnloadIL();
            UnloadDetours();
            UnloadUI();
            AutoloadingManager.UnloadManager(this);
            instance = null;
            Noise2DShift = null;
        }


        private int delay;
        private float pauseShaderTImer;
        public SpaceInvaders simpleGame;

        public ModPacket GetPacket(EEMessageType type, int capacity)
        {
            ModPacket packet = GetPacket(capacity + 1);
            packet.Write((byte)type);
            return packet;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EENet.ReceievePacket(reader, whoAmI);
        }

        private int lerps;
        private float alphas;
        private int delays;
        private readonly Verlet verlet = new Verlet();
        private bool mode;

        public void UpdateVerlet()
        {
            ScTex = Main.screenTarget;
            if (ActivateVerletEngine.JustPressed)
            {
                mode = !mode;
            }
            if (mode)
            {
                verlet.Update();
            }

            verlet.GlobalRenderPoints();
            /*if (Main.LocalPlayer.controlUp && delays == 0)
            {
                if (Verlet.points.Count == 0)
                {
                    verlet.CreateVerletPoint(Main.MouseWorld);
                }
                else
                {
                      int a = verlet.CreateVerletPoint(Main.MouseWorld);
                     verlet.BindPoints(a - 1, a);
                }
              //  verlet.CreateStickMan(Main.MouseWorld);
                delays = 20;
            }*/
            /*if (Main.LocalPlayer.controlUseItem && delays == 0)
            {
                if (Verlet.points.Count == 0)
                {
                    verlet.CreateVerletPoint(Main.MouseWorld, true);
                }
                else
                {
                    int a = verlet.CreateVerletPoint(Main.MouseWorld, true);
                    verlet.BindPoints(a - 1, a);
                }
                delays = 20;
            }*/
            if (Main.LocalPlayer.controlHook && delays == 0)
            {
                verlet.ClearPoints();
                delays = 20;
            }
        }

        float counter;
        public static Vector2[,,] lol1 = new Vector2[3, 200, 2];


        public void UpdateGame(GameTime gameTime)
        {
            lerps++;
            if (delays > 0)
            {
                delays--;
            }
            float lerpLol = Math.Abs((float)Math.Sin(lerps / 50f));
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if ((npc.type == ModContent.NPCType<OrbCollection>() || npc.type == ModContent.NPCType<SpikyOrb>()) && npc.active)
                {
                    float Dist = Vector2.Distance(npc.Center, Main.LocalPlayer.Center);
                    if (Dist < 1000)
                    {
                        if (!Main.LocalPlayer.GetModPlayer<EEPlayer>().isPickingUp)
                        {
                            UIText("Pick Up?", Color.White * alphas, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 50), 1);
                        }
                        if (Dist < 100)
                        {
                            if (alphas < 1)
                            {
                                alphas += 0.01f;
                            }

                            if (Inspect.JustPressed && delays == 0)
                            {
                                var modp = Main.LocalPlayer.GetModPlayer<EEPlayer>();
                                if (!modp.isPickingUp)
                                    npc.ai[1] = Main.myPlayer;
                                modp.isPickingUp = !modp.isPickingUp;
                                delays = 120;
                            }
                        }
                        else
                        {
                            if (alphas > 0)
                            {
                                alphas -= 0.01f;
                            }
                        }
                    }
                }
            }
            simpleGame = simpleGame ?? new SpaceInvaders();
            simpleGame.Update(gameTime);
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (Inspect.JustPressed && player.GetModPlayer<EEPlayer>().playingGame == true)
                    {
                        player.GetModPlayer<EEPlayer>().playingGame = false;
                        player.webbed = false;
                        simpleGame.EndGame();
                        break;
                    }
                    if (Inspect.JustPressed && Main.tile[(int)player.Center.X / 16, (int)player.Center.Y / 16].type == ModContent.TileType<BlueArcadeMachineTile>() && player.GetModPlayer<EEPlayer>().playingGame == false && PlayerExtensions.GetSavings(player) >= 2500)
                    {
                        simpleGame = new SpaceInvaders();
                        Main.PlaySound(SoundID.CoinPickup, Main.LocalPlayer.Center);
                        player.BuyItem(2500);
                        simpleGame.StartGame(i);
                        player.GetModPlayer<EEPlayer>().playingGame = true;
                        break;
                    }
                }
            }
        }


        public override void MidUpdateProjectileItem()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                trailManager.UpdateTrails();
                prims.UpdateTrails();
            }
            EEPlayer.UpdateOceanMapElements();
        }

        //internal void ShowMyUI()
        //{
        //    SpeedrunnTimer?.SetState(RunUI);
        //}

        //internal void HideMyUI()
        //{
        //    SpeedrunnTimer?.SetState(null);
        //}

        public static Effect Noise2D;
        public static Effect White;
        UIManager UI;
        public override void PreUpdateEntities()
        {
            base.PreUpdateEntities();
            Particles.Update();
        }
        public override void Load()
        {

            UI = new UIManager();
            Noise2D = GetEffect("Effects/Noise2D");
            HandwritingCNN = new Handwriting();
            instance = this;
            RuneActivator = RegisterHotKey("Rune UI", "Z");
            RuneSpecial = RegisterHotKey("Activate Runes", "V");
            Inspect = RegisterHotKey("Inspect", "E");
            ActivateVerletEngine = RegisterHotKey("Activate VerletEngine", "N");
            Train = RegisterHotKey("Train Neural Network", "P");
            AutoloadingManager.LoadManager(this);
            //IL.Terraria.IO.WorldFile.SaveWorldTiles += ILSaveWorldTiles;
            if (!Main.dedServ)
            {
                Ref<Effect> screenRef3 = new Ref<Effect>(GetEffect("Effects/Ripple"));
                Ref<Effect> screenRef2 = new Ref<Effect>(GetEffect("Effects/SeaTrans"));
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/SunThroughWalls"));
                Filters.Scene["EEMod:Ripple"] = new Filter(new ScreenShaderData(screenRef3, "Ripple"), EffectPriority.High);
                Filters.Scene["EEMod:Ripple"].Load();
                Filters.Scene["EEMod:SeaTrans"] = new Filter(new ScreenShaderData(screenRef2, "SeaTrans"), EffectPriority.High);
                Filters.Scene["EEMod:SeaTrans"].Load();
                Filters.Scene["EEMod:SunThroughWalls"] = new Filter(new ScreenShaderData(screenRef, "SunThroughWalls"), EffectPriority.High);
                Filters.Scene["EEMod:SunThroughWalls"].Load();
                Filters.Scene["EEMod:SavingCutscene"] = new Filter(new SavingSkyData("FilterMiniTower").UseColor(0f, 0.20f, 1f).UseOpacity(0.3f), EffectPriority.High);
                SkyManager.Instance["EEMod:SavingCutscene"] = new SavingSky();
                NoiseSurfacing = GetEffect("Effects/NoiseSurfacing");
                White = GetEffect("Effects/WhiteOutline");
                /*
		  SpeedrunnTimer = new UserInterface();
		  //RunUI.Activate();
		  RunUI = new RunninUI();
		  SpeedrunnTimer.SetState(RunUI);
                */
            }
            LoadUI();
            LoadIL();
            LoadDetours();
            Particles = new ParticleZoneHandler();
            Particles.AddZone("Main", 400);
            Particles.AppendSpawnModule("Main", new SpawnPeriodically(4));
        }

        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;

        public static ModHotKey RuneActivator;
        public static ModHotKey RuneSpecial;
        public static ModHotKey Inspect;
        public static ModHotKey ActivateVerletEngine;
        public static ModHotKey Train;
        private GameTime lastGameTime;
        public UserInterface EEInterface;
        float sineInt;
        bool IsTraining;
        void UpdateNet()
        {
            HandwritingCNN.Draw();
            if (Train.JustPressed)
            {
                IsTraining = !IsTraining;
            }
            if (IsTraining)
            {
                UIText(HandwritingCNN.ERROR.ToString(), Color.White, Main.screenPosition.ForDraw() + new Vector2(50,400), 1);
                for (int i = 0; i < 60; i++)
                {
                    HandwritingCNN.Update();
                }
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            sineInt += 0.003f;
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            OnModifyInterfaceLayers?.Invoke(layers, mouseTextIndex, lastGameTime);
            if (mouseTextIndex != -1)
            {
                LegacyGameInterfaceLayer EEInterfaceLayer = new LegacyGameInterfaceLayer("EEMod: EEInterface",
                delegate
                {
                    if (lastGameTime != null)
                    {
                        UI.Draw(lastGameTime);
                        Particles.Draw();
                        TestParticleSystem();
                        //UpdateNet();
                        UpdateGame(lastGameTime);
                        //   UpdateJellyfishTesting();
                         UpdateVerlet();
                        if (Main.worldName == KeyID.CoralReefs)
                        {
                            DrawCR();
                        }
                    }

                    return true;
                }, InterfaceScaleType.UI);
                layers.Insert(mouseTextIndex, EEInterfaceLayer);
            }
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
            {
                DrawZipline();
            }

            if (Main.worldName == KeyID.Sea)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    var layer = layers[i];
                    //Remove Resource bars
                    if (layer.Name.Contains("Vanilla: Resource Bars") || layer.Name.Contains("Vanilla: Info Accessories Bar"))
                    {
                        layers.RemoveAt(i);
                    }
                }
            }

            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            var computerState = new LegacyGameInterfaceLayer("EE: UI",
            delegate
            {
                Ascension();
                if (Main.worldName == KeyID.Sea)
                {
                    SeamapUpdates.UpdateShipMovement();
                    SeamapRender.Render();
                    DrawSubText();
                }
                if (Main.worldName == KeyID.Pyramids || Main.worldName == KeyID.Sea || Main.worldName == KeyID.CoralReefs)
                {
                    DrawText();
                }

                return true;
            },
            InterfaceScaleType.UI);
            layers.Insert(textLayer, computerState);
            /*if (mouseTextIndex != -1)
		    {
		        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
		        "SpeedrunTimer: SpeedrunnTimer",
		        delegate
		        {
		            if (_lastUpdateUiGameTime != null && SpeedrunnTimer?.CurrentState != null)
		            {
			            SpeedrunnTimer.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
		            }
		            return true;
		        },
		        InterfaceScaleType.UI));
		    }*/
        }

        public string text;
        public static int AscentionHandler;
        public static int startingTextHandler;
        public static bool isAscending;

        public static void UIText(string text, Color colour, Vector2 position, int style)
        {
            DynamicSpriteFont font = style == 0 ? Main.fontDeathText : Main.fontMouseText;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            //float textPositionRight = position.X + textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public void DrawCR()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            var Bubbles = modPlayer.bubbles;
            for (int i = 0; i < Bubbles.Count; i++)
            {
                Color drawColour = Lighting.GetColor((int)Bubbles[i].Position.X / 16, (int)Bubbles[i].Position.Y / 16);
                Main.spriteBatch.Draw(EEMod.instance.GetTexture("ForegroundParticles/Bob1"), Bubbles[i].Position.ForDraw(), null, drawColour * Bubbles[i].alpha, Bubbles[i].Velocity.ToRotation() + Bubbles[i].rotation, Vector2.Zero, Bubbles[i].scale, SpriteEffects.None, 0);
            }
        }

        private void Ascension()
        {
            float seperation = 400;
            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (EEPlayer.startingText)
            {
                float alpha;
                startingTextHandler++;
                if (startingTextHandler % seperation <= (seperation / 2) && startingTextHandler > seperation)
                {
                    alpha = (float)Math.Sin(startingTextHandler % seperation / (seperation / 2) * Math.PI);
                }
                else
                {
                    alpha = 0;
                }
                Color color = Color.White;
                if (startingTextHandler < seperation * 2)
                {
                    text = "Im too weak";
                }
                else if (startingTextHandler < seperation * 3)
                {
                    text = "Haha Funny Sans Go Burr";
                }
                else if (startingTextHandler < seperation * 4)
                {
                    text = "Sans Slime was too much";
                }
                else
                {
                    text = "Go to the world and avenge me ples ok? Thx bye";
                }
                color *= alpha;
                Vector2 textSize = Main.fontDeathText.MeasureString(text);
                float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                //float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (isAscending)
            {
                float alpha;
                AscentionHandler++;
                if (AscentionHandler % seperation <= (seperation / 2) && AscentionHandler > seperation)
                {
                    alpha = (float)Math.Sin(AscentionHandler % seperation / (seperation / 2) * Math.PI);
                }
                else
                {
                    alpha = 0;
                }
                Color color = Color.Black;
                if (AscentionHandler < seperation * 2)
                {
                    text = "You have discovered your first rune";
                }
                else if (AscentionHandler < seperation * 3)
                {
                    text = "Be wary, many more remain";
                }
                else if (AscentionHandler < seperation * 4)
                {
                    text = "Once you collect them all, you will be able to make synergies"; //"Collect runes for synergies" This is technically wrong since you can only get synergies after collecting them all
                }
                else
                {
                    text = "Good luck.";
                }
                color *= alpha;
                Vector2 textSize = Main.fontDeathText.MeasureString(text);
                float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                //float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }

        private void DrawText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.titleText;
            Color color = Color.White * alpha;
            /*if (Main.worldName == KeyID.Sea)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }*/
            if (Main.ActiveWorldFileData.Name == KeyID.Pyramids)
            {
                text = "The Pyramids";
                color = Color.Yellow * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                text = "The Coral Reefs";
                color = Color.Blue * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoIsland)
            {
                text = "The Volcano";
                color = Color.OrangeRed * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoInside)
            {
                text = "The Volcano's Core";
                color = Color.Red * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Island)
            {
                text = "Tropical Island";
                color = Color.GreenYellow * alpha;
            }
            Texture2D Outline = EEMod.instance.GetTexture("UI/Outline");
            Texture2D Screen = EEMod.instance.GetTexture("Seamap/SeamapAssets/OceanScreen");
            Vector2 textSize = Main.fontDeathText.MeasureString(text);
            float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
            float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
            if (Main.worldName == KeyID.Sea)
                Main.spriteBatch.Draw(EEMod.instance.GetTexture("Seamap/SeamapAssets/OceanScreen"), (Main.screenPosition + new Vector2(Main.screenWidth / 2, 100)).ForDraw(), new Rectangle(0, 0, Screen.Width, Screen.Height), Color.White * alpha, 0, new Rectangle(0, 0, Screen.Width, Screen.Height).Size() / 2, 1, SpriteEffects.None, 0);
            if (Main.worldName == KeyID.Sea)
            {
                Main.spriteBatch.Draw(EEMod.instance.GetTexture("Seamap/SeamapAssets/OceanScreen"), (Main.screenPosition + new Vector2(Main.screenWidth / 2, 100)).ForDraw(), new Rectangle(0, 0, Screen.Width, Screen.Height), Color.White * alpha, 0, new Rectangle(0, 0, Screen.Width, Screen.Height).Size() / 2, 1, SpriteEffects.None, 0);
            }
            else
            {
                Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(Outline, new Vector2(textPositionLeft - 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(Outline, new Vector2(textPositionRight + 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup group0 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gemstones", new int[]
            {
                ItemID.Amber,
                ItemID.Amethyst,
                ItemID.Diamond,
                ItemID.Emerald,
                ItemID.Ruby,
                ItemID.Sapphire,
                ItemID.Topaz
            });
            // Registers the new recipe group with the specified name
            RecipeGroup.RegisterGroup("EEMod:Gemstones", group0);
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemType<SaharaSceptoid>(), 1);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.CrystalSerpent, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemType<QuartzicLifeFragment>(), 1);
            recipe.AddIngredient(ItemID.Gel, 25);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(ItemID.SlimeStaff, 1);
            recipe.AddRecipe();
        }*/
    }
}
