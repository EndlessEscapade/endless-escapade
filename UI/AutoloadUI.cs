using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Autoloading;
using EEMod.Autoloading.AutoloadTypes;
using EEMod.Extensions;
using EEMod.UI;

namespace EEMod.UI
{
#pragma warning disable IDE0051 // Remove unused private members
    public sealed class AutoloadUI : AutoloadTypeManager<IAutoloadUI> // TODO: Test (AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA)
    {
        [FieldInit]
        private static Dictionary<Type, UIHelper> UIs;
        public static T GetUIState<T>() where T : UIState => UIs.TryGetValue(typeof(T), out var val) ? val.uistate as T : null;
        public static UserInterface GetUserInterfaceFor<T>() where T : UIState => UIs.TryGetValue(typeof(T), out var val) ? val.userInterface : null;
        public static bool IsVisible<T>() where T : UIState
        {
            return UIs.TryGetValue(typeof(T), out var ui) && ui.Visible;
        }
        public static void IsVisible<T>(bool value) where T : UIState
        {
            if (UIs.TryGetValue(typeof(T), out var ui))
                ui.Visible = value;
        }
        public override void CreateInstance(Type type)
        {
            if (type.IsSubclassOf(typeof(UIState)))
                if (type.TryCreateInstance(out UIState state))
                {
                    UIs.Add(type, new UIHelper(state, new UserInterface(), true));
                }
        }


        private static void UpdateUIs(GameTime gameTime)
        {
            foreach (var m in UIs.Values)
            {
                if (m.Visible)
                    m.userInterface.Update(gameTime);
            }
        }

        private static void ModifyLayersUI(List<GameInterfaceLayer> layers, int mouseTextIndex, GameTime lastUpdateUIGameTime)
        {
            if (mouseTextIndex != -1)
            {
                foreach (var ui in UIs)
                {
                    if (ui.Value is IAutoloadUI s)
                    {
                        int index = mouseTextIndex;
                        var layer = s.GetUILayer(layers, ref index, lastUpdateUIGameTime, ui.Value.userInterface);
                        if (layer != null)
                            layers.Insert(index, layer);
                    }
                }
            }
        }

        [LoadingMethod]
        private static void SubscribeToEvents()
        {
            EEMod.OnModifyInterfaceLayers += ModifyLayersUI;
            EEMod.OnUpdateUI += UpdateUIs;
        }
        [UnloadingMethod]
        private static new void Unload()
        {
            UIs?.Clear();
            UIs = null;
        }
    }
}
