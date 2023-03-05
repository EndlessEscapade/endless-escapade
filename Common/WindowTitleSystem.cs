using EndlessEscapade.Common.Configuration;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Common;

[Autoload(Side = ModSide.Client)]
public class WindowTitleSystem : ModSystem
{
    public override void OnWorldLoad() {
        if (Main.instance.Window == null || !ClientSideConfig.Instance.EnableWindowTitles) {
            return;
        }

        Main.changeTheTitle = false;

        const int titleCount = 12;

        string title = Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.WindowTitles.Title{Main.rand.Next(titleCount)}");
        string windowTitle = $"Endless Escapade: {title}";

        Main.instance.Window.Title = windowTitle;
    }

    public override void OnWorldUnload() {
        Main.changeTheTitle = true;
    }
}