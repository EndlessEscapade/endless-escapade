using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using EndlessEscapade.Common.Seamap;
using Terraria.Graphics.Effects;
using System.Diagnostics;
using Terraria.Audio;
using Terraria.UI.Chat;
using SubworldLibrary;
using Terraria.IO;
using Terraria.GameContent;

namespace EndlessEscapade.Subworlds
{
    public class Sea : EESubworld
    {
        public override int Width => 600;
        public override int Height => 600;

        public override string Name => "Sea";

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SubworldGenerationPass(progress =>
            {
                progress.Message = "Spawning Seamap"; //Sets the text above the worldgen progress bar

	    		Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
	    		Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            })
        };

        public void ReturnHome(Player player) {
            SubworldSystem.Exit();
        }

        public override string subworldKey => KeyID.Sea;
    }

    public class SeaSystem : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(ModContent.GetInstance<EndlessEscapade>(), "Assets/Music/Seamap");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player) {
            return SubworldLibrary.SubworldSystem.IsActive<Sea>();
        }
    }
}