using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using EEMod.EEWorld;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        private void LoadIL()
        {
            IL.Terraria.Main.DrawMenu += DrawMenuPatch;
        }
        private void UnloadIL()
        {
            IL.Terraria.Main.DrawMenu -= DrawMenuPatch;
        }
        private void DrawMenuPatch(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel ifStatementEnd = null;
            var a = typeof(Main).GetField(nameof(Main.menuMode));
            if (!c.TryGotoNext(i => i.MatchLdsfld(a),
                i => i.MatchLdcI4(-7),
                i => i.MatchBneUn(out ifStatementEnd)))
            {
                Logger.Info("Draw menu's patch failed");
                return;
            }
            if (ifStatementEnd == null)
            {
                Logger.Info("Draw menu's patch's label is null");
                return;
            }

            ILLabel reeee = il.DefineLabel();

            int start = c.Index; // i need to remove this or something
            c.GotoLabel(ifStatementEnd);
            int end = c.Index;
            c.Goto(start);
            // c.RemoveRange(end - start);

            c.Emit(OpCodes.Br, reeee);
            c.Goto(end);
            c.MarkLabel(reeee);

            var focus = typeof(Main).GetField("focusMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            var selectedmenu = typeof(Main).GetField("selectedMenu", BindingFlags.NonPublic | BindingFlags.Instance);

            // TODO: turn this to pure IL (only emits and removes)

            // pushing fields by reference to the delegate (except this)
            c.Emit(OpCodes.Ldarg_0); // this
            c.Emit(OpCodes.Ldarg_0); // this
            c.Emit(OpCodes.Ldflda, focus); // focusmenu
            c.Emit(OpCodes.Ldarg_0); // this
            c.Emit(OpCodes.Ldflda, selectedmenu); // selectedmenu
            c.Emit(OpCodes.Ldloca, 5); // num2
            c.Emit(OpCodes.Ldloca, 7); // num4
            c.Emit(OpCodes.Ldloca, 19); // array4
            c.Emit(OpCodes.Ldloca, 21); // array6
            c.Emit(OpCodes.Ldloca, 26); // array9
            c.Emit(OpCodes.Ldloca, 16); // array
            c.Emit(OpCodes.Ldloca, 8); // num5
            c.Emit(OpCodes.Ldloca, 25); // flag5

            c.Emit(OpCodes.Call, ((modifyingdelegate)GenkaiMenu).Method); // now we call E V E R Y T H I N G that was inside that if statement manually, rip

            // var rgbbs = typeof(Color).GetProperty(nameof(Color.B)).GetSetMethod();

            //if (!c.TryGotoNext(i => i.MatchStloc(172))) // color10
            //    throw new Exception();
            //if (!c.TryGotoNext(i => i.MatchCall(rgbbs)))
            //    throw new Exception();

            //c.Emit(OpCodes.Ldloca, 172);
            //c.Emit(OpCodes.Ldloc, 180);
            // c.Emit(OpCodes.Call, ((colorrefdelegate)ModifyColor).Method);
        }
        //private static void ModifyColor(ref Color color, byte val)
        //{
        // 
        //}
        // private delegate void colorrefdelegate(ref Color color, byte val);
        private delegate void modifyingdelegate(Main instance, ref int focusmenu, ref int selectedmenu, ref int num2, ref int num4, ref int[] array4, ref byte[] array6, ref string[] array9, ref bool[] array, ref int num5, ref bool flag);
#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
        private static void GenkaiMenu(Main instance, ref int focusMenu, ref int selectedMenu, ref int num2, ref int num4, ref int[] array4, ref byte[] array6, ref string[] array9, ref bool[] array, ref int num5, ref bool flag5)
        {
            num2 = 200;
            num4 = 60;
            int offset = -10;
            array4[2] = 30 + offset - 1; //30 - 20; // 30
            array4[3] = 30 + offset - 3 - 1; //30 - 10; // 30
            array6[3] = 2; //2; // rarity // 2
            array4[4] = 70; // 70
            array4[5] = -40 + offset / 2 - 1; // -40 - 10;
            array6[5] = 5;
            if (focusMenu == 2)
            {
                array9[0] = Language.GetTextValue("UI.NormalDescriptionFlavor");
                array9[1] = Language.GetTextValue("UI.NormalDescription");
            }
            else if (focusMenu == 3)
            {
                array9[0] = Language.GetTextValue("UI.ExpertDescriptionFlavor");
                array9[1] = Language.GetTextValue("UI.ExpertDescription");
            }
            else if (focusMenu == 5) // Genkai's
            {
                array9[0] = "Not for easily angried";
                array9[1] = "(What'll it be? Who knows, find out ;])";
            }
            else
            {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                array9[0] = Lang.menu[32].Value;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            }
            array[0] = true;
            array[1] = true;

            array9[2] = Language.GetTextValue("UI.Normal");
            array9[3] = Language.GetTextValue("UI.Expert");
            array9[4] = Language.GetTextValue("UI.Back");
            array9[5] = "Genkai"; // Genkai
            num5 = 6;
            if (selectedMenu == 2)
            {
                Main.expertMode = false;
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = 7;
                if (Main.SettingsUnlock_WorldEvil)
                {
                    Main.menuMode = -71;
                }
            }
            else if (selectedMenu == 3)
            {
                Main.expertMode = true;
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = 7;
                if (Main.SettingsUnlock_WorldEvil)
                {
                    Main.menuMode = -71;
                }
            }
            else if (selectedMenu == 5) // Genkai's
            {
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = Main.SettingsUnlock_WorldEvil ? -71 : 7;
                Main.expertMode = true;
                EEWorld.EEWorld.GenkaiMode = true;
            }
            else if (selectedMenu == 4 || flag5)
            {
                flag5 = false;
                Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                Main.menuMode = 16;
            }
            Main.clrInput();
        }
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
    }
}