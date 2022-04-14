using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Items.Fish;
using EEMod.Projectiles;
using EEMod.Projectiles;
using EEMod.Items.Weapons.Mage;
using EEMod.Projectiles.Runes;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static EEMod.EEWorld.EEWorld;
using static Terraria.ModLoader.ModContent;
using EEMod.Seamap.Core;
using Terraria.DataStructures;
using System.Linq;

using EEMod.EEWorld;
using EEMod.Players;
using EEMod.Items.Accessories;
using EEMod.Seamap.Content;
using EEMod;

namespace EEMod
{
    public class ShipyardInfo
    {
        public virtual void LeftClickAbility(SeamapPlayerShip boat, SeamapObject cannonball)
        {

        }

        public virtual void RightClickAbility(SeamapPlayerShip boat)
        {

        }

        public virtual SeamapObject GetCannonball(int team) 
        {
            return null;
        }
    }
}
