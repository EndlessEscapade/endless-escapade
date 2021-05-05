using EEMod.Autoloading;
using EEMod.Effects;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs.CoralReefs;
using EEMod.Skies;
using EEMod.UI.States;
using EEMod.Tiles.Furniture;
using EEMod.VerletIntegration;
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
using EEMod.Seamap.SeamapContent;
using EEMod.MachineLearning;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Items.Dyes;
using EEMod.Prim;
using Terraria.DataStructures;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;
        public static ModHotKey RuneActivator;
        public static ModHotKey RuneSpecial;
        public static ModHotKey Inspect;
        public static ModHotKey ActivateVerletEngine;
        public static ModHotKey Train;
        public static Effect Noise2D;
        public static Effect White;
        public static ParticleZoneHandler Particles;
        public static UIManager UI;
        internal static ParticleZone MainParticles;
        private GameTime lastGameTime;
        public UserInterface EEInterface;
        public FishermansLogUI FishermansLogUI;
        public RenderTarget2D playerDrawData;
        public RenderTarget2D playerTarget;
        public RenderTarget2D lightingTarget;
        public ComponentManager<TileObjVisual> TVH;

        public override void PostSetupContent()
        {
        }

        public override void Load()
        {
            TVH = new ComponentManager<TileObjVisual>();
            verlet = new Verlet();
            Terraria.ModLoader.IO.TagSerializer.AddSerializer(new BigCrystalSerializer());
            Terraria.ModLoader.IO.TagSerializer.AddSerializer(new EmptyTileEntitySerializer());
            Terraria.ModLoader.IO.TagSerializer.AddSerializer(new CrystalSerializer());
            if (!Main.dedServ)
            {
                playerDrawData = new RenderTarget2D(Main.graphics.GraphicsDevice, 500, 500);
                lightingTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 16, Main.screenHeight / 16);
                playerTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 100, 100);
                UI = new UIManager();
                FishermansLogUI = new FishermansLogUI();
                FishermansLogUI.Activate();
                UI.AddInterface("EEInterfacee");
                UI.AddUIState("FishermansLogUI", FishermansLogUI);
                Noise2D = GetEffect("Effects/Noise2D");
                primitives = new PrimTrailManager();
            }
            //HandwritingCNN = new Handwriting();

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
                Ref<Effect> MyTestShader = new Ref<Effect>(GetEffect("Effects/MyTestShader"));
                Filters.Scene["EEMod:Ripple"] = new Filter(new ScreenShaderData(screenRef3, "Ripple"), EffectPriority.High);
                Filters.Scene["EEMod:Ripple"].Load();
                Filters.Scene["EEMod:SeaTrans"] = new Filter(new ScreenShaderData(screenRef2, "SeaTrans"), EffectPriority.High);
                Filters.Scene["EEMod:SeaTrans"].Load();
                Filters.Scene["EEMod:SunThroughWalls"] = new Filter(new ScreenShaderData(screenRef, "SunThroughWalls"), EffectPriority.High);
                Filters.Scene["EEMod:SunThroughWalls"].Load();
                Filters.Scene["EEMod:SavingCutscene"] = new Filter(new SavingSkyData("FilterMiniTower").UseColor(0f, 0.20f, 1f).UseOpacity(0.3f), EffectPriority.High);
                Filters.Scene["EEMod:MyTestShader"] = new Filter(new ScreenShaderData(MyTestShader, "MyTestShaderFlot"), EffectPriority.High);
                Filters.Scene["EEMod:MyTestShader"].Load();

                GameShaders.Misc["EEMod:SpireHeartbeat"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/SpireShine")), "SpireHeartbeat").UseImage("Noise/WormNoisePixelated");

                SkyManager.Instance["EEMod:SavingCutscene"] = new SavingSky();
                NoiseSurfacing = GetEffect("Effects/NoiseSurfacing");
                White = GetEffect("Effects/WhiteOutline");


                Ref<Effect> hydrosDye = new Ref<Effect>(GetEffect("Effects/HydrosDye"));
                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(hydrosDye, "HydrosDyeShader"));
                Ref<Effect> aquamarineDye = new Ref<Effect>(GetEffect("Effects/AquamarineDye"));
                GameShaders.Armor.BindShader(ModContent.ItemType<HydrosDye>(), new ArmorShaderData(aquamarineDye, "AquamarineDyeShader"));

                /*
                  SpeedrunnTimer = new UserInterface();
                  //RunUI.Activate();
                  RunUI = new RunninUI();
                  SpeedrunnTimer.SetState(RunUI);
                */

                if (Main.netMode != NetmodeID.Server)
                {
                    trailManager = new TrailManager(this);
                    prims = new Prims(this);
                    primitives.CreateTrail(new RainbowLightTrail(null));

                    //primitives.CreateTrail(new RainbowLightTrail(null));
                    prims.CreateVerlet();
                }
                LoadUI();
            }
            LoadIL();
            LoadDetours();
            if (!Main.dedServ)
            {
                Particles = new ParticleZoneHandler();
                Particles.AddZone("Main", 40000);
                MainParticles = Particles.Get("Main");
            }

            InitializeAmbience();
        }

        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            //HandwritingCNN = null;
            PrismShader = null;
            SpireShader = null;
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
            Noise2DShift = null;
            Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
            Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
            Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
            Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
            Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EENet.ReceievePacket(reader, whoAmI);
        }

        public override void MidUpdateProjectileItem()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                trailManager.UpdateTrails();
                prims.UpdateTrails();
                primitives.UpdateTrailsAboveTiles();
            }
            MechanicManager.MidUpdateProjectileItem();
            EEPlayer.UpdateOceanMapElements();
            Seamap.SeamapContent.Seamap.UpdateShipMovement();
        }
        public override void MidUpdateNPCGore()
        {
            MechanicManager.MidUpdateNPCGore();
        }
        public override void MidUpdateDustTime()
        {
            MechanicManager.MidUpdateDustTime();
        }

        public override void PreUpdateEntities()
        {
            RenderTargetBinding[] oldtargets1 = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(lightingTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            int Width = Main.screenWidth;
            int Height = Main.screenHeight;
            for (int i = 0; i < Width / 16; i++)
            {
                for (int j = 0; j < Height / 16; j++)
                {
                    Vector2 SP = Main.screenPosition / 16;
                    Point p = new Point((int)SP.X + i, (int)SP.Y + j);
                    Color c = Lighting.GetColor(p.X, p.Y);
                    Main.spriteBatch.Draw(Main.magicPixel, new Rectangle(i, j, 1, 1), c);
                }
            }
            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets1);
            LightingBufferEffect.Parameters["buffer"].SetValue(lightingTarget);
            RenderTargetBinding[] oldtargets2 = Main.graphics.GraphicsDevice.GetRenderTargets();
            Main.graphics.GraphicsDevice.SetRenderTarget(playerDrawData);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin();
            for (int i = 0; i <= Main.playerDrawData.Count; i++)
            {
                int num = -1;
                if (num != 0)
                {
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                    num = 0;
                }

                if (i != Main.playerDrawData.Count)
                {
                    DrawData value = Main.playerDrawData[i];
                    if (value.shader >= 0)
                    {
                        GameShaders.Hair.Apply(0, Main.LocalPlayer, value);
                        GameShaders.Armor.Apply(value.shader, Main.LocalPlayer, value);
                    }
                    else if (Main.LocalPlayer.head == 0)
                    {
                        GameShaders.Hair.Apply(0, Main.LocalPlayer, value);
                        GameShaders.Armor.Apply(Main.LocalPlayer.cHead, Main.LocalPlayer, value);
                    }
                    else
                    {
                        GameShaders.Armor.Apply(0, Main.LocalPlayer, value);
                        GameShaders.Hair.Apply((short)(-value.shader), Main.LocalPlayer, value);
                    }
                    if (!value.sourceRect.HasValue)
                    {
                        value.sourceRect = value.texture.Frame();
                    }
                    num = value.shader;
                    if (value.texture != null)
                    {
                        Main.spriteBatch.Draw(value.texture, value.position - Main.LocalPlayer.position.ForDraw() + playerDrawData.TextureCenter() / 2, value.sourceRect, Color.White, value.rotation, value.origin, value.scale, value.effect, 0f);
                    }
                }
            }
            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets2);
            base.PreUpdateEntities();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            sineInt += 0.003f;
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                LegacyGameInterfaceLayer EEInterfaceLayer = new LegacyGameInterfaceLayer("EEMod: EEInterface", delegate
                {
                    if (lastGameTime != null)
                    {
                        UI.Draw(lastGameTime);
                        //UpdateNet();
                        UpdateGame(lastGameTime);
                        AfterTiles?.Invoke(Main.spriteBatch);
                        //UpdateSpiderPort();
                        //   UpdateJellyfishTesting();
                        UpdateVerlet();
                        if (Main.worldName == KeyID.CoralReefs)
                        {
                            DrawCR();
                        }
                    }

                    return true;
                }, InterfaceScaleType.Game);
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
                    if (layer.Name.Contains("Vanilla: Resource Bars") || layer.Name.Contains("Vanilla: Info Accessories Bar") || layer.Name.Contains("Vanilla: Map / Minimap") || layer.Name.Contains("Vanilla: Inventory"))
                    {
                        layers.RemoveAt(i);
                    }
                }
            }

            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (textLayer != -1)
            {
                var computerState = new LegacyGameInterfaceLayer("EE: UI", delegate
                {
                    Ascension();
                    if (Main.worldName == KeyID.Pyramids || Main.worldName == KeyID.Sea || Main.worldName == KeyID.CoralReefs)
                    {
                        DrawText();
                    }
                    return true;
                },
                InterfaceScaleType.UI);
                layers.Insert(textLayer, computerState);
            }
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

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (!Main.gameMenu)
            {
                Player player = Main.LocalPlayer;
                EEPlayer eeplayer = player?.GetModPlayer<EEPlayer>();
                if (eeplayer != null)
                {
                    int length = eeplayer.reefMinibiome.Length;

                    if (Main.worldName == KeyID.CoralReefs)
                    {
                        if (Main.LocalPlayer.Center.Y < ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                        {
                            music = GetSoundSlot(SoundType.Music, "Sounds/Music/SurfaceReefs");
                            priority = MusicPriority.Environment;
                        }

                        if (Main.LocalPlayer.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16 && Main.LocalPlayer.Center.Y < (Main.maxTilesY / 10) * 4 * 16)
                        {
                            music = GetSoundSlot(SoundType.Music, "Sounds/Music/UpperReefs");
                            priority = MusicPriority.Environment;
                        }

                        if (Main.LocalPlayer.Center.Y >= ((Main.maxTilesY / 10) * 4) * 16 && Main.LocalPlayer.Center.Y < (Main.maxTilesY / 10) * 7 * 16)
                        {
                            music = GetSoundSlot(SoundType.Music, "Sounds/Music/LowerReefs");
                            priority = MusicPriority.Environment;
                        }

                        if ((int)MinibiomeID.KelpForest < length)
                        {
                            if (eeplayer.reefMinibiome[(int)MinibiomeID.KelpForest])
                            {
                                music = GetSoundSlot(SoundType.Music, "Sounds/Music/KelpForest");
                                priority = MusicPriority.BiomeHigh;
                            }
                        }

                        if ((int)MinibiomeID.CrystallineCaves < length)
                        {
                            if (eeplayer.reefMinibiome[(int)MinibiomeID.CrystallineCaves])
                            {
                                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Aquamarine");
                                priority = MusicPriority.BiomeHigh;
                            }
                        }

                        if ((int)MinibiomeID.GlowshroomGrotto < length)
                        {
                            if (eeplayer.reefMinibiome[(int)MinibiomeID.GlowshroomGrotto])
                            {
                                music = GetSoundSlot(SoundType.Music, "Sounds/Music/GlowshroomGrotto");
                                priority = MusicPriority.BiomeHigh;
                            }
                        }

                        /*if ((int)MinibiomeID.ThermalVents < length)
                        {
                            if (eeplayer.reefMinibiome[(int)MinibiomeID.ThermalVents])
                            {
                                music = GetSoundSlot(SoundType.Music, "Sounds/Music/ThermalVents");
                                priority = MusicPriority.BiomeHigh;
                            }
                        }*/
                    }

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.modNPC is AquamarineSpire spire)
                        {
                            if (spire.awake)
                            {
                                music = GetSoundSlot(SoundType.Music, "Sounds/Music/AquamarineSpire");
                                priority = MusicPriority.BossLow;
                            }
                        }
                    }
                }

                if (Main.worldName == KeyID.Sea)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/Seamap");
                    priority = MusicPriority.BiomeHigh;
                }
            }
        }
    }
}
