using EEMod.Systems.Subworlds.EESubworlds;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace EEMod.Systems.Subworlds
{
    public abstract class Subworld
    {
        public virtual string Name => "Subworld";

        public virtual Point Dimensions => Point.Zero;

        public virtual Point SpawnTile => Point.Zero;

        internal void Generate(int seed, GenerationProgress customProgressObject = null) 
        {
            Main.maxTilesX = Dimensions.X;
            Main.maxTilesY = Dimensions.Y;
            Main.spawnTileX = SpawnTile.X;
            Main.spawnTileY = SpawnTile.Y;

            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            WorldGeneration(seed, customProgressObject);

            // EEMod.Subworlds.IsSaving = false;
        }

        internal virtual void WorldGeneration(int seed, GenerationProgress customProgressObject = null) { }

        internal virtual void PlayerUpdate(Player player) { }
    }
}