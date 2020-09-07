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

namespace EEMod.UI
{
    public class UIHelper
    {
        public UIState uistate;
        public UserInterface userInterface;
        public bool Visible
        {
            get => userInterface?.CurrentState != null;
            set => userInterface?.SetState(value ? uistate : null);
        }
        public UIHelper(UIState state, UserInterface userInterface, bool activate = false)
        {
            this.uistate = state;
            this.userInterface = userInterface;
            if (activate)
                state.Activate();
        }
        private UIHelper() { }
    }
}
