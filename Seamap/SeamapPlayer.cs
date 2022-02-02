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
        //Shipyard upgrade vars
        public int cannonType; //Stores the cannon type in terms of Item IDs so the player can retrieve stuff from UI
        public int figureheadType; //See above
        public int boatTier; //Saves the total upgrades of the boat - 0: ordinary, 1: ironclad?
        
        public byte[] shipStorage; //Stores what the player has in their ship's hold, begins at 20 items, gains 20 on each upgrade
        
        
        //Seamap vars
        public float shipSpeed;
        public float steeringSpeed;
        
        public int defense;
        public int maxHealth;
        
        
        
        public override void SaveData(TagCompound tag)
        {
            tag["cannonType"] = cannonType;
            tag["figureheadType"] = figureheadType;
            tag["boatTier"] = boatTier;
            
            tag["shipStorage"] = shipStorage;
        }

        public override void LoadData(TagCompound tag)
        {
            tag.TryGetByteArrayRef("cannonType", ref cannonType);
            tag.TryGetRef("figureheadType", ref figureheadType);
            tag.TryGetRef("boatTier", ref boatTier);
            
            if(boatTier == 0) shipStorage = new byte[20];
            if(boatTier == 1) shipStorage = new byte[40];
            
            tag.TryGetByteArrayRef("shipStorage", ref shipStorage);
        }
        
        public void UpgradeBoat() {
            boatTier++;
            
            //Handling storage upgrade transfer
            byte[] tempArray;
            
            //if(boatTier == 0) tempArray = new byte[20];
            if(boatTier == 1) tempArray = new byte[40];
            
            for(int i = 0; i < shipStorage.Length; i++) {
                tempArray[i] = shipStorage[i];
            }
            
            shipStorage = tempArray;
            
            
        }
    }
}
