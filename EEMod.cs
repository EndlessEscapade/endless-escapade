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
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader.IO;
using EEMod.ModSystems;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static bool isSaving = false;

        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;

        public static GameTime lastGameTime;

        public static ModKeybind Inspect;

        public static Noise2D Noise2D;
        public static ParticleZoneHandler Particles;
        internal static ParticleZone MainParticles;
        public UserInterface EEInterface;
        public ComponentManager<TileObjVisual> TVH;

        public override void Load()
        {
            TVH = new ComponentManager<TileObjVisual>();
            verlet = new Verlet();

            //TagSerializer.AddSerializer(new BigCrystalSerializer());
            //TagSerializer.AddSerializer(new EmptyTileEntitySerializer());
            //TagSerializer.AddSerializer(new CrystalSerializer());

            Inspect = KeybindLoader.RegisterKeybind(this, "Inspect", Keys.OemCloseBrackets);

            if (!Main.dedServ)
            {
                PrimitiveSystem.primitives = new PrimTrailManager();
            }

            Main.QueueMainThreadAction(() =>
            {
                if (!Main.dedServ)
                {
                    AutoloadingManager.LoadManager(this);

                    LoadUI();
                }

                LoadIL();
                LoadDetours();

                if (!Main.dedServ)
                {
                    Particles = new ParticleZoneHandler();
                    Particles.AddZone("Main", 40000);
                    MainParticles = Particles.Get("Main");

                    PrimitiveSystem.primitives.Load();
                }
            });

            //Example
            //LayeredMusic.Groups[GetSoundSlot(SoundType.Music, "Sounds/Music/UpperReefs")] = "AquamarineGroup";
            //LayeredMusic.Groups[GetSoundSlot(SoundType.Music, "Sounds/Music/LowerReefs")] = "AquamarineGroup";

            MusicLoader.AddMusic(this, "Assets/Music/SurfaceReefs");
        }

        public override void Unload()
        {
            Inspect = null;
            simpleGame = null;

            UnloadIL();
            UnloadDetours();
            UnloadUI();

            AutoloadingManager.UnloadManager(this);

            Noise2DShift = null;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EENet.ReceievePacket(reader, whoAmI);
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup gemstoneGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gemstones", new int[]
            {
                ItemID.Amber,
                ItemID.Amethyst,
                ItemID.Diamond,
                ItemID.Emerald,
                ItemID.Ruby,
                ItemID.Sapphire,
                ItemID.Topaz
            });

            RecipeGroup.RegisterGroup("EEMod:Gemstones", gemstoneGroup);
        }
    }
}
