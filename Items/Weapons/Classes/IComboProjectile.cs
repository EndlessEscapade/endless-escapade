using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public interface IComboProjectile
    {
        void SetCombo(int Combo);
    }
}