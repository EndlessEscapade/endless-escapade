using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Titles;

[Autoload(Side = ModSide.Client)]
public sealed class WindowTitleTweaks : ModSystem
{
    public override void Load() {
        IL_Main.DrawMenu += DrawMenuPatch;
    }

    public override void Unload() {
        Main.changeTheTitle = true;
    }

    private static void SetTitle() {
        const int GameTitleCount = 25;
        
        Main.changeTheTitle = false;
        Main.instance.Window.Title = Language.GetTextValue("Mods.EndlessEscapade.GameTitle." + Main.rand.Next(GameTitleCount));
    }
    
    private void DrawMenuPatch(ILContext il) {
        try {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(1))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(WindowTitleTweaks)} disabled: Failed to match IL.");
                return;
            }

            c.EmitDelegate(SetTitle);
        }
        catch (Exception exception) {
            MonoModHooks.DumpIL(Mod, il);
        }
    }
}
