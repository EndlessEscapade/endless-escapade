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
        public UIManager UI;
        public void LoadUI()
        {
            if (!Main.dedServ)
            {
                UI = new UIManager();
                UI.AddUIState("RunUI", new RunninUI());
                UI.AddUIState("MBUI", new MerchantBoatUI());
                UI.AddUIState("EEUI", new EEUI());
                UI.AddInterface("CustomResources");
                //autobind
                UI.AddInterface("SpeedrunnTimer", "RunUI");
                UI.AddInterface("MerchantBoatUI", "MBUI");
                UI.AddInterface("EEInterface", "EEUI");
            }
        }
        public void UnloadUI()
        {
            UI.UnLoad();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            OnUpdateUI?.Invoke(gameTime);
            UI.Update(gameTime);
            lastGameTime = gameTime;
            base.UpdateUI(gameTime);
            if (RuneActivator.JustPressed && delay == 0)
            {
                UI.SwitchBindedState("EEInterface");
                if (UI.isActive("EEInterface"))
                {
                    if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Deactivate("EEMod:Pause");
                    }
                }
                else
                {
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Activate("EEMod:Pause").GetShader().UseOpacity(pauseShaderTImer);
                    }
                }
                delay++;
            }
            if (UI.isActive("EEInterface"))
            {
                Filters.Scene["EEMod:Pause"].GetShader().UseOpacity(pauseShaderTImer);
                pauseShaderTImer += 50;
                if (pauseShaderTImer > 1000)
                {
                    pauseShaderTImer = 1000;
                }
            }
            else
            {
                pauseShaderTImer = 0;
            }
            if (delay > 0)
            {
                delay++;
                if (delay == 60)
                {
                    delay = 0;
                }
            }
        }
    }
}
