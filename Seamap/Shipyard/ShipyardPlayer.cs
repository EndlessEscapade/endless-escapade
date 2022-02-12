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
using EEMod.Seamap.Core;
using Terraria.DataStructures;
using System.Linq;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.EEWorld;
using EEMod.Players;
using EEMod.Items.Accessories;
using EEMod.Seamap.Content;
using EEMod;
using EEMod.Items.Shipyard.Cannonballs;
using EEMod.Items.Shipyard.Cannons;
using EEMod.Items.Shipyard.Figureheads;

namespace EEMod
{
    public class ShipyardPlayer : ModPlayer
    {
        //Shipyard upgrade vars
        public int cannonType; //Stores the cannon type in terms of Item IDs so the player can retrieve stuff from UI
        public Dictionary<int, ShipyardInfo> cannons = new Dictionary<int, ShipyardInfo>()
        {
            {ModContent.ItemType<SteelCannon>(), new SteelCannonInfo()},
        };

        public int figureheadType; //See above
        public Dictionary<int, ShipyardInfo> figureheads = new Dictionary<int, ShipyardInfo>()
        {
            {ModContent.ItemType<WoodenFigurehead>(), new WoodenFigureheadInfo()},
        };

        public int boatTier; //Saves the total upgrades of the boat - 0: ordinary, 1: ironclad?

        public Item[] shipStorage; //Stores what the player has in their ship's hold, begins at 20 items, gains 20 on each upgrade

        public Dictionary<int, ShipyardInfo> cannonballs = new Dictionary<int, ShipyardInfo>()
        {
            {ModContent.ItemType<IronCannonball>(), new IronCannonballInfo()},
            {ModContent.ItemType<LeadCannonball>(), new LeadCannonballInfo()},
            {ModContent.ItemType<MeteorCannonball>(), new MeteorCannonballInfo()},
        };


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
            
            //tag["shipStorage"] = shipStorage;
        }

        public override void LoadData(TagCompound tag)
        {
            tag.TryGetRef("cannonType", ref cannonType);
            tag.TryGetRef("figureheadType", ref figureheadType);
            tag.TryGetRef("boatTier", ref boatTier);
            
            if(boatTier == 0) shipStorage = new Item[20];
            if(boatTier == 1) shipStorage = new Item[40];
            
            //tag.TryGetIntArray("shipStorage", out shipStorage);
        }
        
        public void UpgradeBoat() 
        {
            boatTier++;

            //Handling storage upgrade transfer
            Item[] tempArray = new Item[20];

            //if(boatTier == 0) tempArray = new int[20];
            if (boatTier == 1) tempArray = new Item[40];
            
            for(int i = 0; i < shipStorage.Length; i++) {
                tempArray[i] = shipStorage[i];
            }
            
            shipStorage = tempArray;
        }
        
        public void LeftClickAbility(SeamapPlayerShip boat)
        {
            for (int i = 0; i < shipStorage.Length; i++)
            {
                if((shipStorage[i].type == ModContent.ItemType<MeteorCannonball>() ||
                    shipStorage[i].type == ModContent.ItemType<IronCannonball>() ||
                    shipStorage[i].type == ModContent.ItemType<LeadCannonball>()
                    ) && shipStorage[i].stack > 0)
                {
                    shipStorage[i].stack--;

                    cannons[cannonType].LeftClickAbility(boat, cannonballs[shipStorage[i].type].GetCannonball());
                }
            }
        }

        public void RightClickAbility(SeamapPlayerShip boat)
        {
            figureheads[figureheadType].RightClickAbility(boat);
        }
    }
}
