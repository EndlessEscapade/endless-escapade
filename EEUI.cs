using EEMod.Autoloading;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.ModSystems;
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
        public FishermansLogUI FishermansLogUI;
        public KelpArmorAmmoUI KelpArmorAmmoUI;
        public IndicatorsUI IndicatorsUI;
        public DialogueUI DialogueUI;
        public ShipLoadoutUI ShipLoadoutUI;

        public static UIManager UI;

        public void LoadUI()
        {
            UI = new UIManager();

            FishermansLogUI = new FishermansLogUI();
            FishermansLogUI.Activate();
            UI.AddInterface("EEInterfacee");
            UI.AddUIState("FishermansLogUI", FishermansLogUI);

            KelpArmorAmmoUI = new KelpArmorAmmoUI();
            KelpArmorAmmoUI.Activate();
            UI.AddInterface("KelpArmorAmmoInterface");
            UI.AddUIState("KelpArmorAmmoUI", KelpArmorAmmoUI);

            IndicatorsUI = new IndicatorsUI();
            IndicatorsUI.Activate();
            UI.AddInterface("IndicatorsInterface");
            UI.AddUIState("IndicatorsUI", IndicatorsUI);

            DialogueUI = new DialogueUI();
            DialogueUI.Activate();
            UI.AddInterface("DialogueInterface");
            UI.AddUIState("DialogueUI", DialogueUI);

            ShipLoadoutUI = new ShipLoadoutUI();
            ShipLoadoutUI.Activate();
            UI.AddInterface("ShipLoadoutInterface");
            UI.AddUIState("ShipLoadoutUI", ShipLoadoutUI);

            if (!Main.dedServ)
            {
                UI.AddInterface("CustomResources");

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
