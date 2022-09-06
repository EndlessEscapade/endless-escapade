using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EEMod.Config
{
    internal class EEOptimizationsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Tooltip("Use multiple threads for generating perlin noise")]
        public bool MultithreadPerlinNoise { get; set; }
    }
}
