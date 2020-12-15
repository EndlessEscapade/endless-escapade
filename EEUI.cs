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
using Terraria.World.Generation;

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
                UI.AddInterface("SpeedrunnTimer", "RunUI");
                UI.AddInterface("MerchantBoatUI", "MBUI");
                UI.AddInterface("EEInterface", "EEUI");
                UI.AddInterface("ArrowInterface", "ArrowUI");

                UI.SwitchBindedState("ArrowInterface");
            }
        }
        public void UnloadUI()
        {
            UI.UnLoad();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            UI.Update(gameTime);
            lastGameTime = gameTime;
            UIControls();
            base.UpdateUI(gameTime);
        }
        public void UIControls()
        {
            if (RuneActivator.JustPressed && delay == 0)
            {
                UI.SwitchBindedState("EEInterface");
            }
        }
    }
}
