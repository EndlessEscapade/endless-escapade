using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using EEMod.ID;
using EEMod.Tiles;
using EEMod.VerletIntegration;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.WorldBuilding;
using EEMod.Seamap.Core;
using Terraria.Graphics.Effects;
using System.Diagnostics;
using Terraria.Audio;
using EEMod.Seamap.Content;
using EEMod.Seamap.Content.Islands;
using EEMod.Seamap;
using EEMod;
using Terraria.UI.Chat;
//using EEMod.Subworlds.CoralReefs;
using SubworldLibrary;
using Terraria.IO;
using Terraria.GameContent;

namespace EEMod.Subworlds
{
    public class Sea : Subworld
    {
        public override int Width => 600;
        public override int Height => 600;

        public override string Name => "Sea";

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SeaGenPass(progress =>
            {
                progress.Message = "Spawning Seamap"; //Sets the text above the worldgen progress bar

	    		Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
	    		Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            })
        };

        public void ReturnHome(Player player)
        {
            SubworldSystem.Exit();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            Main.spriteBatch.Begin();

            ModContent.GetInstance<EEMod>().DrawLoadingScreen();

            Main.spriteBatch.End();

            return;
        }

        public override void OnEnter()
        {
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().cutSceneTriggerTimer = 0;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().triggerSeaCutscene = false;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().speedOfPan = 0;

            base.OnEnter();
        }

        public override void OnExit()
        {
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().cutSceneTriggerTimer = 0;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().triggerSeaCutscene = false;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().speedOfPan = 0;

            base.OnExit();
        }
    }

    public class SeaSystem : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(ModContent.GetInstance<EEMod>(), "Assets/Music/Seamap");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            return SubworldLibrary.SubworldSystem.IsActive<Sea>();
        }
    }

    public class SeaGenPass : GenPass
    {
        private Action<GenerationProgress> method;

        public SeaGenPass(Action<GenerationProgress> method) : base("", 1)
        {
            this.method = method;
        }

        public SeaGenPass(float weight, Action<GenerationProgress> method) : base("", weight)
        {
            this.method = method;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            method(progress);
        }
    }
}