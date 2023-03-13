using EndlessEscapade.Common.Configuration;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common;

[Autoload(Side = ModSide.Client)]
public class WindowTitleSystem : ModSystem
{
    public override void OnWorldLoad() {
        if (Main.instance.Window == null || !MiscellaneousConfig.Instance.EnableWindowTitles) {
            return;
        }

        Main.changeTheTitle = false;

        const int titleCount = 12;

        string title = Mod.GetTextValue($"WindowTitles.Title{Main.rand.Next(titleCount)}");
        string windowTitle = $"Endless Escapade: {title}";

        Main.instance.Window.Title = windowTitle;
    }

    public override void OnWorldUnload() {
        if (Main.instance.Window == null || !MiscellaneousConfig.Instance.EnableWindowTitles) {
            return;
        }
        
        Main.changeTheTitle = true;
    }
}