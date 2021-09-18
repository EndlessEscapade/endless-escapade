using EEMod.Autoloading;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs.CoralReefs;
using EEMod.Skies;
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
                UI.AddUIState("ArrowUI", new ArrowsUIState());
                UI.AddUIState("RunUI", new RunninUI());
                UI.AddUIState("MBUI", new MerchantBoatUI());
                UI.AddUIState("EEUI", new EEUI());
                UI.AddInterface("CustomResources");
                //autobind
                UI.AddInterface("SpeedrunnTimer", false, "RunUI");
                UI.AddInterface("MerchantBoatUI", false, "MBUI"); //Not sure if it's false
                UI.AddInterface("EEInterface", true, "EEUI"); //Not sure if it's true
                UI.AddInterface("ArrowInterface", false, "ArrowUI");

                UI.SwitchBindedState("ArrowInterface");
            }
        }
        public void UnloadUI()
        {
            UI.UnLoad();
        }
        /*public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            UI.Update(gameTime);
            lastGameTime = gameTime;
            UIControls();
        }*/
        public void UIControls()
        {
            if (RuneActivator.JustPressed && delay == 0)
            {
                UI.SwitchBindedState("EEInterface");
            }
        }
    }
}
