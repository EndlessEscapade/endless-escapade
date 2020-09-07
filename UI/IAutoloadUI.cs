using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Autoloading.AutoloadTypes;

namespace EEMod.UI
{
    public interface IAutoloadUI : IAutoloadType
    {
        LegacyGameInterfaceLayer GetUILayer(List<GameInterfaceLayer> layers, ref int myIndex, GameTime lastUpdateUIGameTime, UserInterface myUserInterface);
    }
}
