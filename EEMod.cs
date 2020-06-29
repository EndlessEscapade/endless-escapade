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
using System.Reflection.Emit;
using Mono.Cecil.Cil;
using OpCodes = Mono.Cecil.Cil.OpCodes;

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

        public UserInterface customResources;

        public static void GenerateWorld(string key, int seed, GenerationProgress customProgressObject = null)
        {
            typeof(EESubWorlds).GetMethod(key).Invoke(null, new object[] { seed, customProgressObject });
        }
      
        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            RuneActivator = null;
            UnloadIL();
            AutoloadingManager.UnloadManager(this);
            instance = null;
        }
        internal EEUI eeui;
        public UserInterface EEInterface;
        private GameTime lastGameTime;
        int delay;
        float pauseShaderTImer;
        public override void UpdateUI(GameTime gameTime)
        {
            lastGameTime = gameTime;
            if (EEInterface?.CurrentState != null)
            {
                EEInterface.Update(gameTime);
            }
            base.UpdateUI(gameTime);

            if (RuneActivator.JustPressed && delay == 0)
            {
                if (EEUIVisible)
                {
                    EEUIVisible = false;
                    if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Deactivate("EEMod:Pause");
                    }
                }
                else
                {
                    EEUIVisible = true;
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Activate("EEMod:Pause").GetShader().UseOpacity(pauseShaderTImer);
                    }
                }
                delay++;
            }
            if (EEUIVisible)
            {
                Filters.Scene["EEMod:Pause"].GetShader().UseOpacity(pauseShaderTImer);
                pauseShaderTImer += 50;
                if (pauseShaderTImer > 1000)
                {
                    pauseShaderTImer = 1000;
                }
            }
            else
            {
                pauseShaderTImer = 0;
            }
            if (delay > 0)
            {
                delay++;
                if (delay == 60)
                    delay = 0;
            }
        }


        public override void Load()
        {
            RuneActivator = RegisterHotKey("Rune UI", "Z");
            instance = this;
            AutoloadingManager.LoadManager(this);
            //IL.Terraria.IO.WorldFile.SaveWorldTiles += ILSaveWorldTiles;
            if (!Main.dedServ)
            {
                eeui = new EEUI();
                eeui.Activate();
                EEInterface = new UserInterface();
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
            LoadIL();
        }

        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;

        public static ModHotKey RuneActivator;

        internal bool EEUIVisible
        {
            get => EEInterface?.CurrentState != null;
            set => EEInterface?.SetState(value ? eeui : null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                LegacyGameInterfaceLayer EEInterfaceLayer = new LegacyGameInterfaceLayer("EEMod: EEInterface",
                delegate
                {
                    if (lastGameTime != null && EEInterface?.CurrentState != null)
                    {
                        EEInterface.Draw(Main.spriteBatch, lastGameTime);
                    }
                    return true;
                }, InterfaceScaleType.UI);
                layers.Insert(mouseTextIndex, EEInterfaceLayer);
            }

            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
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
                if (Main.ActiveWorldFileData.Name == KeyID.Sea)
                {
                    DrawSubText();
                    DrawShip();
                }
                if (Main.ActiveWorldFileData.Name == KeyID.Pyramids || Main.ActiveWorldFileData.Name == KeyID.Sea || Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
                    DrawText();
                return true;
            },
            InterfaceScaleType.UI);
            layers.Insert(textLayer, computerState);
        }
        public string text;
        public static int AscentionHandler;
        public static int startingTextHandler;
        public static bool isAscending;
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
                    text = "Collect runes for synergies";
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
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Pyramids)
                text = "The Pyramids";
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
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
            float alpha = modPlayer.subTextAlpha;
            Color color = Color.White;
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
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
            Player player = Main.LocalPlayer;
            if (!Main.gamePaused)
            {
                position += velocity;
                if (player.controlJump)
                {
                    velocity.Y -= 0.1f;
                }
                if (player.controlDown)
                {
                    velocity.Y += 0.1f;
                }
                if (player.controlRight)
                {
                    velocity.X += 0.1f;
                }
                if (player.controlLeft)
                {
                    velocity.X -= 0.1f;
                }
            }
            velocity.X = Helpers.Clamp(velocity.X, -1, 1);
            velocity.Y = Helpers.Clamp(velocity.Y, -1, 1);
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
            //for (int i = 0; i < 200; i++)
            //{
            //    for (int j = 0; j < 100; j++)
            //    {

            //    }
            //}
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
