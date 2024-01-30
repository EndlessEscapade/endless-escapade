using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Titles;

[Autoload(Side = ModSide.Client)]
public sealed class CustomWindowTitles : ILoadable
{
    private const int GameTitleCount = 25;

    void ILoadable.Load(Mod mod) {
        // This patch completely replaces Terraria's window titles by custom titles, changing them upon language selection.
        IL_Main.DrawMenu += DrawMenuPatch;
    }

    void ILoadable.Unload() { }

    private static void DrawMenuPatch(ILContext il) {
        try {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(1))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(CustomWindowTitles)} disabled: Failed to match IL instruction: {nameof(OpCodes.Ldc_I4_1)}");
                return;
            }

            if (!c.TryGotoNext(i => i.MatchStsfld<Main>(nameof(Main.changeTheTitle)))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(CustomWindowTitles)} disabled: Failed to match IL instruction: {nameof(OpCodes.Stsfld)}");
                return;
            }

            c.Index++;

            c.EmitDelegate(() => {
                Main.changeTheTitle = false;
                Main.instance.Window.Title = Language.GetTextValue("Mods.EndlessEscapade.GameTitle." + Main.rand.Next(GameTitleCount));
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(1))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(CustomWindowTitles)} disabled: Failed to match IL instruction: {nameof(OpCodes.Ldc_I4_1)}");
                return;
            }

            if (!c.TryGotoNext(i => i.MatchStsfld<Main>(nameof(Main.changeTheTitle)))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(CustomWindowTitles)} disabled: Failed to match IL instruction: {nameof(OpCodes.Stsfld)}");
                return;
            }

            c.EmitDelegate(() => {
                Main.changeTheTitle = false;
                Main.instance.Window.Title = Language.GetTextValue("Mods.EndlessEscapade.GameTitle." + Main.rand.Next(GameTitleCount));
            });
        }
        catch (Exception) {
            MonoModHooks.DumpIL(EndlessEscapade.Instance, il);
        }
    }
}
