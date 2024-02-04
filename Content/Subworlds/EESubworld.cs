using System;
using System.Collections.Generic;
using EndlessEscapade.Common.Seamap;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Subworlds;

public abstract class EESubworld : Subworld
{
    public static string progressMessage = "";
    public override int Height => 0;
    public override int Width => 0;

    public override List<GenPass> Tasks => new();
    public virtual string subworldKey => KeyID.Island;

    public override void OnExit() {
        Main.LocalPlayer.GetModPlayer<SeamapPlayer>().prevKey = subworldKey;

        base.OnExit();
    }

    public override void DrawMenu(GameTime gameTime) {
        //DrawLoadingScreen();
    }

    public void DrawLoadingScreen() { }
}

public class SubworldGenerationPass : GenPass
{
    private readonly Action<GenerationProgress> method;

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
