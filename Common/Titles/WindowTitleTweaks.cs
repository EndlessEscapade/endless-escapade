using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Titles;

[Autoload(Side = ModSide.Client)]
public sealed class WindowTitleTweaks : ModSystem
{
    // TODO: Make the title change when a language is set.
    public override void PostSetupContent() {
        const int GameTitleCount = 25;
        
        Main.instance.Window.Title = Language.GetTextValue("Mods.EndlessEscapade.GameTitle." + Main.rand.Next(GameTitleCount));
    }
    
    public override void Unload() {
        Main.changeTheTitle = true;
    }
}
