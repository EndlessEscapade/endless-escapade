using EEMod.Autoloading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using EEMod.ID;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using Terraria.GameContent.Events;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Terraria.DataStructures;
using EEMod.Tiles.EmptyTileArrays;
using System.Linq;
using EEMod.VerletIntegration;
using EEMod.Prim;

using Terraria.UI;
using EEMod.Subworlds;
using EEMod.Subworlds.CoralReefs;

namespace EEMod.ModSystems
{
    public class UIManager : ModSystem
    {
        private readonly Dictionary<string, UserInterface> UIInterfaces = new Dictionary<string, UserInterface>();
        private readonly Dictionary<string, UIState> UIStates = new Dictionary<string, UIState>();
        private readonly Dictionary<UserInterface, UIState> Binds = new Dictionary<UserInterface, UIState>();
        private readonly List<UserInterface> StatesScaledWithGame = new List<UserInterface>();
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            EEMod.UI.Update(gameTime);
            EEMod.lastGameTime = gameTime;
        }
        public void AddUIState(string UIStateName, UIState UiState)
        {
            if (UIStates.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("State name already used");
            }
            UIStates.Add(UIStateName, UiState);
        }

        public void AddInterface(string UIStateName, bool IsScaledWithGame = false, string Bind = "")
        {
            if (UIInterfaces.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("Interface name already used");
            }
            var TheInterface = new UserInterface();
            UIInterfaces.Add(UIStateName, TheInterface);
            if (IsScaledWithGame)
            {
                StatesScaledWithGame.Add(TheInterface);
            }
            if (Bind != "")
            {
                BindInterfaceToState(UIStateName, Bind);
            }
        }

        public void Remove(string UIStateName)
        {
            if (!UIInterfaces.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("State doesn't exist");
            }
            UIInterfaces.Remove(UIStateName);
        }

        public void SetState(string UIInterfaceName, string UIStateName)
        {
            if (!UIInterfaces.ContainsKey(UIInterfaceName) || !UIStates.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("State or Interface Not Found");
            }
            UIInterfaces[UIInterfaceName].SetState(UIStates[UIStateName]);
        }

        public void RemoveState(string UIInterfaceName)
        {
            if (!UIInterfaces.ContainsKey(UIInterfaceName))
            {
                throw new InvalidOperationException("State or Interface Not Found");
            }
            UIInterfaces[UIInterfaceName].SetState(null);
        }

        public void BindInterfaceToState(string UIInterfaceName, string UIStateName)
        {
            if (!UIInterfaces.ContainsKey(UIInterfaceName) || !UIStates.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("State or Interface Not Found");
            }
            Binds.Add(UIInterfaces[UIInterfaceName], UIStates[UIStateName]);
        }

        public void SetToBindedState(string UIInterfaceName)
        {
            if (IsBinded(UIInterfaceName))
                UIInterfaces[UIInterfaceName].SetState(Binds[UIInterfaces[UIInterfaceName]]);
        }
        public UserInterface GetInterface(string UIInterfaceName)
        {
            if (UIInterfaces.ContainsKey(UIInterfaceName))
            {
                return UIInterfaces[UIInterfaceName];
            }
            else
            {
                return new UserInterface();
            }
        }
        public UIState GetState(string UIInterfaceName)
        {
            if (UIInterfaces.ContainsKey(UIInterfaceName))
            {
                return UIInterfaces[UIInterfaceName].CurrentState;
            }
            else
            {
                return new UIState();
            }
        }
        public bool IsActive(string UIInterfaceName) => UIInterfaces[UIInterfaceName].CurrentState != null;

        public bool IsBinded(string UIInterfaceName) => UIInterfaces.ContainsKey(UIInterfaceName) && Binds.ContainsKey(UIInterfaces[UIInterfaceName]);

        public void SwitchBindedState(string UIInterfaceName)
        {
            if (IsBinded(UIInterfaceName))
            {
                if (!IsActive(UIInterfaceName))
                {
                    SetToBindedState(UIInterfaceName);
                }
                else
                {
                    RemoveState(UIInterfaceName);
                }
            }
        }

        public new void Load()
        {
            for (int i = 0; i < UIStates.Count; i++)
            {
                UIStates.Values.ToArray()[i].OnActivate();
            }
        }

        public void UnLoad()
        {
            UIInterfaces.Clear();
            UIStates.Clear();
            Binds.Clear();
        }

        public void Update(GameTime gameTime)
        {
            foreach (UserInterface item in UIInterfaces.Values)
            {
                if (item.CurrentState != null)
                {
                    item.Update(gameTime);
                }
            }
        }

        public void DrawWithScaleUI(GameTime gameTime)
        {
            foreach (UserInterface item in UIInterfaces.Values)
            {
                if (item.CurrentState != null && !StatesScaledWithGame.Contains(item))
                {
                    item.Draw(Main.spriteBatch, gameTime);
                }
            }
        }
        public void DrawWithScaleGame(GameTime gameTime)
        {
            foreach (UserInterface item in UIInterfaces.Values)
            {
                if (item.CurrentState != null && StatesScaledWithGame.Contains(item))
                {
                    item.Draw(Main.spriteBatch, gameTime);
                }
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                LegacyGameInterfaceLayer EEInterfaceLayerUI = new LegacyGameInterfaceLayer("EEMod: EEInterface", delegate
                {
                    if (EEMod.lastGameTime != null)
                    {
                        EEMod.UI.DrawWithScaleUI(EEMod.lastGameTime);
                    }

                    return true;
                }, InterfaceScaleType.UI);
                layers.Insert(mouseTextIndex, EEInterfaceLayerUI);
                LegacyGameInterfaceLayer EEInterfaceLayerGame = new LegacyGameInterfaceLayer("EEMod: EEInterface", delegate
                {
                    if (EEMod.lastGameTime != null)
                    {
                        EEMod.UI.DrawWithScaleGame(EEMod.lastGameTime);
                        //UpdateGame(lastGameTime);
                        if (Main.worldName == KeyID.CoralReefs)
                        {
                            //DrawCR();
                        }
                    }

                    return true;
                }, InterfaceScaleType.Game);
                layers.Insert(mouseTextIndex, EEInterfaceLayerGame);
            }
            //if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
            //{
            //    //DrawZipline();
            //}

            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (textLayer != -1)
            {
                var computerState = new LegacyGameInterfaceLayer("EE: UI", delegate
                {
                    if (SubworldLibrary.SubworldSystem.IsActive<Sea>())
                    {
                        //DrawText();
                    }
                    return true;
                },
                InterfaceScaleType.UI);
                layers.Insert(textLayer, computerState);
            }
            if (SubworldLibrary.SubworldSystem.IsActive<Sea>())
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    var layer = layers[i];

                    if (!layer.Name.Contains("Vanilla: Settings Button"))
                    {
                        layers.RemoveAt(i);
                    }
                }
            }
        }
    }
}