using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace EEMod
{
    public class UIManager : IUpdateableGT
    {

        private readonly Dictionary<string, UserInterface> UIInterfaces = new Dictionary<string, UserInterface>();
        private readonly Dictionary<string, UIState> UIStates = new Dictionary<string, UIState>();
        private readonly Dictionary<UserInterface, UIState> Binds = new Dictionary<UserInterface, UIState>();
        public void AddUIState(string UIStateName, UIState UiState)
        {
            if (UIStates.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("State name already used");
            }
            UIStates.Add(UIStateName, UiState);
        }

        public void AddInterface(string UIStateName, string Bind = "")
        {
            if (UIInterfaces.ContainsKey(UIStateName))
            {
                throw new InvalidOperationException("Interface name already used");
            }
            UIInterfaces.Add(UIStateName, new UserInterface());
            if(Bind != "")
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
            if(IsBinded(UIInterfaceName))
            UIInterfaces[UIInterfaceName].SetState(Binds[UIInterfaces[UIInterfaceName]]);
        }

        public bool isActive(string UIInterfaceName) => UIInterfaces[UIInterfaceName].CurrentState != null;

        public bool IsBinded(string UIInterfaceName) => UIInterfaces.ContainsKey(UIInterfaceName) && Binds.ContainsKey(UIInterfaces[UIInterfaceName]);

        public void SwitchBindedState(string UIInterfaceName)
        {
            if (IsBinded(UIInterfaceName))
            {
                if(!isActive(UIInterfaceName))
                {
                    SetToBindedState(UIInterfaceName);
                }
                else
                {
                    RemoveState(UIInterfaceName);
                }
            }
        }
        public void Load()
        {
            for(int i = 0; i< UIStates.Count; i++)
            {
                UIStates.Values.ToArray()[i].OnActivate();
            }
        }
        public void UnLoad() { UIInterfaces.Clear(); UIStates.Clear(); Binds.Clear(); }
        public void Update(GameTime gameTime)
        {
            foreach(UserInterface item in UIInterfaces.Values)
            {
                if (item.CurrentState != null)
                {
                   item.Update(gameTime);
                }
            }
        }
        public void Draw(GameTime gameTime)
        {
            foreach (UserInterface item in UIInterfaces.Values)
            {
                if (item.CurrentState != null)
                {
                    item.Draw(Main.spriteBatch, gameTime);
                }
            }
        }
    }
}