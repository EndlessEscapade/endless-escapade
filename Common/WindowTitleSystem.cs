using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace EndlessEscapade.Common;

[Autoload(Side = ModSide.Client)]
public class WindowTitleSystem : ModSystem
{
    public const int TitleCount = 12;

    public override void OnWorldLoad() {
        if (Main.instance.Window == null) {
            return;
        }

        Main.changeTheTitle = false;
        
        string title = Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.WindowTitles.Title{Main.rand.Next(1, TitleCount + 1)}");
        string windowTitle = $"Endless Escapade: {title}";

        Main.instance.Window.Title = windowTitle;
    }

    public override void OnWorldUnload() {
        Main.changeTheTitle = true;
    }
}