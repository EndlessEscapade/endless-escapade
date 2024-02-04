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
using System.Diagnostics;
using SubworldLibrary;
using Terraria.IO;
using Terraria.DataStructures;
using EndlessEscapade.Common.Seamap;

namespace EndlessEscapade.Subworlds
{
    public abstract class EESubworld : Subworld
    {
        public override int Height => 0;
        public override int Width => 0;

        public override List<GenPass> Tasks => new List<GenPass>();
        public virtual string subworldKey => KeyID.Island;

        public static string progressMessage = "";

        public override void OnExit() {
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().prevKey = subworldKey;

            base.OnExit();
        }

        public override void DrawMenu(GameTime gameTime) {
            //DrawLoadingScreen();

            return;
        }

        public void DrawLoadingScreen() {

        }
    }

    public class SubworldGenerationPass : GenPass
    {
        private Action<GenerationProgress> method;

        public SubworldGenerationPass(Action<GenerationProgress> method) : base("", 1) {
            this.method = method;
        }

        public SubworldGenerationPass(float weight, Action<GenerationProgress> method) : base("", weight) {
            this.method = method;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
            method(progress);
        }
    }

    public class KeyID
    {
        public const string Sea = "Sea";
        public const string Island = "Island";
    }
}