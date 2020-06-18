using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using EEMod.Autoloading;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Tiles;
using EEMod.UI;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static EEMod instance;

        public override void PostSetupContent()
        {

        }
        public static double worldSurface;

        public static double worldSurfaceLow;

        public static double worldSurfaceHigh;

        public static double rockLayer;

        public static double rockLayerLow;

        public static double rockLayerHigh;

        public static int _lastSeed;

        private static WorldGenerator _generator;
        public UserInterface customResources;

        private static void AddGenerationPass(string name, WorldGenLegacyMethod method)
        {
            _generator.Append(new PassLegacy(name, method));
        }
        public static void GenerateWorld(string key, int seed, GenerationProgress customProgressObject = null)
        {
            typeof(EESubWorlds).GetMethod(key).Invoke(null, new object[] { seed, customProgressObject });
        }
        public static void GenerateWorld2(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            Logging.Terraria.InfoFormat("Generating World: {0}", (object)Main.ActiveWorldFileData.Name);
            _lastSeed = seed;
            _generator = new WorldGenerator(seed);
            MicroBiome.ResetAll();

            //WorldHooks.PreWorldGen();
            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                Liquid.ReInit();
                progress.Message = "";
                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                Main.raining = false;
                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();

            });
            Main.worldID = WorldGen.genRand.Next(int.MaxValue);
            //WorldHooks.ModifyWorldGenTasks(_generator._passes, ref _generator._totalLoadWeight);
            _generator.GenerateWorld(customProgressObject);

            EEWorld.EEWorld.FillWall(400, 400, new Vector2(0, 0), WallID.Waterfall);
            //Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
        }
        public static void GenerateWorld3(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 1000;
            Main.maxTilesY = 2000;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            Logging.Terraria.InfoFormat("Generating World: {0}", (object)Main.ActiveWorldFileData.Name);
            _lastSeed = seed;
            _generator = new WorldGenerator(seed);
            MicroBiome.ResetAll();

            //WorldHooks.PreWorldGen();
            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                Liquid.ReInit();
                progress.Message = "";
                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                Main.raining = false;
                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();

            });
            Main.worldID = WorldGen.genRand.Next(int.MaxValue);
            //WorldHooks.ModifyWorldGenTasks(_generator._passes, ref _generator._totalLoadWeight);
            _generator.GenerateWorld(customProgressObject);

            EEWorld.EEWorld.FillRegion(1000, 2000, Vector2.Zero, ModContent.TileType<HardenedGemsandTile>());
            EEWorld.EEWorld.CoralReef();
            //Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
        }



        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            On.Terraria.Main.DoUpdate -= OnUpdate;
            On.Terraria.WorldGen.SaveAndQuitCallBack -= OnSave;
            On.Terraria.Main.DrawMenu -= OnDrawMenu;
            AutoloadingManager.UnloadManager(this);
            instance = null;
        }
        internal EEUI eeui;
        public UserInterface EEInterface;
        private GameTime lastGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            lastGameTime = gameTime;
            if (EEInterface?.CurrentState != null)
            {
                EEInterface.Update(gameTime);
            }
            base.UpdateUI(gameTime);
        }
        public override void Load()
        {
            instance = this;
            AutoloadingManager.LoadManager(this);
            //IL.Terraria.IO.WorldFile.SaveWorldTiles += ILSaveWorldTiles;
            if (!Main.dedServ)
            {
                eeui = new EEUI();
                eeui.Initialize();
                EEInterface = new UserInterface();
                EEInterface.SetState(eeui);
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
            }
            On.Terraria.Main.DoUpdate += OnUpdate;
            On.Terraria.WorldGen.SaveAndQuitCallBack += OnSave;
            On.Terraria.Main.DrawMenu += OnDrawMenu;
        }

        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;
        public void OnSave(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadcontext)
        {
            isSaving = true;
            orig(threadcontext);
            isSaving = false;

            //saveInterface?.SetState(null);

        }
        public void OnUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, GameTime gameTime)
        {
            if (!Main.gameMenu && Main.netMode != NetmodeID.MultiplayerClient && !isSaving)
            {
                loadingChoose = Main.rand.Next(62);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;
                if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                Main.logo2Texture = TextureCache.Terraria_Logo2Texture;
                Main.logoTexture = TextureCache.Terraria_LogoTexture;
                Main.sun2Texture = TextureCache.Terraria_Sun2Texture;
                Main.sun3Texture = TextureCache.Terraria_Sun3Texture;
                Main.sunTexture = TextureCache.Terraria_SunTexture;
                for (int i = 0; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = ModContent.GetTexture("Terraria/Background_" + i);
            }
            orig(self, gameTime);
        }
        private void OnDrawMenu(On.Terraria.Main.orig_DrawMenu orig, Main self, GameTime gameTime)
        {

            position = start;
            velocity = Vector2.Zero;
            if (isSaving)
            {
                for (int i = 0; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = TextureCache.Empty;
                Main.numClouds = 0;
                Main.logo2Texture = TextureCache.Empty;
                Main.logoTexture = TextureCache.Empty;
                Main.sun2Texture = TextureCache.Empty;
                Main.sun3Texture = TextureCache.Empty;
                Main.sunTexture = TextureCache.Empty;
                if (SkyManager.Instance["EEMod:SavingCutscene"] != null) SkyManager.Instance.Activate("EEMod:SavingCutscene", default, new object[0]);
                if (loadingFlag)
                {
                    loadingChoose = Main.rand.Next(62);
                    loadingChooseImage = Main.rand.Next(5);
                    loadingFlag = false;
                }
                switch (loadingChoose)
                {
                    case 0:
                        Main.statusText = "Watch Out! Dune Shamblers Pop from the ground from time to time!";
                        break;
                    case 1:
                        Main.statusText = "Gallagar is Satan!";
                        break;
                    case 2:
                        Main.statusText = "Tip of the Day! Loading screens are useless";
                        break;
                    case 3:
                        Main.statusText = "Fear the MS Paint cat";
                        break;
                    case 4:
                        Main.statusText = "Terraria sprites need outlines... except when I make them";
                        break;
                    case 5:
                        Main.statusText = "Remove the Banding";
                        break;
                    case 6:
                        Main.statusText = Main.LocalPlayer.name + " ....huh...what a cruddy name";
                        break;
                    case 7:
                        Main.statusText = "Dont ping everyone you big dumb stupid";
                        break;
                    case 8:
                        Main.statusText = "I'm nothing without attention";
                        break;
                    case 9:
                        Main.statusText = "Why are you even reading this?";
                        break;
                    case 10:
                        Main.statusText = "We actually think we are funny";
                        break;
                    case 11:
                        Main.statusText = "Interitos...whats that?";
                        break;
                    case 12:
                        Main.statusText = "its my style";
                        break;
                    case 13:
                        Main.statusText = "Now featuring 50% more monkey per chimp!";
                        break;
                    case 14:
                        Main.statusText = "im angy";
                        break;
                    case 15:
                        Main.statusText = "Send help, please this is not a joke im actually being held hostage";
                        break;
                    case 16:
                        Main.statusText = "Mod is not edgy I swear!";
                        break;
                    case 17:
                        Main.statusText = "All programmers are cyniks";
                        break;
                    case 18:
                        Main.statusText = "Im gonna have to mute you for that";
                        break;
                    case 19:
                        Main.statusText = "Gamers rise up!";
                        break;
                    case 20:
                        Main.statusText = "THATS NOT THE CONCEPT";
                        break;
                    case 21:
                        Main.statusText = "caramel popcorn and celeste";
                        break;
                    case 22:
                        Main.statusText = "D D D A G# G F D F G";
                        break;
                    case 23:
                        Main.statusText = "We live in a society";
                        break;
                    case 24:
                        Main.statusText = "Dont mine at night!";
                        break;
                    case 25:
                        Main.statusText = "deleting system32...";
                        break;
                    case 26:
                        Main.statusText = "Sans in real!";
                        break;
                    case 27:
                        Main.statusText = "I sure hope I didnt break the codeghsduighshsy";
                        break;
                    case 28:
                        Main.statusText = "If you liked endless escapade you will love endless escapade premium!";
                        break;
                    case 29:
                        Main.statusText = "IL edit that breaks the runtime";
                        break;
                    case 30:
                        Main.statusText = "mario in real life";
                        break;
                    case 31:
                        Main.statusText = "the 𝖕𝖍𝖆𝖓𝖙𝖆m of the opera";
                        break;
                    case 32:
                        Main.statusText = "EEMod Foretold? More like doesn't exist";
                        break;
                    case 33:
                        Main.statusText = "You think this is a game? Look behind you 0_0";
                        break;
                    case 34:
                        Main.statusText = "Respect the drip Karen";
                        break;
                    case 35:
                        Main.statusText = "trust me there is a lot phesh down in here, the longer the player is in the reefs the more amphibious he will become";
                        break;
                    case 36:
                        Main.statusText = "All good sprites made by daimgamer!";
                        break;
                    case 37:
                        Main.statusText = "All (good) music made by Universe";
                        break;
                    case 38:
                        Main.statusText = "All good sprites made by Vadim";
                        break;
                    case 39:
                        Main.statusText = "All good sprites made by Zarn";
                        break;
                    case 40:
                        Main.statusText = "ni-𝕌ℕ𝕀𝕍𝔼ℝ𝕊𝔼";
                        break;
                    case 41:
                        Main.statusText = "Totally not copying Starlight River";
                        break;
                    case 42:
                        Main.statusText = "Do a Barrel Roll";
                        break;
                    case 43:
                        Main.statusText = "The man behind the laughter";
                        break;
                    case 44:
                        Main.statusText = "We all eventually die!";
                        break;
                    case 45:
                        Main.statusText = "An apple a day keeps the errors away!";
                        break;
                    case 46:
                        Main.statusText = "Poggers? Poggers.";
                        break;
                    case 47:
                        Main.statusText = $@"Totally not sentient AI. By the way, {Environment.UserName} is a dumb computer name";
                        break;
                    case 48:
                        Main.statusText = "Ḯ̴͝t̶͐̈́ ̶̄͆ȁ̷͠l̶̄̑l̵̇͝ ̴̀̎e̶͌̌n̶̍̓d̵͋̂s̶̑̃ ̵͊̉ẻ̶̛v̶̍̍ë̴́́n̶͗͠t̷́͘u̵͒̆à̶̎l̷̊͗l̶͊̍y̴̌̋!̴͂̑";
                        break;
                    case 49:
                        Main.statusText = "Illegal in 5 countries!";
                        break;
                    case 50:
                        Main.statusText = "Inside jokes you wont understand!";
                        break;
                    case 51:
                        Main.statusText = "Big content mod bad!";
                        break;
                    case 52:
                        Main.statusText = "Loading the random chimp event...";
                        break;
                    case 53:
                        Main.statusText = "Sending you to the Aether...";
                        break;
                    case 54:
                        Main.statusText = "When";
                        break;
                    case 55:
                        Main.statusText = "[Insert non funny joke here]";
                        break;
                    case 56:
                        Main.statusText = "The dev server is indeed an asylum";
                        break;
                    case 57:
                        Main.statusText = "owo";
                        break;
                    case 58:
                        Main.statusText = "That's how the mafia works";
                        break;
                    case 59:
                        Main.statusText = "Hacking the mainframe...";
                        break;
                    case 60:
                        Main.statusText = "Not Proud";
                        break;
                    case 61:
                        Main.statusText = "You know I think the ocean needs more con- Haha the literal ocean goes brr";
                        break;
                }
            }
            else
            {
                loadingChoose = 47;//Main.rand.Next(62);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;
                if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                Main.logo2Texture = TextureCache.Terraria_Logo2Texture;
                Main.logoTexture = TextureCache.Terraria_LogoTexture;
                Main.sun2Texture = TextureCache.Terraria_Sun2Texture;
                Main.sun3Texture = TextureCache.Terraria_Sun3Texture;
                Main.sunTexture = TextureCache.Terraria_SunTexture;
                for (int i = 0; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = ModContent.GetTexture("Terraria/Background_" + i);
            }
            orig(self, gameTime);
        }

        /*private static void ILSaveWorldTiles(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            PropertyInfo statusText = typeof(Main).GetProperty(nameof(Main.statusText));
            MethodInfo set = statusText.GetSetMethod();

            if (!c.TryGotoNext(i => i.MatchCall(set)))
                throw new Exception();

            c.EmitDelegate<Func<string, string>>((originalText) =>
            {
                return originalText;
            });
        }*/

        internal void ShowMyUI()
        {
            EEInterface?.SetState(eeui);
        }

        internal void HideMyUI()
        {
            EEInterface?.SetState(null);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MyMod: MyInterface",
                    delegate
                    {
                        if (lastGameTime != null && EEInterface?.CurrentState != null)
                        {
                            // EEInterface.Draw(Main.spriteBatch, lastGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
            if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    //Remove Resource bars
                    if (layers[i].Name.Contains("Vanilla: Resource Bars"))
                    {
                        layers.RemoveAt(i);
                    }
                }
                for (int i = 0; i < layers.Count; i++)
                {
                    //Remove Resource bars
                    if (layers[i].Name.Contains("Vanilla: Info Accessories Bar"))
                    {
                        layers.RemoveAt(i);
                    }
                }
            }
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            var computerState = new LegacyGameInterfaceLayer("EE: UI",
                delegate
                {
                    if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
                    {
                        DrawSubText();
                        DrawShip();
                    }
                    if (Main.ActiveWorldFileData.Name == EEPlayer.key1 || Main.ActiveWorldFileData.Name == EEPlayer.key2 || Main.ActiveWorldFileData.Name == EEPlayer.key3)
                        DrawText();
                    return true;
                },
                InterfaceScaleType.UI);
            layers.Insert(textLayer, computerState);
        }
        public string text;
        private void DrawText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.titleText;
            Color color = Color.White * alpha;
            if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }
            if (Main.ActiveWorldFileData.Name == EEPlayer.key1)
                text = "The Pyramids";
            if (Main.ActiveWorldFileData.Name == EEPlayer.key3)
                text = "The Coral Reefs";
            Texture2D Outline = TextureCache.Outline;
            Vector2 textSize = Main.fontDeathText.MeasureString(text);
            float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
            float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
            Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Outline, new Vector2(textPositionLeft - 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Outline, new Vector2(textPositionRight + 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.FlipHorizontally, 0);
        }
        Texture2D texture;
        Rectangle frame;
        int Countur;
        int frames;
        int frameSpeed;
        public static float ShipHelthMax = 100;
        public static float ShipHelth = 100;
        public static Vector2 position;
        public static Vector2 velocity;
        public static readonly Vector2 start = new Vector2(1700, 900);
        private void DrawSubText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.subText;
            Color color = Color.White;
            if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
            {
                text = "Disembark?";
                color *= alpha;
            }
            Vector2 textSize = Main.fontMouseText.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(textPositionLeft, position.Y + 20), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
        private void DrawShip()
        {
            Player modPlayer = Main.LocalPlayer;
            if (!Main.gamePaused)
            {
                position += velocity;
                if (modPlayer.controlJump)
                {
                    velocity.Y -= 0.1f;
                }
                if (modPlayer.controlDown)
                {
                    velocity.Y += 0.1f;
                }
                if (modPlayer.controlRight)
                {
                    velocity.X += 0.1f;
                }
                if (modPlayer.controlLeft)
                {
                    velocity.X -= 0.1f;
                }
            }
            if (velocity.X > 1)
                velocity.X = 1;
            if (velocity.X < -1)
                velocity.X = -1;
            if (velocity.Y > 1)
                velocity.Y = 1;
            if (velocity.Y < -1)
                velocity.Y = -1;
            // Mod mod = EEMod.instance;
            texture = TextureCache.ShipMount;
            frames = 1;
            frameSpeed = 15;
            if (Countur++ > frameSpeed)
            {
                Countur = 0;
                frame.Y += texture.Height / frames;
            }
            if (frame.Y >= (texture.Height / frames) * (frames - 1))
            {
                frame.Y = 0;
            }
            if (!Main.gamePaused)
            {
                velocity *= 0.98f;
            }
            //Dust.NewDust(Main.screenPosition + position, 1, 1, DustID.BlueCrystalShard);
            //Lighting.AddLight(new Vector2(Main.screenPosition.X + Main.screenWidth, Main.screenPosition.Y + Main.screenHeight), 1f, 1f, 1f);
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 100; j++)
                {

                }
            }
            for (int i = 0; i < EEPlayer.objectPos.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                    Lighting.AddLight(EEPlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 1)
                    Lighting.AddLight(EEPlayer.objectPos[i], .15f, .15f, .15f);
            }

            Texture2D texture3 = TextureCache.ShipHelth;
            Lighting.AddLight(Main.screenPosition + position, .1f, .1f, .1f);
            float quotient = ShipHelth / ShipHelthMax;
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 100, 100), new Rectangle(0, 0, (int)(texture3.Width * quotient), texture3.Height), Color.White, 0, new Rectangle(0, 0, texture3.Width, texture3.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height / frames), Color.White, velocity.X / 10, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            /*  Main.spriteBatch.End();
              Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

              Main.spriteBatch.End();
              Main.spriteBatch.Begin();*/
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ModContent.ItemType<SaharaSceptoid>(), 1);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.CrystalSerpent, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ModContent.ItemType<QuartzicLifeFragment>(), 1);
            recipe.AddIngredient(ItemID.Gel, 25);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(ItemID.SlimeStaff, 1);
            recipe.AddRecipe();
        }
    }
}
