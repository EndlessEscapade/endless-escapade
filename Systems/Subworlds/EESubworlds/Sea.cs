
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using EEMod.ID;
using EEMod.Tiles;
using EEMod.VerletIntegration;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.WorldBuilding;

namespace EEMod.Systems.Subworlds.EESubworlds
{
    public class Sea : Subworld
    {
        public override Point Dimensions => new Point(400, 405);

        public override Point SpawnTile => new Point(234, 92);

        public override string Name => "Sea";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {

        }
        internal override void PlayerUpdate(Player player)
        {
           
        }
    }
}