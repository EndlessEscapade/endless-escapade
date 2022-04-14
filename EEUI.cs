using EEMod.Autoloading;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public void LoadUI()
        {
            if (!Main.dedServ)
            {
                UI.AddUIState("RunUI", new RunninUI());
                UI.AddUIState("EEUI", new EEUI());

                UI.AddInterface("CustomResources");
                UI.AddInterface("SpeedrunnTimer", false, "RunUI");
                UI.AddInterface("EEInterface", true, "EEUI"); //Not sure if it's true

                UI.SwitchBindedState("ArrowInterface");
            }
        }
        public void UnloadUI()
        {
            UI.UnLoad();
        }
        public void UIControls()
        {

        }
    }
}
