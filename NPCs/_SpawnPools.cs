using EEMod.ID;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Summon;
using EEMod.NPCs.SurfaceReefs;
using EEMod.NPCs.ThermalVents;
using EEMod.NPCs.KelpForest;
using EEMod.NPCs.KelpForest.Kelpweaver;
using EEMod.NPCs.UpperReefs;
using EEMod.NPCs.TropicalIslands;
using EEMod.NPCs.LowerReefs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Players;

namespace EEMod.NPCs
{
    public class SpawnPools : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player spawnplayer = spawnInfo.Player;
            if (spawnplayer.GetModPlayer<EEZonePlayer>().ZoneCoralReefs)
            {
                pool[0] = 0f;
                if (spawnInfo.Player.Center.Y < ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                {
                    if (!spawnplayer.GetModPlayer<EEPlayer>().jellyfishMigration)
                    {
                        pool.Add(ModContent.NPCType<HermitCrab>(), 5f);
                        pool.Add(ModContent.NPCType<FlowerHatJelly>(), 5f);
                        pool.Add(ModContent.NPCType<SeaSlug>(), 5f);
                        pool.Add(ModContent.NPCType<OlvinicSnail>(), 5f);
                        pool.Add(NPCID.Shark, 5f);
                    }
                    else
                    {
                        pool.Add(ModContent.NPCType<FlowerHatJelly>(), 5f);
                        //pool.Add(ModContent.NPCType<CombJelly>(), 5f);
                        //pool.Add(ModContent.NPCType<CrownJelly>(), 5f);
                        pool.Add(ModContent.NPCType<OlvinicSnail>(), 5f);
                    }
                }

                else if (spawnplayer.Center.Y <= 12800 && spawnInfo.Player.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16 && spawnInfo.Player.GetModPlayer<EEZonePlayer>().reefMinibiomeID == MinibiomeID.None)
                {
                    pool.Add(ModContent.NPCType<LunaJelly>(), 5f);
                    pool.Add(ModContent.NPCType<SeaSlug>(), 5f);
                    pool.Add(ModContent.NPCType<Seahorse>(), 5f);
                }

                else if (spawnplayer.Center.Y > 12800 && spawnInfo.Player.GetModPlayer<EEZonePlayer>().reefMinibiomeID == MinibiomeID.None)
                {
                    pool.Add(ModContent.NPCType<SeaSlug>(), 0.5f);
                }

                if (spawnInfo.Player.GetModPlayer<EEZonePlayer>().reefMinibiomeID == MinibiomeID.KelpForest)
                {
                    pool[0] = 0f;
                    pool.Add(ModContent.NPCType<GlowingKelpSpider>(), 0.1f);
                }
            }

            if (Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2)
            {
                pool[0] = 0f;
                pool.Add(ModContent.NPCType<CoconutCrab>(), 0.5f);
                pool.Add(ModContent.NPCType<Cococritter>(), 0.5f);
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2 && !Main.dayTime)
            {
                pool[0] = 0f;
                pool.Add(ModContent.NPCType<CoconutSpider>(), 0.5f);
            }

            if (spawnplayer.ZoneBeach)
            {
                //pool.Add(ModContent.NPCType<FlowerHatJelly>(), 5f);
                //pool.Add(ModContent.NPCType<SeaSlug>(), 5f);

                //pool.Add(ModContent.NPCType<Cococritter>());
                //pool.Add(NPCID.PinkJellyfish, 5f);
                //pool.Add(NPCID.Shark, 5f);
            }
        }
    }
}