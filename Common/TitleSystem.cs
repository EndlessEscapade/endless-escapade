using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common;

[Autoload(Side = ModSide.Client)]
public class TitleSystem : ModSystem
{
    public const int TitleCount = 11;

    public override void PostSetupContent() {
        var selected = Mod.GetLocalizationValue($"Titles.Title{Main.rand.Next(TitleCount)}");
        var title = $"{Mod.DisplayName}: {selected}";
        
        Main.instance.Window.Title = title;
    }

    public override void OnModUnload() {
        Main.changeTheTitle = true;
    }
}