using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public abstract class EEProjectile : ModProjectile
    {
        public Projectile Projectile => base.projectile; // for 1.4 port
    }
}
