using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EndlessEscapade.Common.Config;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace EndlessEscapade.Common.Systems.Debug;

internal class ReloadNRejoin : ModSystem
{
    private static string characterName, worldName;
    private static bool reloadModOnFoundModSources;

    internal static bool loaded;

    private static bool enterCharacterSelectMenu;

    private static bool reloadModSources;

    // internal static string ModSourcesPathOverride;
    public static void SetRestart(string characterName, string worldName, bool isreload) {
        reloadModSources = isreload;
        reloadModOnFoundModSources = isreload;
        Environment.SetEnvironmentVariable("EE_REJOINPLAYER", characterName);
        Environment.SetEnvironmentVariable("EE_REJOINWORLD", worldName);
    }

    public override void Load() {
        if (DebugConfig.Instance?.EnableReloadNRejoin is true) {
            MonoModHooks.Add(
                typeof(ModContent).Assembly.GetType("Terraria.ModLoader.Core.ModOrganizer")!.GetMethod("SaveLastLaunchedMods", BindingFlags.NonPublic | BindingFlags.Static)!,
                (Action orig) => {
                    orig();
                    if (Environment.GetEnvironmentVariable("EE_REJOINACTIVE") is "1") {
                        characterName = Environment.GetEnvironmentVariable("EE_REJOINPLAYER");
                        worldName = Environment.GetEnvironmentVariable("EE_REJOINWORLD");
                        enterCharacterSelectMenu = true;
                        Environment.SetEnvironmentVariable("EE_REJOINACTIVE", null);
                    }
                    //enterCharacterSelectMenu = true;
                }
            );
            MonoModHooks.Modify(
                typeof(ModContent).Assembly.GetType("Terraria.ModLoader.UI.UIModSources").GetMethod("Populate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public),
                IL_UIModSources_Populate_TriggerSomethingAfterModFolderPopulation
            );
            IL_Main.DrawMenu += IL_Main_DrawMenu_SwitchToModSourcesMenu;
            ;
            On_Main.DrawMenu += Main_DrawMenu;
            loaded = true;
        }
    }

    public override void OnModLoad() {
        base.OnModLoad();
    }

    public override void OnWorldLoad() {
        base.OnWorldLoad();
    }

    private static void OnFinishLoadingModSources() {
        if (!reloadModOnFoundModSources) {
            return;
        }

        Console.WriteLine("RNR: Finished loading mod sources");
        const BindingFlags instanceflags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var ui = typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.Interface").GetField("modSources", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        var items = GetFieldValue(ui, "_items") as IList;
        var entry = items.Cast<object>().FirstOrDefault(t => GetFieldValue(t, "modName").Equals(nameof(EndlessEscapade))) as UIPanel;
        if (entry == null) {
            EndlessEscapade.Instance.Logger.Debug("RNR: Mod was not found within the mod list?");
            return;
        }

        entry.GetType().GetMethod("BuildAndReload", instanceflags).Invoke(entry, new object[] { null, null });

        Environment.SetEnvironmentVariable("EE_REJOINACTIVE", "1");

        static object GetFieldValue(object obj, string fieldName) {
            return obj.GetType().GetField(fieldName, instanceflags).GetValue(obj);
        }
    }

    private static void IL_UIModSources_Populate_TriggerSomethingAfterModFolderPopulation(ILContext il) {
        var c = new ILCursor(il);
        c.GotoNext(MoveType.Before, t => t.MatchPop());
        c.Emit(OpCodes.Dup);
        c.EmitDelegate<Action<Task>>(task => task.ContinueWith(t => Task.Run(OnFinishLoadingModSources)));
    }

    private static void IL_Main_DrawMenu_SwitchToModSourcesMenu(ILContext il) {
        var c = new ILCursor(il);
        if (c.TryGotoNext(i => i.MatchCall("Terraria.ModLoader.UI.Interface", "ModLoaderMenus"))) {
            //c.Emit(OpCodes.Ldsflda, typeof(Main).GetField(nameof(Main.menuMode)));
            c.EmitDelegate(
                static () => {
                    if (reloadModSources) {
                        Main.menuMode = 10001;
                        reloadModSources = false;
                        //Environment.SetEnvironmentVariable("EE_REJOINACTIVE", "1", EnvironmentVariableTarget.Process);
                    }
                }
            );
        }
        else {
            EndlessEscapade.Instance.Logger.Debug("RNR: IL for changing menu mode failed");
        }
    }

    private void Main_DrawMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime) {
        orig(self, gameTime);
        if (reloadModSources) {
            var was10001before = Main.menuMode == 10001;
            Main.menuMode = 10001;

            if (was10001before) { }

            reloadModSources = false;
        }

        if (enterCharacterSelectMenu) {
            enterCharacterSelectMenu = false;
            Environment.SetEnvironmentVariable("EE_REJOINACTIVE", null);
            Environment.SetEnvironmentVariable("EE_REJOINPLAYER", null);
            Environment.SetEnvironmentVariable("EE_REJOINWORLD", null);
            Main.OpenCharacterSelectUI();

            var player = Main.PlayerList.FirstOrDefault(d => d.Name == characterName); // put player name here
            if (player == null) {
                Mod.Logger.Error("RNR: Player could not be found?");
                return;
            }

            Main.SelectPlayer(player);

            Main.OpenWorldSelectUI();
            var worldSelectUI = (UIWorldSelect)typeof(Main).GetField("_worldSelectMenu", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!.GetValue(null!)!;
            var worldListUI = (UIList)typeof(UIWorldSelect).GetField("_worldList", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!.GetValue(worldSelectUI)!;
            var world = worldListUI._items.OfType<UIWorldListItem>()
                .FirstOrDefault(
                    d => {
                        return ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(d)!).Name
                               == worldName; // put world name here
                    }
                );
            if (world == null) {
                Mod.Logger.Error("RNR: World could not be found?");
                return;
            }

            typeof(UIWorldListItem).GetMethod("PlayGame", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!.Invoke(
                world,
                new object[] { new UIMouseEvent(world, world.GetDimensions().Position()), world }
            );
        }
    }
}

internal class PlayerReloadNRejoinMessageDisplay : ModPlayer
{
    public override void OnEnterWorld() {
        base.OnEnterWorld();
        if (ReloadNRejoin.loaded) {
            Main.NewText($"ReloadNRejoin command [/rnr (save|nosave)?] is enabled, default save on exit: {!DebugConfig.Instance.ReloadNRejoinExitNoSaveByDefault}");
        }
    }
}

internal class ReloadNRejoinCommand : ModCommand
{
    public override string Command => "rnr";

    public override string Usage => "/rnr (nosave|save)? (onexit)?";

    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args) {
        if (Main.netMode != NetmodeID.SinglePlayer) {
            caller.Reply("This command is only available in singleplayer (and if the config is enabled).");
            return;
        }

        /*if (!DebugConfig.Instance.EnableDebugMode) {
            caller.Reply("To use reloadnrejoin debug mode must be enabled");
            return;
        }*/
        if (DebugConfig.Instance?.EnableReloadNRejoin is not true) {
            caller.Reply("To use reloadnrejoin you must enable it in the mod config first and reload the mod");
            return;
        }

        if (!ReloadNRejoin.loaded) {
            caller.Reply("The mod config has reloadnrejoin enabled but it requires a mod reload");
            return;
        }

        var save = DebugConfig.Instance.ReloadNRejoinExitNoSaveByDefault;
        if (args is { Length: > 0 }) {
            if (args[0] is "help" or "/?" or "/help" or "h" or "?") {
                caller.Reply("Usage:  " + Usage);
            }

            if (args[0] == "nosave") {
                save = false;
            }
            else if (args[0] == "save") {
                save = true;
            }
        }

        if (save) {
            ReloadNRejoin.SetRestart(Main.LocalPlayer.name, Main.ActiveWorldFileData.Name, true);
            Main.gameMenu = true;
        }
        else {
            var playerName = Main.LocalPlayer.name;
            var worldName = Main.ActiveWorldFileData.Name;
            WorldGen.SaveAndQuit(() => { ReloadNRejoin.SetRestart(playerName, worldName, true); });
        }
    }
}
