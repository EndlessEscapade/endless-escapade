using EEMod.Autoloading;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles.OceanMap;
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
        public UserInterface customResources;
        public UserInterface SpeedrunnTimer;
        public UserInterface MerchantBoatUI;
        
        internal RunninUI RunUI;
        internal MerchantBoatUI MBUI;
        internal EEUI eeui;
        

        internal bool EEUIVisible
        {
            get => EEInterface?.CurrentState != null;
            set => EEInterface?.SetState(value ? eeui : null);
        }
        internal bool MerchantBoatUIVisible
        {
            get => MerchantBoatUI?.CurrentState != null;
            set => MerchantBoatUI?.SetState(value ? eeui : null);
        }
        public void LoadUI()
        {
            if (!Main.dedServ)
            {
                eeui = new EEUI();
                MBUI = new MerchantBoatUI();
                eeui.Activate();
                MBUI.Activate();
                EEInterface = new UserInterface();
                MerchantBoatUI = new UserInterface();
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            OnUpdateUI?.Invoke(gameTime);
            lastGameTime = gameTime;
            if (EEInterface?.CurrentState != null)
            {
                EEInterface.Update(gameTime);
            }
            base.UpdateUI(gameTime);

            if (RuneActivator.JustPressed && delay == 0)
            {
                if (EEUIVisible)
                {
                    EEUIVisible = false;
                    if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Deactivate("EEMod:Pause");
                    }
                }
                else
                {
                    EEUIVisible = true;
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Activate("EEMod:Pause").GetShader().UseOpacity(pauseShaderTImer);
                    }
                }
                delay++;
            }
            if (EEUIVisible)
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

            if (SpeedrunnTimer?.CurrentState != null)
            {
                RunUI.Update(gameTime);
            }
            if (MerchantBoatUI?.CurrentState != null)
            {
                MBUI.Update(gameTime);
            }
        }
    }
}
