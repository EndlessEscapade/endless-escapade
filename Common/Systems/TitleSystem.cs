using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems;

[Autoload(Side = ModSide.Client)]
public class TitleSystem : ModSystem
{
    // tModLoader doesn't allow for the use of HJSON lists.
    public const int TitleCount = 11;

    public override void PostSetupContent() {
        var selected = Mod.GetLocalizationValue($"Titles.Title{Main.rand.Next(TitleCount)}");
        var title = $"{Mod.DisplayName}: {selected}";

        Main.instance.Window.Title = title;
    }

    public override void Unload() { Main.changeTheTitle = true; }
}
