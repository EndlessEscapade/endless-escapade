using EEMod.Autoloading;
using EEMod.Effects;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Items.Dyes;
using EEMod.MachineLearning;
using EEMod.Net;
using EEMod.Prim;
using EEMod.Seamap.Core;
using EEMod.Systems;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Tiles.Furniture;
using EEMod.UI.States;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization; // :sadge:
using Terraria.ModLoader;
using Terraria.UI;// l a g
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader.IO;
using EEMod.ModSystems;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static EEMod Instance => ModContent.GetInstance<EEMod>();
        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;

        public static GameTime lastGameTime;

        public static ModKeybind RuneActivator;
        public static ModKeybind RuneSpecial;
        public static ModKeybind Inspect;
        public static ModKeybind ActivateVerletEngine;

        public static UIManager UI;

        public static Noise2D Noise2D;
        public static ParticleZoneHandler Particles;
        internal static ParticleZone MainParticles;
        public UserInterface EEInterface;
        public FishermansLogUI FishermansLogUI;
        public KelpArmorAmmoUI KelpArmorAmmoUI;
        public IndicatorsUI IndicatorsUI;
        public DialogueUI DialogueUI;
        public ShipLoadoutUI ShipLoadoutUI;
        public ComponentManager<TileObjVisual> TVH;

        public override void Load()
        {
            TVH = new ComponentManager<TileObjVisual>();
            verlet = new Verlet();

            //TagSerializer.AddSerializer(new BigCrystalSerializer());
            //TagSerializer.AddSerializer(new EmptyTileEntitySerializer());
            //TagSerializer.AddSerializer(new CrystalSerializer());

            if (!Main.dedServ)
            {
                UI = new UIManager();
                FishermansLogUI = new FishermansLogUI();
                FishermansLogUI.Activate();
                UI.AddInterface("EEInterfacee");
                UI.AddUIState("FishermansLogUI", FishermansLogUI);

                KelpArmorAmmoUI = new KelpArmorAmmoUI();
                KelpArmorAmmoUI.Activate();
                UI.AddInterface("KelpArmorAmmoInterface");
                UI.AddUIState("KelpArmorAmmoUI", KelpArmorAmmoUI);

                IndicatorsUI = new IndicatorsUI();
                IndicatorsUI.Activate();
                UI.AddInterface("IndicatorsInterface");
                UI.AddUIState("IndicatorsUI", IndicatorsUI);

                DialogueUI = new DialogueUI();
                DialogueUI.Activate();
                UI.AddInterface("DialogueInterface");
                UI.AddUIState("DialogueUI", DialogueUI);

                ShipLoadoutUI = new ShipLoadoutUI();
                ShipLoadoutUI.Activate();
                UI.AddInterface("ShipLoadoutInterface");
                UI.AddUIState("ShipLoadoutUI", ShipLoadoutUI);

                PrimitiveSystem.primitives = new PrimTrailManager();
            }
            //HandwritingCNN = new Handwriting();

            RuneActivator = KeybindLoader.RegisterKeybind(this, "Rune UI", Keys.Z);
            RuneSpecial = KeybindLoader.RegisterKeybind(this, "Activate Runes", Keys.V);
            Inspect = KeybindLoader.RegisterKeybind(this, "Inspect", Keys.OemCloseBrackets);
            ActivateVerletEngine = KeybindLoader.RegisterKeybind(this, "Activate VerletEngine", Keys.N);

            //IL.Terraria.IO.WorldFile.SaveWorldTiles += ILSaveWorldTiles;

            Main.QueueMainThreadAction(() => 
            {
                if (!Main.dedServ)
                {
                    AutoloadingManager.LoadManager(this);

                    /*
                      SpeedrunnTimer = new UserInterface();
                      //RunUI.Activate();
                      RunUI = new RunninUI();
                      SpeedrunnTimer.SetState(RunUI);
                    */

                    if (Main.netMode != NetmodeID.Server)
                    {
                        PrimitiveSystem.trailManager = new TrailManager(this);
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
            });

            //Example
            //LayeredMusic.Groups[GetSoundSlot(SoundType.Music, "Sounds/Music/UpperReefs")] = "AquamarineGroup";
            //LayeredMusic.Groups[GetSoundSlot(SoundType.Music, "Sounds/Music/LowerReefs")] = "AquamarineGroup";

            if(!Main.dedServ)
            PrimitiveSystem.primitives.Load();

            MusicLoader.AddMusic(this, "Assets/Music/SurfaceReefs");
        }

        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            PrismShader = null;
            SpireShine = null;
            Noise2D = null;
            RuneActivator = null;
            Inspect = null;
            RuneSpecial = null;
            simpleGame = null;
            ActivateVerletEngine = null;
            NoiseSurfacing = null;
            WhiteOutline = null;
            Effervescence = null;
            Colorify = null;
            UnloadIL();
            UnloadDetours();
            UnloadUI();
            AutoloadingManager.UnloadManager(this);
            Noise2DShift = null;
            //BufferPool.ClearBuffers();
            //Main.logo2Texture = ModContent.Request<Texture2D>("Terraria/Logo2").Value;
            //Main.logoTexture = ModContent.Request<Texture2D>("Terraria/Logo").Value;
            //Main.sun2Texture = ModContent.Request<Texture2D>("Terraria/Sun2").Value;
            //Main.sun3Texture = ModContent.Request<Texture2D>("Terraria/Sun3").Value;
            //Main.sunTexture = ModContent.Request<Texture2D>("Terraria/Sun").Value;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EENet.ReceievePacket(reader, whoAmI);
        }

        /*public override void MidUpdateNPCGore()
        {
            MechanicManager.MidUpdateNPCGore();
        }

        public override void MidUpdateDustTime()
        {
            MechanicManager.MidUpdateDustTime();
        }

        //Mechanic Port
        public override void PreUpdateEntities()
        {
            base.PreUpdateEntities();
            MechanicManager.PreUpdateEntities();      
        }

        public override void PostUpdateEverything()
        {
            UpdateVerlet();
        }*/

        /*public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                LegacyGameInterfaceLayer EEInterfaceLayerUI = new LegacyGameInterfaceLayer("EEMod: EEInterface", delegate
                {
                    if (lastGameTime != null)
                    {
                        UI.DrawWithScaleUI(lastGameTime);
                    }

                    return true;
                }, InterfaceScaleType.UI);
                layers.Insert(mouseTextIndex, EEInterfaceLayerUI);
                LegacyGameInterfaceLayer EEInterfaceLayerGame = new LegacyGameInterfaceLayer("EEMod: EEInterface", delegate
                {
                    if (lastGameTime != null)
                    {
                        UI.DrawWithScaleGame(lastGameTime);
                        UpdateGame(lastGameTime);
                        if (Main.worldName == KeyID.CoralReefs)
                        {
                            DrawCR();
                        }
                    }

                    return true;
                }, InterfaceScaleType.Game);
                layers.Insert(mouseTextIndex, EEInterfaceLayerGame);
            }
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
            {
                DrawZipline();
            }

            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (textLayer != -1)
            {
                var computerState = new LegacyGameInterfaceLayer("EE: UI", delegate
                {
                    if (Main.worldName == KeyID.Pyramids || SubworldLibrary.SubworldSystem.IsActive<Sea>() || Main.worldName == KeyID.CoralReefs)
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
		    }
            if (SubworldLibrary.SubworldSystem.IsActive<Sea>())
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
        }*/

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

            RecipeGroup.RegisterGroup("EEMod:Gemstones", group0);
        }
    }
}
