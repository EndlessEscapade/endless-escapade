using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace EndlessEscapade.Common.FishermansLogUI;

public class FishermansLogUISystem : ModSystem
{
    public static ModKeybind ResetUIKeybind { get; private set; }

    public static UserInterface FishermansLogUserInterface;

    private GameTime lastUpdateUiGameTime;

    public override void Load() {
        ResetUIKeybind = KeybindLoader.RegisterKeybind(Mod, "Reset UI (FOR TESTING PURPOSES)", "R");

        if (!Main.dedServ) {
            FishermansLogUIState FishermansLogUI = new();
            FishermansLogUserInterface = new UserInterface();
            FishermansLogUserInterface.SetState(FishermansLogUI);
            FishermansLogUI.Activate();
        }
    }

    public override void Unload() {
        ResetUIKeybind = null;
    }

    public override void UpdateUI(GameTime gameTime) {
        lastUpdateUiGameTime = gameTime;

        if (FishermansLogUIState.Visible) {
            FishermansLogUserInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1) {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("EndlessEscapade: FishermansLogUserInterface", delegate {
                if (lastUpdateUiGameTime != null && FishermansLogUIState.Visible) FishermansLogUserInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                return true;
            }, InterfaceScaleType.UI));
        }
    }
}

public class TemporaryPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet) {
        if (FishermansLogUISystem.ResetUIKeybind.JustPressed) {
            Main.NewText("UI Reset");

            FishermansLogUIState FishermansLogUI = new();
            FishermansLogUISystem.FishermansLogUserInterface = new UserInterface();
            FishermansLogUISystem.FishermansLogUserInterface.SetState(FishermansLogUI);
            FishermansLogUI.Activate();
        }
    }
}