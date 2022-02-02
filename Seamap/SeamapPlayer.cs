using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Items.Fish;
using EEMod.Projectiles;
using EEMod.Projectiles.Armor;
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
using EEMod.Seamap.SeamapContent;
using Terraria.DataStructures;
using System.Linq;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.EEWorld;
using EEMod.Players;
using EEMod.Items.Accessories;

namespace EEMod
{
    public class SeamapPlayer : ModPlayer
    {
        public int cannonType; //Stores the cannon type in terms of Item IDs so the player can retrieve stuff from UI
        public int figureheadType; //See above
        public int boatTier; //Saves the total upgrades of the boat - 0: ordinary, 1: ironclad?
        
        public int[] shipStorage = new int[40]; //Stores what the player has in their ship's hold, begins at 40 items
        
        //TODO I'll work on establishing this class later :thumb: and making the proper tile classes for the ship
        
        //public override void Initialize() {
        //
        //}
        
        public override void SaveData(TagCompound tag)
        {
            tag["cannonType"] = cannonType;
            tag["figureheadType"] = figureheadType;
            tag["boatTier"] = boatTier;
        }

        public override void LoadData(TagCompound tag)
        {
            tag.TryGetByteArrayRef("cannonType", ref cannonType);
            tag.TryGetRef("figureheadType", ref figureheadType);
            tag.TryGetRef("boatTier", ref boatTier);
        }
    }
}
