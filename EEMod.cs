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
using EEMod.Projectiles.Mage;
using EEMod.Projectiles.OceanMap;
using System.IO;
using System.Threading;
using Terraria.IO;
using EEMod.Projectiles;
using EEMod.Projectiles.CoralReefs;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using EEMod.NPCs.Bosses.Hydros;
using static Terraria.ModLoader.ModContent;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.NPCs.Friendly;
using EEMod.Items;

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

        public void DrawZipline()
        {
            Vector2 PylonBegin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
            Vector2 PylonEnd = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Items/ZipCarrier2"), Main.LocalPlayer.Center - Main.screenPosition + new Vector2(0, -26), new Rectangle(0, 0, 2, 16), Color.White, 0, new Rectangle(0, 0, 2, 16).Size() / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Items/ZipCarrier"), Main.LocalPlayer.Center - Main.screenPosition + new Vector2(0, -32), new Rectangle(0, 0, 18, 8), Color.White, (PylonEnd - PylonBegin).ToRotation(), new Rectangle(0, 0, 18, 8).Size() / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.End();
        }

        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            RuneActivator = null;
            RuneSpecial = null;
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
            RuneSpecial = RegisterHotKey("Activate Runes", "V");
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
        public static ModHotKey RuneSpecial;

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

            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
                DrawZipline();

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
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }
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
        int frames;
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
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
            {
                text = "Disembark?";
                color *= alpha;
            }
            Vector2 textSize = Main.fontMouseText.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(textPositionLeft, position.Y + 20), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
        float flash = 0;
        float markerPlacer = 0;
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
        private void DrawShip()
        {
            markerPlacer++;
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
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
                if (player.controlUseItem && cannonDelay <= 0)
                {
                    switch (eePlayer.cannonballType)
                    {
                        case 0:
                            break;
                        case 1:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyCannonball>(), 0, 0);
                            break;
                        case 2:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyExplosiveCannonball>(), 0, 0);
                            break;
                        case 3:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyHallowedCannonball>(), 0, 0);
                            break;
                        case 4:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyChlorophyteCannonball>(), 0, 0);
                            break;
                        case 5:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyLuminiteCannonball>(), 0, 0);
                            break;
                    }
                    Main.PlaySound(SoundID.Item61);
                    cannonDelay = 60;
                }
                cannonDelay--;
            }
            velocity.X = Helpers.Clamp(velocity.X, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            velocity.Y = Helpers.Clamp(velocity.Y, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            texture = TextureCache.ShipMount;

            frames = 12;
            int frameNum = 0;
            if (Main.netMode == NetmodeID.SinglePlayer || ((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 0))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 1;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 0;
            }
            if (((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 1))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 3;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 2;
            }
            if (((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 2))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 9;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 8;
            }
            if (((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 3))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 5;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 4;
            }
            if (((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 4))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 7;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 6;
            }
            if (((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 5))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 11;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 10;
            }


            if (!Main.gamePaused)
            {
                velocity *= 0.98f;
            }
            for (int i = 0; i < eePlayer.objectPos.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 1)
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                if (i == 2)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 4)
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                if (i == 7)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 0)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
            }
            //Lighting.AddLight(eePlayer.objectPos[1], 0.9f, 0.9f, 0.9f);

            Texture2D texture3 = TextureCache.ShipHelth;
            Lighting.AddLight(Main.screenPosition + position, .1f, .1f, .1f);
            float quotient = ShipHelth / ShipHelthMax;
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 175, 50), new Rectangle(0, (int)((texture3.Height / 8) * ShipHelth), texture3.Width, texture3.Height / 8), Color.White, 0, new Rectangle(0, (int)((texture3.Height / 8) * ShipHelth), texture3.Width, texture3.Height / 8).Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), Color.White, velocity.X / 10, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            flash += 0.01f;
            if (flash == 2)
            {
                flash = 10;
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
        }*/
    }
}
