using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Items.Fish;
using EEMod.Projectiles;
using EEMod.Items.Weapons.Mage;
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
using EEMod.Items.Shipyard.Cannonballs;
using EEMod.Items.Shipyard.Cannons;
using EEMod.Items.Shipyard.Figureheads;
using EEMod.Subworlds;
using EEMod.Tiles;

namespace EEMod
{
    public class ShipyardPlayer : ModPlayer
    {
        //Shipyard upgrade vars
        public int cannonType; //Stores the cannon type in terms of Item IDs so the player can retrieve stuff from UI

        public int figureheadType; //See above

        public int boatTier; //Saves the total upgrades of the boat - 0: ordinary, 1: ironclad?

        public Item[] shipStorage; //Stores what the player has in their ship's hold, begins at 20 items, gains 20 on each upgrade

        public bool alreadyLoaded;


        //Seamap transition values
        public bool triggerSeaCutscene;
        public int cutSceneTriggerTimer;
        public float speedOfPan = 1;

        private readonly string SeaTransShader = "EEMod:SeaTrans";


        //Seamap vars
        public float shipSpeed;
        public float steeringSpeed;

        public int defense;
        public int maxHealth;

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            cannonType = ModContent.ItemType<SteelCannon>();
            figureheadType = ModContent.ItemType<WoodenFigurehead>();

            return base.AddStartingItems(mediumCoreDeath);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["cannonType"] = cannonType;
            tag["figureheadType"] = figureheadType;
            tag["boatTier"] = boatTier;

            tag["ShipStorage"] = shipStorage;

            //tag["triggerSeaCutscene"] = triggerSeaCutscene;
            //tag["cutSceneTriggerTimer"] = cutSceneTriggerTimer;

            //tag["shipStorage"] = shipStorage;
        }

        public override void LoadData(TagCompound tag)
        {
            tag.TryGetRef("cannonType", ref cannonType);
            tag.TryGetRef("figureheadType", ref figureheadType);
            tag.TryGetRef("boatTier", ref boatTier);

            //tag.TryGetRef("triggerSeaCutscene", ref triggerSeaCutscene);
            //tag.TryGetRef("cutSceneTriggerTimer", ref cutSceneTriggerTimer);

            tag.TryGet<Item[]>("ShipStorage", out shipStorage);

            //tag.TryGetIntArray("shipStorage", out shipStorage);
        }

        public void UpgradeBoat()
        {
            boatTier++;

            //Handling storage upgrade transfer
            Item[] tempArray = new Item[20];

            //if(boatTier == 0) tempArray = new int[20];
            if (boatTier == 1) tempArray = new Item[40];

            for (int i = 0; i < shipStorage.Length; i++) {
                tempArray[i] = shipStorage[i];
            }

            shipStorage = tempArray;
        }

        public void LeftClickAbility(SeamapPlayerShip boat)
        {
            /*for (int i = 0; i < shipStorage.Length; i++)
            {
                if(shipStorage[i].GetGlobalItem<ShipyardGlobalItem>().Tag == ItemTags.Cannonball && shipStorage[i].stack > 0)
                {
                    shipStorage[i].stack--;

                    //(new Item(cannonType)).GetGlobalItem<ShipyardGlobalItem>().info.LeftClickAbility(boat, shipStorage[i].GetGlobalItem<ShipyardGlobalItem>().info.GetCannonball());
                }
            }*/


            (new Item(cannonType)).GetGlobalItem<ShipyardGlobalItem>().info.LeftClickAbility(
                boat, (new Item(ModContent.ItemType<IronCannonball>())).GetGlobalItem<ShipyardGlobalItem>().info.GetCannonball(Player.team)
                );

            /*try
            {
                for(int i = 0; i < shipStorage.Length; i++)
                {
                    if(shipStorage[i].type == ModContent.ItemType<MeteorCannonball>() ||
                       shipStorage[i].type == ModContent.ItemType<LeadCannonball>() ||
                       shipStorage[i].type == ModContent.ItemType<IronCannonball>())
                    {
                        (new Item(cannonType)).GetGlobalItem<ShipyardGlobalItem>().info.LeftClickAbility(
                            boat, (new Item(shipStorage[i].type)).GetGlobalItem<ShipyardGlobalItem>().info.GetCannonball(Player.team)
                            );
                    }
                }
            }
            catch
            {
                Main.NewText("Something's wrong. Report this to the developers.");

                return;
            }*/
        }

        public void RightClickAbility(SeamapPlayerShip boat)
        {
            try
            {
                (new Item(figureheadType)).GetGlobalItem<ShipyardGlobalItem>().info.RightClickAbility(boat);
            }
            catch
            {
                Main.NewText("Something's wrong. Report this to the developers.");

                return;
            }
        }

        public override void ModifyScreenPosition()
        {
            if (cutSceneTriggerTimer > 0 && triggerSeaCutscene)
            {
                if (!Main.gamePaused)
                {
                    speedOfPan += 0.01f;
                }

                Main.screenPosition.X += cutSceneTriggerTimer * speedOfPan;
            }
        }

        public override void PreUpdate()
        {
            if (!SubworldLibrary.SubworldSystem.IsActive<Sea>())
            {
                if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
                {
                    cutSceneTriggerTimer += 2;
                }
            }

            UpdateCutscenesAndTempShaders();
        }

        public override void PreUpdateMovement()
        {
            base.PreUpdateMovement();

            if (!SubworldLibrary.SubworldSystem.IsActive<Sea>())
            {
                if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
                {
                    Player.controlDown = false;
                    Player.controlHook = false;
                    Player.controlJump = false;
                    Player.controlLeft = false;
                    Player.controlRight = false;
                    Player.controlUp = false;
                    Player.controlThrow = false;
                    Player.controlTorch = false;
                    Player.controlSmart = false;
                    Player.controlMount = false;
                }
            }
        }

        public void UpdateCutscenesAndTempShaders()
        {
            Filters.Scene[SeaTransShader].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (!Filters.Scene[SeaTransShader].IsActive())
            {
                Filters.Scene.Activate(SeaTransShader, Player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }

            if (!triggerSeaCutscene)
            {
                if (Filters.Scene[SeaTransShader].IsActive())
                {
                    Filters.Scene.Deactivate(SeaTransShader);
                }
            }

            if (cutSceneTriggerTimer >= 500)
            {
                Player.GetModPlayer<SeamapPlayer>().EnterSeamap();
            }
        }
    }

    public class ShipyardSystem : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(ModContent.GetInstance<EEMod>(), "Assets/Music/Shipyard");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneBeach && (player.Center.X < (Main.maxTilesX * 16));
        }
    }

    public class ShipyardGlobalItem : GlobalItem
    {
        public ItemTags Tag;

        public ShipyardInfo info;

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
    }

    public enum ItemTags
    {
        Cannonball = 1,
        Cannon = 2,
        Figurehead = 3
    }
}
